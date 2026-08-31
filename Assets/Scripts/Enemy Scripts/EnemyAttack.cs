using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackSpeed;
    public float damage;
    float timer = 0;
    public AIGridManager AIGridMgr;
    public AIScript enemyAI;
    public float attackCooldown;
    private float lastAttackTime = 0f;

    private Animator myAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        AIGridMgr = FindFirstObjectByType<AIGridManager>();
        enemyAI = GetComponent<AIScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (IsPlayerInMeleeRange() && Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Ready to attack!");
            StartAttack();
        }
    }

    private bool IsPlayerInMeleeRange()
    {
        var myCoords = AIGridMgr.GetNodeFromWorldPos(transform.position).coords;
        var playerCoords = AIGridMgr.GetNodeFromWorldPos(enemyAI.player.transform.position).coords;
        return enemyAI.calculateDistance(myCoords, playerCoords) <= 2;
    }

    private void StartAttack()
    {
        lastAttackTime = Time.time;
        myAnimator.SetTrigger("EnemyAttack");
    }

    public void ApplyDamage()
    {
        if (enemyAI != null && enemyAI.player != null && IsPlayerInMeleeRange())
        {
            StartCoroutine(enemyAI.player.GetComponent<HealthScript>().reduceHealth(-10));
        }
    }

    public void Attack()
    {
        if (timer + 0.5f >= attackSpeed) myAnimator.SetTrigger("EnemyAttack");
        if (timer >= attackSpeed)
        {
            StartCoroutine(enemyAI.player.GetComponent<HealthScript>().reduceHealth(-10));
            timer = 0;
        }
        timer += Time.fixedDeltaTime;
    }
}
