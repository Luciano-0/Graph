using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 图类
/// </summary>
public class Graph<T> : IEnumerable<VexNode<T>>, IGraph<T>
{
    public Graph(Node<T>[] data)
    {
        NodeDic = new Dictionary<Node<T>, VexNode<T>>();
        foreach (var t in data)
        {
            NodeDic[t] = new VexNode<T>(t);
        }
    }

    public Graph()
    {
        NodeDic = new Dictionary<Node<T>, VexNode<T>>();
    }

    // 是否为有向图
    public bool IsDigraph { get; set; }

    // 顶点数目
    public int NodeNum => NodeDic.Count;

    // 边的数目
    public int EdgeNum
    {
        get
        {
            var count = 0;
            foreach (var node in NodeDic.Values)
            {
                var p = node.FirstAdjNode;
                while (p != null)
                {
                    ++count;
                    p = p.Next;
                }
            }

            return IsDigraph ? count : count / 2;
        }
    }

    public VexNode<T> this[Node<T> node]
    {
        get => NodeDic[node];
        set => NodeDic[node] = value;
    }


    private Dictionary<Node<T>, VexNode<T>> NodeDic;


    // 判断节点是否属于图
    public bool IsNode(Node<T> node)
    {
        return NodeDic.TryGetValue(node, out var _);
    }

    // 判断两点之间是否有边
    public bool HasEdge(Node<T> from, Node<T> to)
    {
        if (!IsNode(from) || !IsNode(to))
        {
            Debug.Log("节点不属于图！");
            return false;
        }

        var p = NodeDic[from].FirstAdjNode;
        while (p != null)
        {
            if (p.Data == to)
            {
                return true;
            }

            p = p.Next;
        }

        return false;
    }

    // 添加节点
    public void AddNode(Node<T> node)
    {
        if (IsNode(node))
        {
            Debug.LogError("已经存在节点！");
            return;
        }

        NodeDic[node] = new VexNode<T>(node);
    }

    // 在两节点之间添加边
    public void AddEdge(Node<T> from, Node<T> to, int cost = 1)
    {
        if (!IsNode(from) || !IsNode(to))
        {
            Debug.LogError("节点不属于图！");
            return;
        }

        if (HasEdge(from, to))
        {
            Debug.LogError("边已经存在！");
            return;
        }

        var p = new AdjNode<T>(to, cost);
        var firstNode = NodeDic[from].FirstAdjNode;
        if (firstNode == null)
        {
            NodeDic[from].FirstAdjNode = p;
        }
        else
        {
            p.Next = firstNode;
            NodeDic[from].FirstAdjNode = p;
        }

        if (IsDigraph) return;
        p = new AdjNode<T>(from, cost);
        firstNode = NodeDic[to].FirstAdjNode;
        if (firstNode == null)
        {
            NodeDic[to].FirstAdjNode = p;
        }
        else
        {
            p.Next = firstNode;
            NodeDic[to].FirstAdjNode = p;
        }
    }

    // 删除边
    public void RemoveEdge(Node<T> from, Node<T> to)
    {
        if (!IsNode(from) || !IsNode(to))
        {
            Debug.LogError("节点不属于图！");
            return;
        }

        if (!HasEdge(from, to))
        {
            Debug.LogError("边不存在！");
            return;
        }

        var p = NodeDic[from].FirstAdjNode;
        AdjNode<T> pre = null;
        while (p != null)
        {
            if (p.Data == to) break;
            pre = p;
            p = p.Next;
        }

        pre.Next = p.Next;

        if (IsDigraph) return;

        p = NodeDic[to].FirstAdjNode;
        pre = null;
        while (p != null)
        {
            if (p.Data == from) break;
            pre = p;
            p = p.Next;
        }

        pre.Next = p.Next;
    }

    // 改变边的值
    public void ChangeEdgeCost(Node<T> from, Node<T> to, float cost)
    {
        if (!IsNode(from) || !IsNode(to))
        {
            Debug.LogError("节点不属于图！");
            return;
        }

        if (!HasEdge(from, to))
        {
            Debug.LogError("边不存在！");
            return;
        }

        var p = NodeDic[from].FirstAdjNode;
        while (p != null)
        {
            if (p.Data == to) break;
            p = p.Next;
        }

        p.Cost = cost;

        if (IsDigraph) return;
        p = NodeDic[to].FirstAdjNode;
        while (p != null)
        {
            if (p.Data == from) break;
            p = p.Next;
        }

        p.Cost = cost;
    }

    public IEnumerator<VexNode<T>> GetEnumerator()
    {
        return NodeDic.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}