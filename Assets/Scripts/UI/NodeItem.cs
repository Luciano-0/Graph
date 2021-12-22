using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeItem : MonoBehaviour
{
    public Node<int> data;

    private RectTransform canvasRT;
    private Camera mainCamera;
    private Text label;
    private Text orderLabel;
    private Image image;
    private EdgeItem dragingEdge;
    private readonly List<EdgeItem> startEdgeList = new List<EdgeItem>();
    private readonly List<EdgeItem> endEdgeList = new List<EdgeItem>();
    private bool mouseRightButtonDown;

    private void Awake()
    {
        canvasRT = Main.Instance.canvasRT;
        mainCamera = Main.Instance.mainCamera;
        image = gameObject.GetComponent<Image>();
        label = transform.Find("Text").GetComponent<Text>();
        orderLabel = transform.Find("Order").GetComponent<Text>();
    }

    public void SetNum(int num)
    {
        data = new Node<int>(num);
        label.text = num.ToString();
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void SetOrder(int order)
    {
        if (order == -1)
        {
            orderLabel.gameObject.SetActive(false);
            return;
        }
        orderLabel.gameObject.SetActive(true);
        orderLabel.text = order.ToString();
    }

    public void OnDrag()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButton(1))
        {
            if (dragingEdge == null) return;
            var mousePosition = Input.mousePosition;
            dragingEdge.SetEndPoint(mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 400)));
        }
        else
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, Input.mousePosition, null, out var p);
            transform.GetComponent<RectTransform>().anchoredPosition = p;
            foreach (var edge in startEdgeList)
            {
                edge.SetStartPoint(Main.Instance.CanvasPositionToWorldPosition(transform.position));
                edge.UpdateCostFieldPosition();
            }

            foreach (var edge in endEdgeList)
            {
                edge.SetEndPoint(Main.Instance.CanvasPositionToWorldPosition(transform.position));
                edge.UpdateCostFieldPosition();
            }
        }
    }

    public void OnBeginDrag()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButton(1))
        {
            mouseRightButtonDown = true;
            dragingEdge = Main.Instance.OnAddEdgeStart(this);
        }
    }

    public void OnEndDrag()
    {
        if (Input.GetKey(KeyCode.LeftControl) || mouseRightButtonDown)
        {
            mouseRightButtonDown = false;
            if (!Main.Instance.OnAddEdgeEnd(this, dragingEdge))
            {
                Destroy(dragingEdge.gameObject);
                dragingEdge = null;
            }
        }
    }

    public void AddStartEdgeList(EdgeItem edge)
    {
        startEdgeList.Add(edge);
    }

    public void AddEndEdgeList(EdgeItem edge)
    {
        endEdgeList.Add(edge);
    }
}