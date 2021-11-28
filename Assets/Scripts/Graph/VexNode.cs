/// <summary>
/// 邻接表顶点节点类
/// </summary>
/// <typeparam name="T"></typeparam>
public class VexNode<T>
{
    public VexNode(Node<T> data, AdjListNode<T> adjList =null)
    {
        Data = data;
        FirstAdjListNode = adjList;
    }
    public Node<T> Data { get; set; }
    public AdjListNode<T> FirstAdjListNode { get; set; }
    
}