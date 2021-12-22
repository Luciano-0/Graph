/// <summary>
/// 邻接表顶点节点类
/// </summary>
/// <typeparam name="T"></typeparam>
public class VexNode<T>
{
    public VexNode(Node<T> data, AdjNode<T> adjNode = null)
    {
        Data = data;
        FirstAdjNode = adjNode;
    }

    public Node<T> Data { get; }
    public AdjNode<T> FirstAdjNode { get; set; }
}