using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpSpeed = 5f;

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D capsuleCollider2d;
    BoxCollider2D boxCollider2d;

    bool playerHasHorizontalSpeed = false;
    bool playerIsJumping = false;
    bool playerIsTouchingGround = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2d = GetComponent<CapsuleCollider2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
        FlipSprite();
        Jump();
    }

    // on move
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // on jump
    void OnJump(InputValue value)
    {
        if (value.isPressed && playerIsTouchingGround)
        {
            rb2d.linearVelocity += new Vector2(0f, jumpSpeed);
            playerIsJumping = true;
            //Invoke(nameof(OnJumpDelay), 0.2f);
        }
    }
    void OnJumpDelay()
    {
        playerIsJumping = true;
    }

    // walk
    void Walk()
    {
        // we have to add interpolation to the move, when it stop don t just stop, but it smoothly stops
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, rb2d.linearVelocity.y);   // move only in X axis and leave the Y as is
        rb2d.linearVelocity = playerVelocity;

        playerHasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon; // movement > 0 // never use 0 because, depending of the precision of unity and joystick and whatever, we have to set a dead zone, for this use Epsilon (is very near to 0)
        animator.SetBool("isWalking", playerHasHorizontalSpeed);
    }

    // flip the player when it changes direction
    void FlipSprite()
    {
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.linearVelocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);  // I change the sign of localScale.X and keep the same localScale.Y
        }
    }

    // jump
    void Jump()
    {
        // set isJumping at the start of the jump and end when it touches the ground again
        playerIsTouchingGround = boxCollider2d.IsTouchingLayers(LayerMask.GetMask("Ground", "Bouncy"));
        if (playerIsJumping && playerIsTouchingGround)
        {
            playerIsJumping = false;
        }
        animator.SetBool("isJumping", playerIsJumping);
    }
}
