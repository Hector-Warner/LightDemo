using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Variables")]
    public int speed;
    float horizontal;
    float vertical;
    float movementAdjustment = 1;
    public Collider2D MeleeCollider;


    public Rigidbody2D myRigidBody;
    public bool godMode = false;
    public HealthScript healthScript;
    public PlayerMeleeAttack meleeAttack;
    Vector2 currentDir;
    bool dashing = false;
    float dashTimer = 0f;
    public GameObject bullet;
    public GridController gridController;
    public SpriteRenderer spriteRenderer;
    public bool facingRight;
    public Vector2 Direction;
    private Animator myAnimator;


    private Camera myCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthScript = GetComponent<HealthScript>();
        meleeAttack = GetComponentInChildren<PlayerMeleeAttack>();
        myCam = Camera.main;
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        speed = 10;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(healthScript.reduceHealth(-5));
            dashTimer = 0;
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            myAnimator.SetTrigger("PlayerMeleeAtk");
        }

    }

    private void MeleeAttack()
    {
        meleeAttack.MeleeAttack();
    }

    private void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0)
        {
            movementAdjustment = 0.7f;
        }
        else
        {
            movementAdjustment = 1;
        }
        CharMovement();
        getMousePos();
        myAnimator.SetBool("IsWalking", true);
        MeleeCollider.transform.position = new Vector3(transform.position.x + (Direction.x), transform.position.y + (Direction.y), 0);
    }

    Vector2 getMousePos()
    {
        Vector3 trackingRelativeMousePos = Input.mousePosition;
        trackingRelativeMousePos.z = -myCam.transform.position.z;
        Vector3 worldMousePos = myCam.ScreenToWorldPoint(trackingRelativeMousePos);
        trackingRelativeMousePos.z = 0f;
        Direction = ((Vector2)worldMousePos - (Vector2)transform.position).normalized;
        return ((Vector2)worldMousePos - (Vector2)transform.position).normalized;
    }

    void CharMovement()
    {
        if (!dashing)
        {
            myRigidBody.linearVelocity = new Vector2(horizontal * speed * movementAdjustment, vertical * speed * movementAdjustment);
            if (horizontal != 0 || vertical != 0) currentDir = new Vector2(horizontal, vertical);
            if (horizontal > 0 && facingRight)
            {
                facingRight = !facingRight;
                spriteRenderer.flipX = !facingRight;
            }
            else if (horizontal < 0 && !facingRight)
            {
                facingRight = !facingRight;
                spriteRenderer.flipX = !facingRight;
            }
        }
    }

    IEnumerator Dash()
    {
        dashing = true;
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        float dashSpeed = speed * 3f * movementAdjustment;
        float dashDuration = 0.25f;
        float timer = 0f;

        while (timer < dashDuration)
        {
            myRigidBody.linearVelocity = currentDir.normalized * dashSpeed;
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        myRigidBody.linearVelocity = Vector2.zero;
        dashing = false;
    }

    void Shoot()
    {
        
        GameObject newBullet = Instantiate(bullet,transform.position, Quaternion.identity);
        float bulletSpeed = 30f;
        newBullet.GetComponent<Rigidbody2D>().linearVelocity = Direction * bulletSpeed;
        StartCoroutine(healthScript.reduceHealth(-10));
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Torch"))
        {
            if (Input.GetKey(KeyCode.Q))
            {
                Light2D torchLight = collision.gameObject.GetComponentInChildren<Light2D>();
                if (torchLight.pointLightOuterRadius > 0 && healthScript.health < 100)
                {
                    torchLight.pointLightOuterRadius -= Time.deltaTime;
                    healthScript.playerLight.pointLightOuterRadius += Time.deltaTime;
                    gameObject.GetComponent<CircleCollider2D>().radius = healthScript.playerLight.pointLightOuterRadius;
                    healthScript.health += Time.deltaTime * 100 / 10;
                    collision.gameObject.GetComponent<CircleCollider2D>().radius = torchLight.pointLightOuterRadius;
                }
            }
        }
    }
}
