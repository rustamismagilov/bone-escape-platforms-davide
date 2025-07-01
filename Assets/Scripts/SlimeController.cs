using UnityEngine;

[RequireComponent(typeof(EnemyController))]
public class SlimeController : MonoBehaviour
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
            if (jumpSound != null)
                audioSource.PlayOneShot(jumpSound);
        }
    }

    // check move
    void CheckMove()
    {
        // we have to add interpolation to the move, when it stop don t just stop, but it smoothly stops
        Vector2 velocity = new Vector2(moveInput.x * speed, rb2d.linearVelocity.y);   // move only in X axis and leave the Y as is
        rb2d.linearVelocity = velocity;

        hasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon; // movement > 0 // never use 0 because, depending of the precision of unity and joystick and whatever, we have to set a dead zone, for this use Epsilon (is very near to 0) 
        if (hasHorizontalSpeed && moveSound != null && !audioSource.isPlaying)
            audioSource.PlayOneShot(moveSound);
    }

    // flip the sprite when it changes direction
    void CheckFlipSprite()
    {
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);  // I change the sign of localScale.X and keep the same localScale.Y
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
