using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class HealthScript : MonoBehaviour
{
    public Light2D playerLight;
    public float health = 100f;
    bool isReducing = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerLight = GetComponentInChildren<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public IEnumerator reduceHealth(float healthChange)
    {
        isReducing = true;
        float newHealth = health + healthChange;
        while (health > newHealth)
        {
            health -= Time.deltaTime * 40f;
            playerLight.pointLightOuterRadius = (health/100) * 10;
            if (health < newHealth)
            {
                health = newHealth;
            }
            yield return null;
        }
        isReducing = false;
    }
}
