public interface IGraph<T>
{
    bool IsDigraph { get; set; }
    int NodeNum { get; }
    int EdgeNum { get; }
    bool IsNode(Node<T> node);
    bool HasEdge(Node<T> from, Node<T> to);
    void AddNode(Node<T> node);
    void AddEdge(Node<T> from, Node<T> to, int cost);
    void RemoveEdge(Node<T> from, Node<T> to);
    void ChangeEdgeCost(Node<T> from, Node<T> to, float cost);
}