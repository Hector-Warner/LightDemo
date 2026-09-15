using UnityEngine;

public class PlayerSpawnpoint : MonoBehaviour
{
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        PlayerController playerScript = FindFirstObjectByType<PlayerController>();
        if (playerScript != null)
        {
            player = playerScript.gameObject;
        }
        Debug.Log(player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPlayer()
    {
        
        player.transform.position = transform.position;
    }
}
