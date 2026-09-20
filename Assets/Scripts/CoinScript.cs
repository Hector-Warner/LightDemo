using UnityEngine;

public class CoinScript : MonoBehaviour
{
    // Value attributed to this coin
    public int coinAmount;
    GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerController playerScript = FindFirstObjectByType<PlayerController>();
        if (playerScript != null)
        {
            player = playerScript.gameObject;
        }
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        Vector2 InitialForce = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        rb.linearVelocity = InitialForce;
        //rb.AddForce(InitialForce);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            player.GetComponent<HealthScript>().money += coinAmount;
            Destroy(gameObject);
        }
    }
}
