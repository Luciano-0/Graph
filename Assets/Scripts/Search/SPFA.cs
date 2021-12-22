using System.Collections.Generic;

public static class SPFA<T>
{
    private class Distance
    {
        public float value = float.MaxValue;
        public List<Node<T>> path = new List<Node<T>>();
    }

    public static List<Node<T>> SPFASearch(Graph<T> graph, Node<T> start, Node<T> end)
    {
        if (graph == null || start == null || !graph.IsNode(start)) return new List<Node<T>>();
        Dictionary<Node<T>, Distance> dis = new Dictionary<Node<T>, Distance>();
        Queue<Node<T>> queue = new Queue<Node<T>>();
        foreach (var vn in graph)
        {
            dis[vn.Data] = new Distance();
        }

        dis[start].value = 0;
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Node<T> fNode = queue.Dequeue();
            var adjNode = graph[fNode].FirstAdjNode;
            while (adjNode != null)
            {
                var d = dis[adjNode.Data];
                if (adjNode.Cost + dis[fNode].value < d.value)
                {
                    d.value = adjNode.Cost + dis[fNode].value;
                    d.path = new List<Node<T>>(dis[fNode].path) {fNode};
                    if (!queue.Contains(adjNode.Data)) queue.Enqueue(adjNode.Data);
                }

                adjNode = adjNode.Next;
            }
        }

        if (dis[end].path.Count > 0) dis[end].path.Add(end);
        return dis[end].path;
    }
}