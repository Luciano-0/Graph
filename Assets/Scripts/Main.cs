using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Main : MonoBehaviour
{
    public static Main Instance;
    public GameObject itemPrefab;
    public GameObject itemsRoot;
    public GameObject linePrefab;
    public GameObject linesRoot;
    public Camera mainCamera;
    public RectTransform canvasRT;
    public Button searchButton;
    public Toggle isDigraphToggle;
    public Toggle showCostToggle;
    public Toggle autoCostToggle;
    public Dropdown searchDropdown;
    public Dropdown startNodeDropdown;
    public Dropdown endNodeDropdown;
    public Color startColor;
    public Color endColor;
    public GameObject costInputRoot;
    public GameObject costInputPrefab;


    private readonly Dictionary<Node<int>, NodeItem> NodeDic = new Dictionary<Node<int>, NodeItem>();
    private readonly List<EdgeItem> EdgeList = new List<EdgeItem>();
    private Graph<int> graph;
    private int count;


    private void Awake()
    {
        Instance = this;
        graph = new Graph<int> {IsDigraph = isDigraphToggle.isOn};
        searchButton.onClick.AddListener(OnSearchClick);
        isDigraphToggle.onValueChanged.AddListener(OnIsDigraphToggleValueChanged);
        showCostToggle.onValueChanged.AddListener(OnShowCostToggleValueChanged);
        searchDropdown.onValueChanged.AddListener(OnSearchDropdownValueChanged);
        autoCostToggle.onValueChanged.AddListener(OnAutoCostToggleValueChanged);
    }


    public void OnClick()
    {
        if (itemPrefab == null || itemsRoot == null) return;
        var node = Instantiate(itemPrefab, itemsRoot.transform).GetComponent<NodeItem>();
        node.gameObject.SetActive(true);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, Input.mousePosition, null, out var p);
        ((RectTransform) node.transform).anchoredPosition = p;
        node.SetNum(count);
        graph.AddNode(node.data);
        count++;
        NodeDic[node.data] = node;
        RefreshDropdown();
    }


    public EdgeItem OnAddEdgeStart(NodeItem node)
    {
        var edgeItem = Instantiate(linePrefab, linesRoot.transform).GetComponent<EdgeItem>();
        edgeItem.gameObject.SetActive(true);
        edgeItem.SetStartPoint(CanvasPositionToWorldPosition(node.transform.position));
        var mousePosition = Input.mousePosition;
        edgeItem.SetEndPoint(mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y,
            -mainCamera.transform.position.z)));
        SetEdgeColor(edgeItem);
        return edgeItem;
    }

    public bool OnAddEdgeEnd(NodeItem node, EdgeItem edge)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, Input.mousePosition, null, out var endPos);
        var minDis = 10000f;
        NodeItem minNode = null;
        foreach (var n in NodeDic.Values)
        {
            if (n == node) continue;
            var rect = (RectTransform) n.transform;
            var nodePos = rect.anchoredPosition;
            var dis = Vector2.Distance(nodePos, endPos);
            if (dis > rect.rect.width / 2) continue;
            if (dis < minDis)
            {
                minDis = dis;
                minNode = n;
            }
        }

        if (minNode == null) return false;
        if (graph.HasEdge(node.data, minNode.data)) return false;
        graph.AddEdge(node.data, minNode.data);
        node.AddStartEdgeList(edge);
        minNode.AddEndEdgeList(edge);
        edge.StartNode = node;
        edge.EndNode = minNode;
        EdgeList.Add(edge);
        edge.SetEndPoint(CanvasPositionToWorldPosition(minNode.transform.position));
        edge.SetCostField();
        return true;
    }

    public void OnChangeEdgeCost(EdgeItem edge, float cost)
    {
        graph.ChangeEdgeCost(edge.StartNode.data, edge.EndNode.data, cost);
    }

    private void OnSearchClick()
    {
        int.TryParse(startNodeDropdown.captionText.text, out var startVal);
        int.TryParse(endNodeDropdown.captionText.text, out var endVal);
        Node<int> start = null, end = null;
        foreach (var n in NodeDic.Keys)
        {
            if (n.Data == startVal) start = n;
            if (n.Data == endVal) end = n;
        }

        switch (searchDropdown.value)
        {
            case 0:
                DFS(start);
                break;
            case 1:
                BFS(start);
                break;
            case 2:
                Dijkstra(start, end);
                break;
            case 3:
                AStar(start, end);
                break;
            case 4:
                SPFA(start, end);
                break;
            case 5:
                FloydSearch(start, end);
                break;
            default:
                Debug.LogError("未实现该搜索方法！");
                break;
        }
    }


    public Vector3 CanvasPositionToWorldPosition(Vector3 pos)
    {
        var sX = pos.x / canvasRT.rect.width * Screen.width;
        var sY = pos.y / canvasRT.rect.height * Screen.height;
        var sVec = new Vector3(sX, sY, -mainCamera.transform.position.z);
        return mainCamera.ScreenToWorldPoint(sVec);
    }

    private void DFS(Node<int> start)
    {
        var res = DFS<int>.DFSSearchStack(graph, start);
        SetPath(res);
    }

    private void BFS(Node<int> start)
    {
        var res = BFS<int>.BFSSearchRecursive(graph, start);
        SetPath(res);
    }

    private void Dijkstra(Node<int> start, Node<int> end)
    {
        var res = Dijkstra<int>.DijkstraSearch(graph, start, end);
        SetPath(res);
        if (res.Count == 0) NodeDic[start].SetOrder(0);
    }

    private void AStar(Node<int> start, Node<int> end)
    {
        var res = AStar<int>.AStarSearch(graph, start, end, (a, b) =>
        {
            var aPos = NodeDic[a].transform.position;
            var bPos = NodeDic[b].transform.position;
            return Vector3.Distance(aPos, bPos) / 10;
        });
        SetPath(res);
        if (res.Count == 0) NodeDic[start].SetOrder(0);
    }

    private void SPFA(Node<int> start, Node<int> end)
    {
        var res = SPFA<int>.SPFASearch(graph, start, end);
        SetPath(res);
        if (res.Count == 0) NodeDic[start].SetOrder(0);
    }

    private void FloydSearch(Node<int> start, Node<int> end)
    {
        var res = Floyd<int>.FloydSearch(graph, start, end);
        SetPath(res);
        if (res.Count == 0) NodeDic[start].SetOrder(0);
    }

    private void SetPath(List<Node<int>> path)
    {
        foreach (var node in NodeDic.Values)
        {
            node.SetOrder(-1);
        }

        for (int i = 0; i < path.Count; i++)
        {
            NodeDic[path[i]].SetOrder(i);
        }
    }

    private void SetEdgeColor(EdgeItem edge)
    {
        if (graph.IsDigraph)
        {
            edge.SetStartColor(startColor);
            edge.SetEndColor(endColor);
        }
        else
        {
            edge.SetStartColor(startColor);
            edge.SetEndColor(startColor);
        }
    }

    private void RefreshDropdown()
    {
        if (searchDropdown.value <= 1)
        {
            endNodeDropdown.gameObject.SetActive(false);
            var data = new List<Dropdown.OptionData>();
            foreach (var n in NodeDic.Keys)
            {
                data.Add(new Dropdown.OptionData(n.Data.ToString()));
            }

            startNodeDropdown.options = data;
        }
        else
        {
            endNodeDropdown.gameObject.SetActive(true);
            var data = new List<Dropdown.OptionData>();
            foreach (var n in NodeDic.Keys)
            {
                data.Add(new Dropdown.OptionData(n.Data.ToString()));
            }

            startNodeDropdown.options = data;
            endNodeDropdown.options = data;
        }
    }

    private void OnIsDigraphToggleValueChanged(bool value)
    {
        Clear();
        graph.IsDigraph = value;
    }

    private void OnSearchDropdownValueChanged(int value)
    {
        RefreshDropdown();
    }

    private void OnShowCostToggleValueChanged(bool value)
    {
        foreach (var edge in EdgeList)
        {
            edge.CostInput.gameObject.SetActive(value);
            edge.UpdateCostFieldPosition();
        }
    }

    private void OnAutoCostToggleValueChanged(bool value)
    {
        foreach (var edge in EdgeList)
        {
            edge.CostInput.readOnly = value;
            edge.UpdateCostFieldPosition();
        }
    }

    private void Clear()
    {
        foreach (var node in NodeDic.Values)
        {
            Destroy(node.gameObject);
        }

        foreach (var edge in EdgeList)
        {
            Destroy(edge.CostInput.gameObject);
            Destroy(edge.gameObject);
        }

        NodeDic.Clear();
        EdgeList.Clear();
        graph = new Graph<int>();
        count = 0;
    }
}