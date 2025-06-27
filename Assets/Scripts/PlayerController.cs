using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 20f;
    [SerializeField] AudioClip moveSound;
    [SerializeField] AudioClip sprintSound;

    [Header("Jump")]
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float jumpEffectDuration = 1f;
    [SerializeField] Collider2D touchingGroundCollider;
    [SerializeField] AudioClip jumpSound;

    [Header("Die")]
    [SerializeField] Vector2 deathKick = new Vector2(0, 10f);
    [SerializeField] AudioClip dieSound;

    AudioSource audioSource;
    Vector2 moveInput;
    Animator animator;
    Rigidbody2D rb2d;
    DamageReceiver damageReceiver;

    int totalHealthAmount;
    int healthAmount;
    float currentSpeed;
    bool hasHorizontalSpeed = false;
    bool isTouchingGround = false;
    bool isAlive = true;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // destroy this if there is another player
        int numPlayer = FindObjectsByType<PlayerController>(FindObjectsSortMode.None).Length;
        if (numPlayer > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        damageReceiver = GetComponentInChildren<DamageReceiver>();

        currentSpeed = speed;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    // on move
    void OnMove(InputValue value)
    {
        if (!isAlive) return;

        moveInput = value.Get<Vector2>();
    }

    // on jump
    void OnJump(InputValue value)
    {
        if (!isAlive) return;

        if (value.isPressed && isTouchingGround)
        {
            rb2d.linearVelocity += new Vector2(rb2d.linearVelocity.x, jumpSpeed);
            animator.SetBool("isJumping", true);
            audioSource.PlayOneShot(jumpSound);
            Invoke(nameof(JumpEffectEnd), jumpEffectDuration);
        }
    }

    // on sprint
    void OnSprint(InputValue value)
    {
        if (!isAlive) return;

        if (value.isPressed)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = speed;
        }
    }

    // reset player as new
    public void ResetPlayer()
    {
        ResetPlayerUI();
        ResetLive();
    }
    // reset player as new
    public void ResetLive()
    {
        // set life 100%
        ResetHelth();
        // set alive
        isAlive = true;
    }
    // reset player ui
    public void ResetPlayerUI()
    {
        // remove animations triggers
        animator.Rebind();
        animator.Update(0f);
        // reset moves
        moveInput = new Vector2(0, 0);
        currentSpeed = speed;
        rb2d.linearVelocity = new Vector2(0, 0);
    }

    // get total health
    public int GetTotalHealthAmount()
    {
        return totalHealthAmount;
    }
    // get health amount
    public int GetHealthAmount()
    {
        return healthAmount;
    }
    // add health amount
    public void AddHelth(int amount)
    {
        damageReceiver.Heal(amount);
    }
    // reset health at 100%
    public void ResetHelth()
    {
        damageReceiver.ResetHealth();
    }

    // check move
    void CheckMove()
    {
        // we have to add interpolation to the move, when it stop don t just stop, but it smoothly stops
        Vector2 velocity = new Vector2(moveInput.x * currentSpeed, rb2d.linearVelocity.y);   // move only in X axis and leave the Y as is
        rb2d.linearVelocity = velocity;

        hasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon; // movement > 0
        animator.SetBool("isWalking", hasHorizontalSpeed);
    }

    // flip the sprite when it changes direction
    void CheckFlipSprite()
    {
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);  // I change the sign of localScale.X and keep the same localScale.Y
        }
    }

    // check if is touching the ground
    void CheckTouchGround()
    {
        isTouchingGround = touchingGroundCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Enemy"));
    }

    void JumpEffectEnd()
    {
        animator.SetBool("isJumping", false);
    }

    // check if is alive
    void CheckDead()
    {
        if (healthAmount <= 0)
        {
            // set is dead
            isAlive = false;
            animator.SetTrigger("die");
            audioSource.PlayOneShot(dieSound);
            rb2d.linearVelocity = deathKick;
            FindFirstObjectByType<GameSession>().ProcessPlayerDeath();
        }
    }

    // take the sum of health of all DamageReceiver inside di game object
    void CheckHealth()
    {
        int totalHealth = 0;
        int health = 0;
        DamageReceiver[] receivers = GetComponentsInChildren<DamageReceiver>();
        foreach (DamageReceiver receiver in receivers)
        {
            totalHealth += receiver.GetTotalHelth();
            health += receiver.GetHelth();
        }
        totalHealthAmount = totalHealth;
        healthAmount = health;
    }
}
