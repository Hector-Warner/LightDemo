using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2Int gridSize;
    public float cellRadius = 0.5f;
    public FlowField curFlowField; //Current FlowField
    public GridDebug gridDebug;

    private void InitialiseFlowField()
    {
        curFlowField = new FlowField(cellRadius, gridSize);
        curFlowField.CreateGrid();
        gridDebug.SetFlowField(curFlowField);
    }

    private void Update()
    {
    }

    public void scanNewPos(Vector3 TargetPos)
    {
        InitialiseFlowField();
        curFlowField.CreateCostField();
        gridDebug.DrawFlowField();

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Cell destinationCell = curFlowField.GetCellFromWorldPos(TargetPos);

        curFlowField.CreateIntegrationField(destinationCell);
        curFlowField.CreateFlowField();
    }
}
