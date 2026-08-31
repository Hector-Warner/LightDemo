using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public Cell[,] grid { get; private set; }
    public Vector2Int gridSize { get; private set;  }
    public float cellRadius { get; private set;  }

    private float cellDiameter;

    public Cell destinationCell;

    public FlowField(float _cellRadius, Vector2Int _gridSize)
    {
        cellRadius = _cellRadius;
        cellDiameter = cellRadius * 2f;
        gridSize = _gridSize;
    }

    public void CreateGrid()
    {
        grid = new Cell[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 worldPos = new Vector3(cellDiameter * x + cellRadius, cellDiameter * y + cellRadius, 0);
                grid[x, y] = new Cell(worldPos, new Vector2Int(x, y));
            }
        }
    }

    public void CreateCostField()
    {
        Vector3 cellHalfExtents = Vector3.one * cellRadius;
        int terrainMask = LayerMask.GetMask("Impassable", "RoughTerrain");
        foreach (Cell curCell in grid)
        {
            Collider[] obstacles = Physics.OverlapBox(curCell.worldPos, cellHalfExtents, Quaternion.identity, terrainMask);
            bool hasIncreasedCost = false;
            foreach (Collider col in obstacles)
            {
                if (col.gameObject.layer == 8)
                {
                    curCell.IncreaseCost(255);
                    continue;
                } else if (!hasIncreasedCost && col.gameObject.layer == 9)
                {
                    curCell.IncreaseCost(3);
                    hasIncreasedCost = true;
                }
            }
        }
    }

    private List<Cell> GetNeighbourCells(Vector2Int nodeIndex, List<GridDirection> directions)
    {
        List<Cell> neighbourCells = new List<Cell>();

        foreach (Vector2Int curDirection in directions)
        {
            Cell newNeighbour = GetCellAtRelativePos(nodeIndex, curDirection);
            if (newNeighbour != null)
            {
                neighbourCells.Add(newNeighbour);
            }
        }
        return neighbourCells;
    }

    private Cell GetCellAtRelativePos(Vector2Int originalPos, Vector2Int relativePos)
    {
        Vector2Int finalPos = originalPos + relativePos;

        if (finalPos.x < 0 || finalPos.x >= gridSize.x || finalPos.y < 0 || finalPos.y >= gridSize.y)
        {
            return null;
        } else
        {
            return grid[finalPos.x, finalPos.y];
        }
    }

    public void CreateIntegrationField(Cell _destinationCell)
    {
        destinationCell = _destinationCell;
        destinationCell.cost = 0;
        destinationCell.bestCost = 0;

        Queue<Cell> cellsToCheck = new Queue<Cell>();

        cellsToCheck.Enqueue(destinationCell);

        while (cellsToCheck.Count > 0)
        {
            Cell curCell = cellsToCheck.Dequeue();
            List<Cell> curNeighbours = GetNeighbourCells(curCell.gridIndex, GridDirection.CardinalDirections);

            foreach (Cell curNeighbour in curNeighbours)
            {
                if (curNeighbour.cost == byte.MaxValue)
                {
                    continue;
                }
                if (curNeighbour.cost + curCell.bestCost < curNeighbour.bestCost)
                {
                    curNeighbour.bestCost = (ushort)(curNeighbour.cost + curCell.bestCost);
                    cellsToCheck.Enqueue(curNeighbour);
                }
            }
        }
    }

    public void CreateFlowField()
    {
        foreach (Cell cell in grid)
        {
            List<Cell> curNeighbours = GetNeighbourCells(cell.gridIndex, GridDirection.AllDirections);
            int bestCost = cell.bestCost;

            foreach (Cell curNeighbour in curNeighbours)
            {
                if (curNeighbour.bestCost < bestCost)
                {
                    bestCost = curNeighbour.bestCost;
                    cell.bestDirection = GridDirection.GetDirectionFromV2I(curNeighbour.gridIndex - cell.gridIndex);
                }
            }
        }
    }

    public Cell GetCellFromWorldPos(Vector3 worldPos)
    {
        float percentX = worldPos.x / (gridSize.x * cellDiameter);
        float percentY = worldPos.y / (gridSize.y * cellDiameter);

        percentX = Mathf.Clamp01((float)percentX);
        percentY = Mathf.Clamp01((float)percentY);

        int x = Mathf.Clamp(Mathf.FloorToInt((gridSize.x) * percentX), 0, gridSize.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridSize.y) * percentY), 0, gridSize.y - 1);
        return grid[x, y];
    }
}
