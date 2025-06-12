using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] int damageAmount = 40;

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D capsuleCollider2d;
    BoxCollider2D boxCollider2d;
    EnemyMovement enemyMovement;

    bool hasHorizontalSpeed = false;
    bool isJumping = false;
    bool isTouchingGround = false;
    bool isAlive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2d = GetComponent<CapsuleCollider2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();

        enemyMovement = FindFirstObjectByType<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return; 
        Walk();
        FlipSprite();
        Jump();
        Death();
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
            rb2d.linearVelocity += new Vector2(0f, jumpSpeed);
            isJumping = true;
            //Invoke(nameof(OnJumpDelay), 0.2f);
        }
    }
    void OnJumpDelay()
    {
        isJumping = true;
    }

    // on collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(boxCollider2d.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            enemyMovement.Hit(damageAmount);
        }

        /*
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.otherCollider == capsuleCollider2d)
            {
                // capsule collider was hit
            }
            if (contact.otherCollider == boxCollider2d)
            {
                // box collider was hit
            }
        }
        */
    }

    // walk
    void Walk()
    {
        // we have to add interpolation to the move, when it stop don t just stop, but it smoothly stops
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, rb2d.linearVelocity.y);   // move only in X axis and leave the Y as is
        rb2d.linearVelocity = playerVelocity;

        hasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon; // movement > 0 // never use 0 because, depending of the precision of unity and joystick and whatever, we have to set a dead zone, for this use Epsilon (is very near to 0)
        animator.SetBool("isWalking", hasHorizontalSpeed);
    }

    // flip the player when it changes direction
    void FlipSprite()
    {
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);  // I change the sign of localScale.X and keep the same localScale.Y
        }
    }

    // jump
    void Jump()
    {
        // set isJumping at the start of the jump and end when it touches the ground again
        isTouchingGround = boxCollider2d.IsTouchingLayers(LayerMask.GetMask("Ground", "Bouncy", "Enemy"));
        if (isJumping && isTouchingGround)
        {
            isJumping = false;
        }
        animator.SetBool("isJumping", isJumping);
    }

    // check if is alive
    void Death()
    {
        if (capsuleCollider2d.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")) || boxCollider2d.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            // set is dead
            isAlive = false;
            animator.SetTrigger("die");
            rb2d.linearVelocity = deathKick;
        }
    }
}
