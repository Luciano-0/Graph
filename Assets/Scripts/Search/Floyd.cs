using System.Collections.Generic;

public static class Floyd<T>
{
    public static List<Node<T>> FloydSearch(Graph<T> graph, Node<T> start, Node<T> end)
    {
        var count = graph.NodeNum;

        // 初始化图邻接矩阵
        float[,] matrix = new float[count, count];
        var indexDic = new Dictionary<Node<T>, int>();
        var nodeList = new Node<T>[count];
        int i = 0;
        foreach (var node in graph)
        {
            indexDic[node.Data] = i;
            nodeList[i] = node.Data;
            i++;
        }

        i = 0;
        foreach (var node in graph)
        {
            for (int j = 0; j < count; j++)
            {
                if (i == j) matrix[i, j] = 0;
                matrix[i, j] = float.MaxValue;
            }

            var adjNode = node.FirstAdjNode;
            while (adjNode != null)
            {
                matrix[i, indexDic[adjNode.Data]] = adjNode.Cost;
                adjNode = adjNode.Next;
            }

            i++;
        }

        // 初始化path矩阵
        int[,] path = new int[count, count];
        for (i = 0; i < count; i++)
        {
            for (int j = 0; j < count; j++)
            {
                path[i, j] = -1;
            }
        }

        // 核心算法
        for (int k = 0; k < count; k++)
        {
            for (i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (matrix[i, j] > matrix[i, k] + matrix[k, j])
                    {
                        matrix[i, j] = matrix[i, k] + matrix[k, j];
                        path[i, j] = k;
                    }
                }
            }
        }

        // 读取路径
        var res = new List<Node<T>>();
        var startIndex = indexDic[start];
        var endIndex = indexDic[end];
        var temp = endIndex;
        while (temp != startIndex && temp != -1)
        {
            res.Add(nodeList[temp]);
            temp = path[startIndex, temp];
        }
        res.Add(start);
        res.Reverse();

        return res;
    }
}