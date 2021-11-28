/// <summary>
///  邻接表节点类
/// </summary>
/// <typeparam name="T"></typeparam>
public class AdjListNode<T>
{
    public AdjListNode(int index, AdjListNode<T> next = null)
    {
        Index = index;
        Next = next;
    }

    public int Index { get; set; }
    public AdjListNode<T> Next { get; set; }
}