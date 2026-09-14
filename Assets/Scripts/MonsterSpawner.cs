using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    GameObject Player;
    bool spawned = false;
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
        if (playerScript != null)
        {
            Player = playerScript.gameObject;
        }
        Debug.Log("Loaded");
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned)
        {
            if (calculateDistance(Player.transform.position,transform.position) < 30)
            {
                SpawnMonsters();
            }
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
            SpawnAmount = Random.Range(2, 6);
        } else if (monsterType == MonsterType.Giant)
        {
            SpawnAmount = Random.Range(1, 4);
        }
        if (Random.Range(1, 3) == 1)
        {
            for (int i = 0; i < SpawnAmount; i++)
            {
                Instantiate(MonsterPrefab, transform.position, transform.rotation);
            }
        }
        spawned = true;
    }
}
