using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchSpawnpoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Torch;
    void Start()
    {
        SpawnTorch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool SpawnTorch()
    {
        if (Random.Range(1,3) == 1)
        {
            GameObject newTorch = Instantiate(Torch, transform.position, transform.rotation);
            int randomLight = Random.Range(3, 7);
            newTorch.GetComponentInChildren<Light2D>().pointLightOuterRadius = randomLight;
            newTorch.GetComponent<CircleCollider2D>().radius = randomLight;

        }
        return true;
    }
}
