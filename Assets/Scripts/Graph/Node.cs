/// <summary>
///  节点数据类
/// </summary>
/// <typeparam name="T"></typeparam>
public class Node<T>
{
    public Node(T data)
    {
        Data = data;
    }

    public T Data { get; }
}