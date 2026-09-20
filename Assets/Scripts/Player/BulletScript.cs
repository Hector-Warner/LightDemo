using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 5)
        {
            timer += Time.deltaTime;
        } else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<AIScript>() != null)
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(40);
            Destroy(gameObject);
        }
    }
}
