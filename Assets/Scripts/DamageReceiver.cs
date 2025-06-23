using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] int health;

    [Header("Hit")]
    [SerializeField] float hitDuration = 0.5f;
    [SerializeField] Vector2 hitKick = new Vector2(0, 10f);

    Animator animator;
    Rigidbody2D rb2d;

    bool isHit = false;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        rb2d = GetComponentInParent<Rigidbody2D>();
    }

    // On trigger from DamageDealer
    void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("Damage receiver, this is: " + this.gameObject.name + ", and collide with: " + collider.gameObject.name);

        DamageDealer damageDealer = collider.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            Hit(damageDealer.GetDamage());
        }
    }

    // get health
    public int GetHelth()
    {
        return health;
    }

    // hit
    public void Hit(int damage) 
    {
        if (health <= 0 || isHit) return;

        health -= damage;

        if (health > 0)
        {
            isHit = true;
            Invoke(nameof(EndHit), hitDuration);

            rb2d.linearVelocity = hitKick;
            animator.SetTrigger("hit");
        }
        // else Destroy(this.gameObject);
    }
    // end hit
    void EndHit()
    {
        isHit = false;
    }
}
