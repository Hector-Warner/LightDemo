using UnityEngine;

public class GenerateRoomType : MonoBehaviour
{
    public GameObject RoomPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GenerateRoom(int x, int y)
    {
        return Instantiate(RoomPrefab, new Vector3(x*20,y*20,0), transform.rotation);
    }
}
