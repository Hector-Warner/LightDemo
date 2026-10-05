using Unity.VisualScripting;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    GameObject Player;
    bool spawned = false;
    GameObject GameManager;
    float Timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    enum MonsterType
    {
        FlowerCreature,
        Giant
    }

    public int SpawnAmount;

    [SerializeField]
    MonsterType monsterType = new MonsterType();

    public GameObject MonsterPrefab;

    void Start()
    {
        PlayerController playerScript = FindFirstObjectByType<PlayerController>();
        SpawningCostCalculator spawnCostCalcScript = FindFirstObjectByType<SpawningCostCalculator>();
        if (playerScript != null)
        {
            Player = playerScript.gameObject;
        }
        if (spawnCostCalcScript != null)
        {
            GameManager = spawnCostCalcScript.gameObject;
        }
        Debug.Log("Loaded");
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (!spawned)
        {
            if (calculateDistance(Player.transform.position,transform.position) < 30)
            {
                SpawnMonsters();
            }
        }
        if (Timer >= 15)
        {
            spawned = false;
            Timer = 0;
        }
    }

    public float calculateDistance(Vector2 startCoord, Vector2 endCoord)
    {
        float distance = Mathf.Abs(startCoord.x - endCoord.x) + Mathf.Abs(startCoord.y - endCoord.y);
        return distance;
    }

    void SpawnMonsters()
    {
        if (monsterType == MonsterType.FlowerCreature)
        {
            SpawnAmount = Random.Range(1, 3);
        } else if (monsterType == MonsterType.Giant)
        {
            SpawnAmount = Random.Range(1, 2);
        }
        if (Random.Range(1, 3) == 1)
        {
            if (GameManager.GetComponent<SpawningCostCalculator>().currentExpendedCost < GameManager.GetComponent<SpawningCostCalculator>().LevelCost)
            {
                for (int i = 0; i < SpawnAmount; i++)
                {
                    Instantiate(MonsterPrefab, transform.position, transform.rotation);
                }
                GameManager.GetComponent<SpawningCostCalculator>().currentExpendedCost += 1;
                spawned = true;
            }
        }
    }
}
