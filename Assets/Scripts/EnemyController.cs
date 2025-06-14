using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] int life = 100;

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
        InvokeRepeating(nameof(OnMove), 2f, 2f);
        InvokeRepeating(nameof(OnJump), 2f, 2f);
        InvokeRepeating(nameof(OnAttack), 2f, 2f);
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
        if (!isAlive || isHit) return;

        int randomX = Random.Range(-1, 2);
        moveInput = new Vector2(randomX, rb2d.linearVelocity.y);
    }

    // on jump
    void OnJump()
    {
        if (!isAlive || isHit) return;

        bool randomValue = Random.Range(0, 2) == 1 ? true : false;
        if (randomValue && isTouchingGround)
        {
            rb2d.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    // on attack
    void OnAttack()
    {
        if (!isAlive || isHit) return;

        bool randomValue = Random.Range(0, 2) == 1 ? true : false;
        animator.SetBool("isAttacking", randomValue);
    }

    // check move
    void CheckMove()
    {
        // we have to add interpolation to the move, when it stop don t just stop, but it smoothly stops
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, rb2d.linearVelocity.y);   // move only in X axis and leave the Y as is
        rb2d.linearVelocity = playerVelocity;

        hasHorizontalSpeed = Mathf.Abs(rb2d.linearVelocity.x) > Mathf.Epsilon; // movement > 0 // never use 0 because, depending of the precision of unity and joystick and whatever, we have to set a dead zone, for this use Epsilon (is very near to 0) 
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
        isTouchingGround = boxCollider2d.IsTouchingLayers(LayerMask.GetMask("Ground", "Bouncy"));
    }

    // check if is alive
    void CheckDead()
    {
        if (life <= 0)
        {
            // set is dead
            isAlive = false;
            animator.SetTrigger("die");
            boxCollider2d.excludeLayers = LayerMask.GetMask("Player");
            rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y);
            Destroy(this.gameObject, 5f);
        }
    }

    // hit the enemy
    public void Hit(int damage)
    {
        // hit 
        isHit = true;
        animator.SetTrigger("hit");
        Invoke(nameof(EndHit), 0.5f);
        //Debug.Log("Start hit");

        // remove life
        life -= damage;
    }
    // end hit the enemy
    void EndHit()
    {
        isHit = false;
        //Debug.Log("End hit");
    }
}
