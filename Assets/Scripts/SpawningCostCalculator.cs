using UnityEngine;

public class SpawningCostCalculator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float LevelCost;
    public float currentExpendedCost;
    public GameObject RoomGenerator;
    void Start()
    {
        GenerateRoomType RoomGeneratorScript = FindFirstObjectByType<GenerateRoomType>();
        if (RoomGeneratorScript != null)
        {
            RoomGenerator = RoomGeneratorScript.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentExpendedCost >= LevelCost)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length == 0)
            {
                RoomGenerator.GetComponent<GenerateRoomType>().Level += 1;
                RoomGenerator.GetComponent<GenerateRoomType>().newLevel(RoomGenerator.GetComponent<GenerateRoomType>().Level);
                currentExpendedCost = 0;
            }
        }
    }
}
