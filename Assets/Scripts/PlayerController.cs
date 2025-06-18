using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 20f;

    [Header("Jump")]
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float jumpEffectDelay = 1f;
    public Collider2D groundCheckCollider;

    [Header("Hit")]
    [SerializeField] Vector2 deathKick = new Vector2(0, 10f);

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D capsuleCollider2d;


    DamageReceiver damageReceiver;

    float currentSpeed;
    bool hasHorizontalSpeed = false;
    bool isJumpStarting = false;        // there is a short period of time between the begin of the jump and the moment when the player doesn t touch the gound anymore.. in this period isJumpStarting is true
    bool isJumping = false;
    bool isTouchingGround = false;
    bool isAlive = true;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2d = GetComponent<CapsuleCollider2D>();
        damageReceiver = GetComponent<DamageReceiver>();

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
        CheckJump();
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
            isJumpStarting = true;
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

    // check move
    void CheckMove()
    {
        // we have to add interpolation to the move, when it stop don t just stop, but it smoothly stops
        Vector2 playerVelocity = new Vector2(moveInput.x * currentSpeed, rb2d.linearVelocity.y);   // move only in X axis and leave the Y as is
        rb2d.linearVelocity = playerVelocity;

        hasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon; // movement > 0
        animator.SetBool("isWalking", hasHorizontalSpeed);
    }

    // flip the player when it changes direction
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
        isTouchingGround = groundCheckCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Enemy"));
    }

    // check if is jumping
    void CheckJump()
    {
        // set isJumpStarting = false when it doesn t touch the ground anymore
        if (isJumpStarting && !isJumping && !isTouchingGround)
        {
            isJumpStarting = false;
            isJumping = true;
            Invoke(nameof(JumpEffect), jumpEffectDelay);
        }

        // set isJumping at the start of the jump and end when it touches the ground again
        if (isJumping && isTouchingGround && !isJumpStarting)
        {
            isJumping = false;
            Invoke(nameof(JumpEffect), jumpEffectDelay);
        }
    }
    void JumpEffect()
    {
        animator.SetBool("isJumping", isJumping);
    }

    // check if is alive
    void CheckDead()
    {
        if (damageReceiver.health <= 0)
        {
            // set is dead
            isAlive = false;
            animator.SetTrigger("die");
            FindFirstObjectByType<GameSessionController>().ProcessPlayerDeath();
        }
    }

    // Hit
    public void Hit(int damage)
    {
        if (FindFirstObjectByType<GameSessionController>().isWinning)
            return;

        damageReceiver.health -= damage;
        rb2d.linearVelocity = deathKick;
    }
}
