using System;
using System.Collections.Generic;

public static class AStar<T>
{
    private class Distance
    {
        public bool found;
        // F = G + H
        public float f = float.MaxValue;
        public float g = float.MaxValue;
        public List<Node<T>> path = new List<Node<T>>();
    }

    public static List<Node<T>> AStarSearch(Graph<T> graph, Node<T> start, Node<T> end,
        Func<Node<T>, Node<T>, float> heuristic)
    {
        if (graph == null || start == null || !graph.IsNode(start)) return new List<Node<T>>();
        Dictionary<Node<T>, Distance> dis = new Dictionary<Node<T>, Distance>();
        foreach (var vn in graph)
        {
            dis[vn.Data] = new Distance();
        }

        dis[start].f = 0;
        dis[start].g = 0;

        while (!dis[end].found)
        {
            Node<T> minNode = null;
            float temp = float.MaxValue;
            //TODO: 使用堆优化
            foreach (var kv in dis)
            {
                if (kv.Value.f < temp && kv.Value.found == false)
                {
                    minNode = kv.Key;
                    temp = kv.Value.f;
                }
            }

            if (minNode == null) break;
            dis[minNode].found = true;
            var adjNode = graph[minNode].FirstAdjNode;
            while (adjNode != null)
            {
                var d = dis[adjNode.Data];
                if (d.found)
                {
                    adjNode = adjNode.Next;
                    continue;
                }

                if (adjNode.Cost + dis[minNode].g < d.g)
                {
                    d.g = adjNode.Cost + dis[minNode].g;
                    d.f = d.g + heuristic(adjNode.Data, end);
                    d.path = new List<Node<T>>(dis[minNode].path) {minNode};
                }

                adjNode = adjNode.Next;
            }
        }

        if (dis[end].path.Count > 0) dis[end].path.Add(end);
        return dis[end].path;
    }
}