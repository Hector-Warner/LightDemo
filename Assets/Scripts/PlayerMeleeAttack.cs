using System.Collections;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    ArrayList collidingObjects = new ArrayList();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MeleeAttack()
    {
        foreach (GameObject enemy in collidingObjects)
        {
            if (enemy.GetComponent<AIScript>() != null)
            {
                Debug.Log("DamageEnemy");
                enemy.GetComponent<EnemyHealth>().TakeDamage(20);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (!collidingObjects.Contains(collision.gameObject))
            {
                collidingObjects.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collidingObjects.Contains(collision.gameObject))
            {
                collidingObjects.Remove(collision.gameObject);
            }
        }
    }
}
