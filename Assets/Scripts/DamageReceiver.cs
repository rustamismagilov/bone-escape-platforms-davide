using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public int health = 100;
    [SerializeField] float hitDelay = 0.5f;
    [SerializeField] float dieDelay = 4f;

    bool isAlive = true;
    bool isHit = false;

    Rigidbody2D rb2d;
    Animator animator;
    Collider2D[] colliders;
    Collider2D triggerWhichWillBeUsedToDamage;
    DamageDealer damageDealer;

    void Awake()
    {
        damageDealer = GetComponent<DamageDealer>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        colliders = GetComponents<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == FindGroundColliderOnPlayer(other.gameObject))
            return;


        DamageDealer dealer = other.GetComponent<DamageDealer>();
        if (dealer == null) return;

        GameObject attacker = other.gameObject;

        // Don't hurt ourselves
        if (attacker == gameObject) return;

        // Don't damage game objects with same tag
        if (CompareTag(attacker.tag)) return;

        bool isPlayer = attacker.CompareTag("Player");
        bool isEnemy = attacker.CompareTag("Enemy");

        // Player jumps on enemy head
        if (isPlayer && gameObject.CompareTag("Enemy"))
        {
            float verticalOffset = attacker.transform.position.y - transform.position.y;

            if (verticalOffset > 0.5f) // means player is above enemy
            {
                ReceiveDamage(dealer.GetDamage());

                // Bounce the player
                Rigidbody2D playerRb = attacker.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 15f); // bounce force

                return;
            }
        }

        // Player attacks enemy with sword/etc
        if (dealer.IsActive() && gameObject.CompareTag("Enemy"))
        {
            ReceiveDamage(dealer.GetDamage());
            return;
        }

        // Enemy touches player = always damages player
        if (isEnemy && gameObject.CompareTag("Player"))
        {
            ReceiveDamage(dealer.GetDamage());
            return;
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (!isAlive) return;

        health -= damage;
        Debug.Log($"{gameObject.name} received {damage} damage. Remaining health: {health}");

        if (health > 0)
        {
            if (animator != null)
                animator.SetTrigger("hit");
        }
        else
        {
            Die();
        }
    }


    Collider2D FindGroundColliderOnPlayer(GameObject player)
    {
        if (!player.CompareTag("Player")) return null;
        var controller = player.GetComponent<PlayerController>();
        return controller != null ? controller.groundCheckCollider : null;
    }

    void Die()
    {
        isAlive = false;

        if (animator != null)
            animator.SetTrigger("die");

        if (rb2d != null)
            rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y);

        foreach (var col in colliders)
            col.enabled = false;

        Destroy(gameObject, dieDelay);
    }

}
