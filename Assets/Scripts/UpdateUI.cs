using TMPro;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

public class UpdateUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI myTextComponent;
    public GameObject player;
    void Start()
    {
        PlayerController playerScript = FindFirstObjectByType<PlayerController>();
        if (playerScript != null)
        {
            player = playerScript.gameObject;
        }

        myTextComponent.text = $"Coins: {player.GetComponent<HealthScript>().money}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCoinCount()
    {
        myTextComponent.text = $"Coins: {player.GetComponent<HealthScript>().money}";
    }
}
