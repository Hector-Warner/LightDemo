using UnityEngine;

public class GenerateRoomType : MonoBehaviour
{
    public GameObject[] RoomPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                GenerateRoom(x, y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GenerateRoom(int x, int y)
    {
        int roomNo = Random.Range(0, RoomPrefab.Length);
        GameObject newRoom = Instantiate(RoomPrefab[roomNo], new Vector3(x * 20, y * 20, 0), transform.rotation);
        newRoom.transform.SetParent(transform);
        return newRoom;
    }
}
