using System.Collections.Generic;

public static class Dijkstra<T>
{
    private class Distance
    {
        public bool found;
        public float value = float.MaxValue;
        public List<Node<T>> path = new List<Node<T>>();
    }

    public static List<Node<T>> DijkstraSearch(Graph<T> graph, Node<T> start, Node<T> end)
    {
        if (graph == null || start == null || !graph.IsNode(start)) return new List<Node<T>>();
        Dictionary<Node<T>, Distance> dis = new Dictionary<Node<T>, Distance>();
        foreach (var vn in graph)
        {
            dis[vn.Data] = new Distance();
        }

        dis[start].value = 0;

        while (!dis[end].found)
        {
            Node<T> minNode = null;
            float temp = float.MaxValue;
            //TODO: 使用堆优化
            foreach (var kv in dis)
            {
                if (kv.Value.value < temp && kv.Value.found == false)
                {
                    minNode = kv.Key;
                    temp = kv.Value.value;
                }
            }

            if (minNode == null) break;
            dis[minNode].found = true;
            var adjNode = graph[minNode].FirstAdjNode;
            while (adjNode != null)
            {
                var d = dis[adjNode.Data];
                if (d.found == false && adjNode.Cost + dis[minNode].value < d.value)
                {
                    d.value = adjNode.Cost + dis[minNode].value;
                    d.path = new List<Node<T>>(dis[minNode].path) {minNode};
                }

                adjNode = adjNode.Next;
            }
        }

        if (dis[end].path.Count > 0) dis[end].path.Add(end);
        return dis[end].path;
    }
}