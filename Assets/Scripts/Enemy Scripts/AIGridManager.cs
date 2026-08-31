using UnityEngine;

public class AIGridManager : MonoBehaviour
{
    public Node[,] grid = new Node[100,100];
    public GameObject player;

    [Header("Obstacle Settings")]
    public LayerMask obstacleLayer;
    public float nodeRadius = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void Awake()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                Node newNode = new Node(new Vector2Int(i, j), true);
                grid[i, j] = newNode;
            }
        }
    }

    public void ResetGridNodes(Transform requestingEnemy = null)
    {
        for (int i = 0; i < 100; i++) // Update boundaries to match grid size
        {
            for (int j = 0; j < 100; j++)
            {
                grid[i, j].g = 0;
                grid[i, j].h = 0;
                grid[i, j].previousNode = null;

                Vector2 worldPos = ConvertCoordToWorld(grid[i, j].coords);

                Collider2D hitCollider = Physics2D.OverlapCircle(worldPos, nodeRadius, obstacleLayer);

                if (hitCollider != null && requestingEnemy != null && hitCollider.transform == requestingEnemy)
                {
                    hitCollider = null;
                }
                grid[i, j].isWalkable = (hitCollider == null);
            }
        }
    }

    public Node GetNodeFromWorldPos(Vector2 worldPos)
    {
        Vector2Int coord = ConvertWorldToCoord(worldPos);
        coord.x = Mathf.Clamp(coord.x, 0, grid.GetLength(0) - 1);
        coord.y = Mathf.Clamp(coord.y, 0, grid.GetLength(1) - 1);
        return grid[coord.x, coord.y];
    }

    public Vector2Int ConvertWorldToCoord(Vector2 worldCoord)
    {
        return new Vector2Int(Mathf.FloorToInt(worldCoord.x), Mathf.FloorToInt(worldCoord.y));
    }

    public Vector2 ConvertCoordToWorld(Vector2Int coord)
    {
        return new Vector2(coord.x, coord.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Node
{
    public Vector2Int coords;
    public bool isWalkable;
    public float g;
    public float h;
    public float f => g + h;
    public Node previousNode;
    

    public Node(Vector2Int coords_, bool isWalkable_)
    {
        coords = coords_;
        isWalkable = isWalkable_;
        g = 0;
        h = 0;
    }
}
