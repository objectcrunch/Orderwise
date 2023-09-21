namespace CoreApplication;
public class DataNode<T>
{
    public T Data { get; }

    public DataNode<T>? Next { get; set; }
    public DataNode(T data)
    {
        Data = data;
    }
}
