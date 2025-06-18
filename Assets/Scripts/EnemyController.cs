using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] bool autoMove = true;
    [SerializeField] float moveFrequency = 2f;
    [SerializeField] float speed = 10f;

    [Header("Jump")]
    [SerializeField] bool canJump = true;
    [SerializeField] float jumpFrequency = 2f;
    [SerializeField] float jumpSpeed = 5f;

    [Header("Attack")]
    [SerializeField] bool canAttack = true;
    [SerializeField] float attackFrequency = 2f;
    [SerializeField] int damageAmount = 100;

    [Header("Status")]
    [SerializeField] int healthAmount = 100;
    [SerializeField] float hitDelay = 0.5f;
    [SerializeField] float dieDelay = 4f;

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D capsuleCollider2d;
    BoxCollider2D boxCollider2d;
    DamageDealer damageDealer;

    bool hasHorizontalSpeed = false;
    bool isTouchingGround = false;
    bool isAlive = true;
    bool isHit = false;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2d = GetComponent<CapsuleCollider2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        damageDealer = GetComponent<DamageDealer>();
    }

    void Start()
    {
        InvokeRepeating(nameof(OnMove), moveFrequency, moveFrequency);

        if (canJump)
            InvokeRepeating(nameof(OnJump), jumpFrequency, jumpFrequency);

        if (canAttack)
            InvokeRepeating(nameof(OnAttack), attackFrequency, attackFrequency);
    }

    void Update()
    {
        if (!isAlive || isHit) return;

        CheckMove();
        CheckFlipSprite();
        CheckTouchGround();
        CheckDead();
    }

    void OnMove()
    {
        if (!isAlive) return;

        if (isHit || !autoMove)
        {
            moveInput = new Vector2(0, rb2d.linearVelocity.y);
        }
        else
        {
            int randomX = Random.Range(-1, 2);
            moveInput = new Vector2(randomX, rb2d.linearVelocity.y);
        }
    }

    void OnJump()
    {
        if (!isAlive || isHit || !isTouchingGround) return;

        bool shouldJump = Random.Range(0, 2) == 1;
        if (shouldJump)
        {
            rb2d.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnAttack()
    {
        if (!isAlive || isHit) return;

        bool attack = Random.Range(0, 2) == 1;
        animator.SetBool("isAttacking", attack);

        if (damageDealer != null)
        {
            damageDealer.canDealDamage = attack;
        }
    }

    void CheckMove()
    {
        Vector2 velocity = new Vector2(moveInput.x * speed, rb2d.linearVelocity.y);
        rb2d.linearVelocity = velocity;

        hasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon;
        animator.SetBool("isWalking", hasHorizontalSpeed); // create parameter, anim and transition for slime
    }

    void CheckFlipSprite()
    {
        if (hasHorizontalSpeed)
        {
            float direction = Mathf.Sign(rb2d.linearVelocity.x);
            transform.localScale = new Vector2(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    void CheckTouchGround()
    {
        isTouchingGround = capsuleCollider2d.IsTouchingLayers(LayerMask.GetMask("Ground", "Bouncy"));
    }

    void CheckDead()
    {
        if (healthAmount <= 0)
        {
            isAlive = false;
            animator.SetTrigger("die");
            capsuleCollider2d.excludeLayers = LayerMask.GetMask("Player");
            boxCollider2d.excludeLayers = LayerMask.GetMask("Player");
            rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y);
            Destroy(gameObject, dieDelay);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (boxCollider2d.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null && other is CapsuleCollider2D)
            {
                player.Hit(damageAmount);
            }
        }
    }
}
