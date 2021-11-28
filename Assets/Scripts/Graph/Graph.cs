using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

/// <summary>
/// 图类
/// </summary>
public class Graph<T>
{
    public Graph(Node<T>[] data)
    {
        NodeList = new List<VexNode<T>>(data.Length);
        for (int i = 0; i < data.Length; i++)
        {
            NodeList[i].Data = data[i];
            NodeList[i].FirstAdjListNode = null;
        }
    }

    // 是否为有向图
    public bool IsDigraph { get; set; }

    // 顶点数目
    public int NodeNum => NodeList.Count;

    // 边的数目
    public int EdgeNum
    {
        get
        {
            var count = 0;
            foreach (var node in NodeList)
            {
                var p = node.FirstAdjListNode;
                while (p != null)
                {
                    ++count;
                    p = p.Next;
                }
            }

            return IsDigraph ? count : count / 2;
        }
    }

    public VexNode<T> this[int index]
    {
        get => NodeList[index];
        set => NodeList[index] = value;
    }


    private List<VexNode<T>> NodeList;


    // 判断节点是否属于图
    public bool IsNode(Node<T> node)
    {
        return NodeList.Find((n) => n.Data == node) != null;
    }

    // 获取节点在邻接表数组中的索引
    public int GetIndex(Node<T> node)
    {
        return NodeList.FindIndex(n => n.Data == node);
    }

    // 判断两点之间是否有边
    public bool HasEdge(Node<T> from, Node<T> to)
    {
        if (!IsNode(from) || !IsNode(to))
        {
            Debug.Log("节点不属于图！");
            return false;
        }

        var p = NodeList[GetIndex(from)].FirstAdjListNode;
        while (p != null)
        {
            if (p.Index == GetIndex(to))
            {
                return true;
            }

            p = p.Next;
        }

        return false;
    }

    // 在两节点之间添加边
    public void AddEdge(Node<T> from, Node<T> to, int v)
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

        var fromIndex = GetIndex(from);
        var toIndex = GetIndex(to);

        var p = new AdjListNode<T>(toIndex);
        var firstNode = NodeList[fromIndex].FirstAdjListNode;
        if (firstNode == null)
        {
            NodeList[fromIndex].FirstAdjListNode = p;
        }
        else
        {
            p.Next = firstNode;
            NodeList[fromIndex].FirstAdjListNode = p;
        }

        if (IsDigraph) return;
        p = new AdjListNode<T>(fromIndex);
        firstNode = NodeList[toIndex].FirstAdjListNode;
        if (firstNode == null)
        {
            NodeList[toIndex].FirstAdjListNode = p;
        }
        else
        {
            p.Next = firstNode;
            NodeList[fromIndex].FirstAdjListNode = p;
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
        
    }
}