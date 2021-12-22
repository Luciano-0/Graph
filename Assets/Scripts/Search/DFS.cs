using System.Collections.Generic;
using UnityEngine;

public static class DFS<T>
{
    private static List<Node<T>> result = new List<Node<T>>();

    private static Graph<T> graph;

    private static HashSet<Node<T>> visited = new HashSet<Node<T>>();

    // 递归版本
    public static List<Node<T>> DFSSearchRecursive(Graph<T> g, Node<T> start)
    {
        result.Clear();
        if (g == null || g.NodeNum == 0)
        {
            Debug.LogError("图为空！");
            return result;
        }

        graph = g;
        visited.Clear();
        DFSCore(start);
        return result;
    }

    private static void DFSCore(Node<T> node)
    {
        visited.Add(node);
        result.Add(node);
        var adjNode = graph[node].FirstAdjNode;
        while (adjNode != null)
        {
            if (!visited.Contains(adjNode.Data))
            {
                DFSCore(adjNode.Data);
            }
            adjNode = adjNode.Next;
        }
    }


    // 非递归版本
    public static List<Node<T>> DFSSearchStack(Graph<T> graph,Node<T> start)
    {
        if (graph == null || graph.NodeNum == 0)
        {
            Debug.LogError("图为空！");
            return new List<Node<T>>();
        }

        var stack = new Stack<Node<T>>();
        var visited = new HashSet<Node<T>>();
        var res = new List<Node<T>>();
        stack.Push(start);
        while (stack.Count > 0)
        {
            var node = stack.Pop();
            if (visited.Contains(node)) continue;
            visited.Add(node);
            res.Add(node);
            var adjNode = graph[node].FirstAdjNode;
            while (adjNode != null)
            {
                stack.Push(adjNode.Data);
                adjNode = adjNode.Next;
            }
        }
        return res;
    }
}