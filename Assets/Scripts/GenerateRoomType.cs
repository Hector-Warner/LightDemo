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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Level = 1;
        newLevel(Level);
    }

    void newLevel(int levelNo)
    {
        GameManager.GetComponent<SpawningCostCalculator>().LevelCost = levelNo * 5;
        for (int x = 0; x < numOfRoomsX; x++)
        {
            for (int y = 0; y < numOfRoomsY; y++)
            {
                GenerateRoom(x, y);
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
            newRoom.GetComponentInChildren<PlayerSpawnpoint>().SpawnPlayer();
        }
        return newRoom;
    }
}
