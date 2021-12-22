/// <summary>
///  邻接表节点类
/// </summary>
/// <typeparam name="T"></typeparam>
public class AdjNode<T>
{
    public AdjNode(Node<T> data, float cost = 0, AdjNode<T> next = null)
    {
        Data = data;
        Next = next;
        Cost = cost;
    }

    public Node<T> Data { get;}
    
    public float Cost { get; set; }
    public AdjNode<T> Next { get; set; }
}