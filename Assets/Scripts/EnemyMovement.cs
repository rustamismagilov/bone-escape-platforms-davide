using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpSpeed = 5f;

    Vector2 moveInput;
    Rigidbody2D rb2d;
    Animator animator;
    //CapsuleCollider2D capsuleCollider2d;
    BoxCollider2D boxCollider2d;

    int life = 100;
    bool hasHorizontalSpeed = false;
    bool isAlive = true;
    bool isHit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //capsuleCollider2d = GetComponent<CapsuleCollider2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();

        InvokeRepeating(nameof(OnMove), 2f, 2f);
        InvokeRepeating(nameof(OnJump), 2f, 2f);
        InvokeRepeating(nameof(OnAttack), 2f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        Walk();
        FlipSprite();
        Death();
    }

    // on move
    void OnMove()
    {
        if (!isAlive) return;

        int randomX = Random.Range(-1, 2);
        moveInput = new Vector2(randomX, rb2d.linearVelocity.y);
    }

    // on jump
    void OnJump()
    {
        if (!isAlive) return;

        bool randomValue = Random.Range(0, 2) == 1 ? true : false;
        bool playerIsTouchingGround = boxCollider2d.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (randomValue && playerIsTouchingGround)
        {
            rb2d.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    // on attack
    void OnAttack()
    {
        bool randomValue = Random.Range(0, 2) == 1 ? true : false;
        animator.SetBool("isAttacking", randomValue);
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

    // check if is alive
    void Death()
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
        animator.SetBool("hit", isHit);
        Invoke(nameof(EndHit), 2f);

        // remove life
        life -= damage;
    }
    // end hit the enemy
    void EndHit()
    {
        isHit = true;
        animator.SetBool("hit", isHit);
    }
}
