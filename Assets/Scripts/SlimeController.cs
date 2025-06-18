using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] bool autoMove = true;
    [SerializeField] float moveFrequency = 2f;
    [SerializeField] float speed = 10f;

    [Header("Jump")]
    [SerializeField] bool autoJump = true;
    [SerializeField] float jumpFrequency = 2f;
    [SerializeField] float jumpSpeed = 5f;

    [Header("Status")]
    [SerializeField] int healthAmount = 100;
    [SerializeField] float hitDelay = 0.5f;
    [SerializeField] float dieDelay = 4f;

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    BoxCollider2D boxCollider2d;

    bool hasHorizontalSpeed = false;
    bool isTouchingGround = false;
    bool isAlive = true;
    bool isHit = false;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2d = GetComponent<BoxCollider2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(OnMove), moveFrequency, moveFrequency);
        InvokeRepeating(nameof(OnJump), jumpFrequency, jumpFrequency);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive || isHit) return;

        CheckMove();
        CheckFlipSprite();
        CheckTouchGround();
        CheckDead();
    }

    // on move
    void OnMove()
    {
        if (!isAlive) return;

        if (isHit || !autoMove)
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
        if (!isAlive || isHit || !autoJump || !isTouchingGround) return;

        if (isTouchingGround)
        {
            bool randomValue = Random.Range(0, 2) == 1 ? true : false;
            if (randomValue)
            {
                rb2d.linearVelocity += new Vector2(0f, jumpSpeed);
            }
        }
    }

    // check move
    void CheckMove()
    {
        // we have to add interpolation to the move, when it stop don t just stop, but it smoothly stops
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, rb2d.linearVelocity.y);   // move only in X axis and leave the Y as is
        rb2d.linearVelocity = playerVelocity;

        hasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon; // movement > 0 // never use 0 because, depending of the precision of unity and joystick and whatever, we have to set a dead zone, for this use Epsilon (is very near to 0) 
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
        isTouchingGround = boxCollider2d.IsTouchingLayers(LayerMask.GetMask("Ground", "Bouncy"));
    }

    // check if is alive
    void CheckDead()
    {
        if (healthAmount <= 0)
        {
            // set is dead
            isAlive = false;
            animator.SetTrigger("die");
            rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y);
            Destroy(this.gameObject, dieDelay);
        }
    }

    // hit the enemy
    public void Hit(int damage)
    {
        if (!isHit)
        {
            //Debug.Log("Start hit");
            isHit = true;
            healthAmount -= damage;
            animator.SetTrigger("hit");
            Invoke(nameof(EndHit), hitDelay);
        }
    }
    // end hit the enemy
    void EndHit()
    {
        isHit = false;
        //Debug.Log("End hit");
    }
}
