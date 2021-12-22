using UnityEngine;
using UnityEngine.UI;

public class EdgeItem : MonoBehaviour
{
    private new LineRenderer renderer;
    public InputField CostInput { get; private set; }
    public NodeItem StartNode { get; set; }
    public NodeItem EndNode { get; set; }

    private void Awake()
    {
        renderer = GetComponent<LineRenderer>();
    }

    public void SetStartColor(Color color)
    {
        renderer.startColor = color;
    }

    public void SetEndColor(Color color)
    {
        renderer.endColor = color;
    }

    public void SetStartPoint(Vector3 pos)
    {
        renderer.SetPosition(0, pos);
    }

    public void SetEndPoint(Vector3 pos)
    {
        renderer.SetPosition(1, pos);
    }

    public void SetCostField()
    {
        var input = Instantiate(Main.Instance.costInputPrefab, Main.Instance.costInputRoot.transform)
            .GetComponent<InputField>();
        CostInput = input;
        if (!Main.Instance.showCostToggle.isOn) return;
        input.gameObject.SetActive(true);
        var startPos = StartNode.transform.position;
        var endPos = EndNode.transform.position;
        input.transform.position = (startPos + endPos) / 2;
        var cost = Main.Instance.autoCostToggle.isOn ? (Vector3.Distance(startPos, endPos) / 10) : 1;
        input.text = cost.ToString("F0");
        Main.Instance.OnChangeEdgeCost(this, cost);
        input.readOnly = Main.Instance.autoCostToggle.isOn;
        input.onEndEdit.AddListener(OnCostInputValueChanged);
    }

    public void UpdateCostFieldPosition()
    {
        if (!Main.Instance.showCostToggle.isOn) return;
        var startPos = StartNode.transform.position;
        var endPos = EndNode.transform.position;
        CostInput.transform.position = (startPos + endPos) / 2;
        if (Main.Instance.autoCostToggle.isOn)
        {
            var cost = Vector3.Distance(startPos, endPos) / 10;
            CostInput.text = cost.ToString("F0");
            Main.Instance.OnChangeEdgeCost(this, cost);
        }
    }

    private void OnCostInputValueChanged(string value)
    {
        if (Main.Instance.autoCostToggle.isOn) return;
        bool r = int.TryParse(value, out var cost);
        if (!r)
        {
            Main.Instance.OnChangeEdgeCost(this, 1);
            CostInput.text = "1";
            return;
        }

        Main.Instance.OnChangeEdgeCost(this, cost);
    }
}