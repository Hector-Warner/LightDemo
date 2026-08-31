using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float Health;
    public Material PureWhiteMat;
    public Material DefaultMat;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        StartCoroutine(FlashWhiteCororoutine());
    }

    private IEnumerator FlashWhiteCororoutine()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.material = PureWhiteMat;

        yield return new WaitForSeconds(0.3f);

        sr.material = DefaultMat;
    }
}
