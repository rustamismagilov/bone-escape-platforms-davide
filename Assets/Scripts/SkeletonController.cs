using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class SkeletonController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] bool autoMove = true;
    [SerializeField] float moveFrequency = 2f;
    [SerializeField] float speed = 10f;
    [SerializeField] AudioClip moveSound;

    [Header("Jump")]
    [SerializeField] bool autoJump = true;
    [SerializeField] float jumpFrequency = 2f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] AudioClip jumpSound;

    [Header("Attack")]
    [SerializeField] bool autoAttack = true;
    [SerializeField] float attackFrequency = 2f;
    [SerializeField] AudioClip attackSound;

    [Header("Die")]
    [SerializeField] Vector2 deathKick = new Vector2(0, 10f);
    [SerializeField] AudioClip dieSound;
    [SerializeField] float afterDeadDecelerationRate = 2f;

    EnemyController enemyController;
    AudioSource audioSource;
    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    TouchingGroundHandler touchingGroundHandler;

    bool hasHorizontalSpeed = false;
    bool isBlocked = false;
    bool isDeadProcessed = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        audioSource = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingGroundHandler = GetComponent<TouchingGroundHandler>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartAI();
    }
    // Update is called once per frame
    void Update()
    {
        if (enemyController.isAlive)
        {
            CheckMove();
            CheckFlipSprite();
        }
        else
        {
            CheckBlockAfterDead();
            ProcessDead();
        }
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
        if (!enemyController.isAlive) return;

        if (!autoMove)
        {
            moveInput = new Vector2(0, rb2d.linearVelocity.y);
        }
        else
        {
            int randomX = Random.Range(-1, 2);
            moveInput = new Vector2(randomX, rb2d.linearVelocity.y);
        }
    }

    // on jump
    void OnJump()
    {
        if (!enemyController.isAlive || !autoJump || !touchingGroundHandler.isTouchingGround) return;

        bool randomValue = Random.Range(0, 2) == 1 ? true : false;
        if (randomValue)
        {
            rb2d.linearVelocity += new Vector2(0f, jumpSpeed);
            if (jumpSound != null) audioSource.PlayOneShot(jumpSound);
        }
    }

    // on attack
    void OnAttack()
    {
        if (!enemyController.isAlive) return;

        if (!autoAttack)
        {
            animator.SetBool("isAttacking", false);
        }
        else
        {
            bool randomValue = Random.Range(0, 2) == 1 ? true : false;
            animator.SetBool("isAttacking", randomValue);
            if (randomValue && attackSound != null)
                audioSource.PlayOneShot(attackSound);
        }
    }

    // check move
    void CheckMove()
    {
        Vector2 velocity = new Vector2(moveInput.x * speed, rb2d.linearVelocity.y);
        rb2d.linearVelocity = velocity;

        hasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon;
        animator.SetBool("isWalking", hasHorizontalSpeed);
        if (hasHorizontalSpeed && moveSound != null && !audioSource.isPlaying) 
            audioSource.PlayOneShot(moveSound);
    }

    // flip the player when it changes direction
    void CheckFlipSprite()
    {
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }


    // block the object smoothly after dead
    void CheckBlockAfterDead()
    {
        if (!isBlocked)
        {
            // Gradually reduce ONLY the X velocity
            float newX = Mathf.Lerp(rb2d.linearVelocity.x, 0f, Time.deltaTime * afterDeadDecelerationRate);
            rb2d.linearVelocity = new Vector2(newX, rb2d.linearVelocity.y);

            // Check if velocity is near zero
            if (rb2d.linearVelocity.magnitude < 0.01f && touchingGroundHandler.isTouchingGround)
            {
                rb2d.linearVelocity = Vector2.zero;
                rb2d.angularVelocity = 0;
                rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                isBlocked = true;
            }
        }
    }

    // process dead only once
    void ProcessDead()
    {
        if (!enemyController.isAlive && !isDeadProcessed)
        {
            animator.SetTrigger("die");
            if (dieSound != null)
                audioSource.PlayOneShot(dieSound);
            rb2d.linearVelocity = deathKick;
            isDeadProcessed = true;
            //Destroy(this.gameObject, dieDelay);
        }
    }
}
