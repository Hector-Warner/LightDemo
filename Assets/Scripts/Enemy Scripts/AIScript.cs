using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    [Header("Do Not Modify")]
    public AIGridManager AIGridMgr;
    public GameObject player;
    public Rigidbody2D rb;
    public EnemyAttack enemyAttack;
    public Material PureWhiteMat;
    public Material DefaultMat;
    public List<Node> FinalPath = new List<Node>();
    private Animator myAnimator;
    

    [Header("To Customise Enemy")]
    public int health;
    public float moveSpeed;
    private float timer = 0;
    List<Node> EvaluatingList = new List<Node>();
    HashSet<Node> ClosedList = new HashSet<Node>();
    
    
    
    bool TracePath = false;
    Node target = null;


    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RequestPath();
        if (gameObject.GetComponent<EnemyAttack>() != null)
        {
            enemyAttack = gameObject.GetComponent<EnemyAttack>();
        }
        AIGridMgr = FindFirstObjectByType<AIGridManager>();
        PlayerController playerScript = FindFirstObjectByType<PlayerController>();
        if (playerScript != null)
        {
            player = playerScript.gameObject;
        }
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<EnemyHealth>().Health <= 0)
        {
            Destroy(gameObject);
        }
        if (timer >= 0.5f)
        {
            RequestPath();
            myAnimator.SetBool("IsWalking", true);
            timer = 0f;
        }
        timer += Time.fixedDeltaTime;

        if (TracePath && FinalPath != null && FinalPath.Count > 0)
        {
            Vector2 targetWorldPos = AIGridMgr.ConvertCoordToWorld(FinalPath[0].coords);
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetWorldPos, Time.fixedDeltaTime * moveSpeed));
            if (Vector2.Distance(rb.position, targetWorldPos) < 0.5f)
            {
                FinalPath.RemoveAt(0);
                if (FinalPath.Count == 0)
                {
                    TracePath = false;
                    //myAnimator.SetBool("IsWalking", false);
                }
            }
        }
    }

    public void TakeDamage()
    {
        StartCoroutine(FlashWhiteCororoutine());
    }

    private IEnumerator FlashWhiteCororoutine()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.material = PureWhiteMat;

        yield return new WaitForSeconds(0.3f);

        sr.material = DefaultMat;
    }

    void RequestPath()
    {
        if (AIGridMgr == null || player == null) return;
        AIGridMgr.ResetGridNodes(transform);
        Node startNode = AIGridMgr.GetNodeFromWorldPos(transform.position);
        Node targetNode = AIGridMgr.GetNodeFromWorldPos(player.transform.position);
        if (!targetNode.isWalkable)
        {
            targetNode = GetNearestWalkableNode(targetNode);
            if (targetNode == null) return;
        }
        FindPath(startNode, targetNode);
    }

    void FindPath(Node startNode, Node targetNode)
    {
        EvaluatingList.Clear();
        ClosedList.Clear();
        target = targetNode;
        EvaluatingList.Add(startNode);
        while (EvaluatingList.Count > 0)
        {
            float minF = float.MaxValue;
            int currentNodeIndex = 0;
            Node currentNode = null;
            for (int i = 0; i < EvaluatingList.Count; i++)
            {
                if (EvaluatingList[i].f < minF)
                {
                    minF = EvaluatingList[i].f;
                    currentNodeIndex = i;
                }
            }
            currentNode = EvaluatingList[currentNodeIndex];
            ClosedList.Add(EvaluatingList[currentNodeIndex]);
            if (currentNode == targetNode)
            {
                RetracePath();
                return;
            }
            EvaluatingList.RemoveAt(currentNodeIndex);
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0) continue;
                    int neighbourX = currentNode.coords.x + x;
                    int neighbourY = currentNode.coords.y + y;

                    if (neighbourX < 0 || neighbourX >= 100 || neighbourY < 0 || neighbourY >= 100)
                        continue;

                    Node neighbour = AIGridMgr.grid[neighbourX, neighbourY];

                    if (!neighbour.isWalkable || ClosedList.Contains(neighbour))
                        continue;

                    float newCostToNeighbour = currentNode.g + calculateDistance(currentNode.coords, neighbour.coords);

                    bool inSearchList = EvaluatingList.Contains(neighbour);

                    if (newCostToNeighbour < neighbour.g || !inSearchList)
                    {
                        neighbour.g = newCostToNeighbour;
                        neighbour.h = calculateDistance(neighbour.coords, targetNode.coords);
                        neighbour.previousNode = currentNode;

                        if (!inSearchList)
                        {
                            EvaluatingList.Add(neighbour);
                        }
                    }
                }
            }
        }
    }

    void RetracePath()
    {
        List<Node> path = new List<Node>();
        Node curr = target;

        while (curr != null)
        {
            path.Add(curr);
            curr = curr.previousNode;
        }

        path.Reverse();
        if (path.Count > 1)
        {
            path.RemoveAt(0);
        }
        FinalPath = path;
        TracePath = true;
    }

    Node GetNearestWalkableNode(Node centerNode)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                int checkX = centerNode.coords.x + x;
                int checkY = centerNode.coords.y + y;

                Node neighbour = AIGridMgr.grid[checkX, checkY];
                if (neighbour.isWalkable) return neighbour;
            }
        }
        return null;
    }


    public float calculateDistance(Vector2Int startCoord, Vector2Int endCoord)
    {
        float distance = Mathf.Abs(startCoord.x - endCoord.x) + Mathf.Abs(startCoord.y - endCoord.y);
        return distance;
    }
}
