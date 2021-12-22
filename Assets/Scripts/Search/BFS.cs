using System.Collections.Generic;
using UnityEngine;

public static class BFS<T>
{
    public static List<Node<T>> BFSSearchRecursive(Graph<T> graph, Node<T> start)
    {
        if (graph == null || graph.NodeNum == 0)
        {
            Debug.LogError("图为空！");
            return new List<Node<T>>();
        }

        var queue = new Queue<Node<T>>();
        var visited = new HashSet<Node<T>>();
        var res = new List<Node<T>>();
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (visited.Contains(node)) continue;
            visited.Add(node);
            res.Add(node);
            var adjNode = graph[node].FirstAdjNode;
            while (adjNode != null)
            {
                queue.Enqueue(adjNode.Data);
                adjNode = adjNode.Next;
            }
        }
        return res;
    }
}