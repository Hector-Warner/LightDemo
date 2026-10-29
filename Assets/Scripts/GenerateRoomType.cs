using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;

public class GenerateRoomType : MonoBehaviour
{
    public GameObject[] RoomPrefab;
    public int numOfRoomsX;
    public int numOfRoomsY;
    public int Level;
    public GameObject GameManager;
    GameObject[] RoomList;
    GameObject Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerController playerScript = FindFirstObjectByType<PlayerController>();
        if (playerScript != null)
        {
            Player = playerScript.gameObject;
        }
        RoomList = new GameObject[1];
        Level = 2;
        newLevel(Level);
    }

    public void newLevel(int levelNo)
    {
        if (levelNo != 1)
        {
            Player.transform.position = new Vector2(-25, 15);
        }
        DestroyPreviousLayout();
        RoomList = new GameObject[numOfRoomsX * numOfRoomsY];
        GameManager.GetComponent<SpawningCostCalculator>().LevelCost = levelNo * 5;
        for (int x = 0; x < numOfRoomsX; x++)
        {
            for (int y = 0; y < numOfRoomsY; y++)
            {
                RoomList[x + y*5] = GenerateRoom(x, y);
            }
        }
    }

    public GameObject GenerateRoom(int x, int y)
    {
        int roomNo = Random.Range(0, RoomPrefab.Length);
        GameObject newRoom = Instantiate(RoomPrefab[roomNo], new Vector3(x * 20 + 10, y * 20 + 10, 0), transform.rotation);
        newRoom.transform.SetParent(transform);

        if (x == Mathf.FloorToInt(numOfRoomsX/2) && y ==  Mathf.FloorToInt(numOfRoomsY/2))
        {
            Debug.Log("Attempting to Spawn");
            if (Level == 1)
            {
                newRoom.GetComponentInChildren<PlayerSpawnpoint>().SpawnPlayer();
            }
        }
        return newRoom;
    }

    public void DestroyPreviousLayout()
    {
        Debug.Log(RoomList.Length);
        foreach (GameObject room in RoomList)
        {
            Debug.Log(room);
            if (room != null)
            {
                Destroy(room);
            }
        }
    }
}
