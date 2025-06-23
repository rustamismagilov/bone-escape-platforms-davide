using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] bool autoMove = true;
    [SerializeField] float moveFrequency = 2f;
    [SerializeField] float speed = 10f;

    [Header("Jump")]
    [SerializeField] bool autoJump = true;
    [SerializeField] float jumpFrequency = 2f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] Collider2D touchingGroundCollider;

    [Header("Attack")]
    [SerializeField] bool autoAttack = true;
    [SerializeField] float attackFrequency = 2f;

    [Header("Die")]
    [SerializeField] Vector2 deathKick = new Vector2(0, 10f);
    [SerializeField] float dieDelay = 4f;

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;

    int healthAmount;
    bool hasHorizontalSpeed = false;
    bool isTouchingGround = false;
    bool isAlive = true;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartAI();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        CheckMove();
        CheckFlipSprite();
        CheckTouchGround();
        CheckHealth();
        CheckDead();
    }

    // start auto move, jump...
    void StartAI()
    {
        InvokeRepeating(nameof(OnMove), moveFrequency, moveFrequency);
        InvokeRepeating(nameof(OnJump), jumpFrequency, jumpFrequency);
        InvokeRepeating(nameof(OnAttack), attackFrequency, attackFrequency);
    }

    // on move
    void OnMove()
    {
        if (!isAlive) return;

        if (!autoMove)
        {
            moveInput = new Vector2(0, rb2d.linearVelocity.y);
        } else
        {
            int randomX = Random.Range(-1, 2);
            moveInput = new Vector2(randomX, rb2d.linearVelocity.y);
        }
    }

    // on jump
    void OnJump()
    {
        if (!isAlive || !autoJump || !isTouchingGround) return;

        if (isTouchingGround)
        {
            bool randomValue = Random.Range(0, 2) == 1 ? true : false;
            if (randomValue)
            {
                rb2d.linearVelocity += new Vector2(0f, jumpSpeed);
            }
        }
    }

    // on attack
    void OnAttack()
    {
        if (!isAlive) return;

        if (!autoAttack)
        {
            animator.SetBool("isAttacking", false);
        }
        else
        {
            bool randomValue = Random.Range(0, 2) == 1 ? true : false;
            animator.SetBool("isAttacking", randomValue);
        }
    }

    // check move
    void CheckMove()
    {
        Vector2 velocity = new Vector2(moveInput.x * speed, rb2d.linearVelocity.y);   
        rb2d.linearVelocity = velocity;

        hasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon; 
        animator.SetBool("isWalking", hasHorizontalSpeed);
    }

    // flip the player when it changes direction
    void CheckFlipSprite()
    {
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y); 
        }
    }

    // check if is touching the ground
    void CheckTouchGround()
    {
        isTouchingGround = touchingGroundCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }

    // check if is alive
    void CheckDead()
    {
        if (healthAmount <= 0)
        {
            // set is dead
            isAlive = false;
            animator.SetTrigger("die");
            rb2d.linearVelocity = deathKick;
            Destroy(this.gameObject, dieDelay);
        }
    }

    // take the sum of health of all DamageReceiver inside di game object
    void CheckHealth()
    {
        int health = 0;
        DamageReceiver[] receivers = GetComponentsInChildren<DamageReceiver>();
        foreach (DamageReceiver receiver in receivers)
        {
            health += receiver.GetHelth();
        }
        healthAmount = health;
    }
}
