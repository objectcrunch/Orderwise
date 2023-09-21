using CoreApplication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Windows;

namespace Calculator;

public partial class App : Application
{
    private readonly IHost _appHost;
    public App()
    {
        var builder = Host.CreateApplicationBuilder();

        builder.Environment.ContentRootPath = Directory.GetCurrentDirectory();

        builder.Configuration.AddJsonFile(
            "appsettings.json",
            optional: true,
            reloadOnChange: true);

        builder.Configuration.AddEnvironmentVariables();

        builder.Services.Configure<Options>(builder.Configuration.GetSection("settings"));

        builder.Services.AddCoreProviders();

        builder.Services.AddSingleton<MainWindow>();
       

        _appHost = builder.Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _appHost.StartAsync();

        var startupForm = _appHost.Services.GetRequiredService<MainWindow>();
        startupForm.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _appHost.StopAsync();

        base.OnExit(e);
    }
}
