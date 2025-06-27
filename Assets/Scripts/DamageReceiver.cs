using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] int totalHealth = 100;

    [Header("Hit")]
    [SerializeField] float hitDuration = 0.5f;
    [SerializeField] Vector2 hitKick = new Vector2(0, 10f);
    [SerializeField] AudioClip hitSound;

    AudioSource audioSource;
    Animator animator;
    Rigidbody2D rb2d;

    int health;
    bool isHit = false;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        audioSource = GetComponentInParent<AudioSource>();
        animator = GetComponentInParent<Animator>();
        rb2d = GetComponentInParent<Rigidbody2D>();
        health = totalHealth;
    }

    // On trigger from DamageDealer
    /*void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log("damageReceiver, this is: " + this.gameObject.name + ", and collide with: " + collider.gameObject.name);

        DamageDealer damageDealer = collider.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            Hit(damageDealer.GetDamage());
        }
    }*/

    // get total health
    public int GetTotalHelth()
    {
        return totalHealth;
    }

    // get health
    public int GetHelth()
    {
        return health;
    }

    // reset health to the amount of total health
    public void ResetHealth()
    {
        health = totalHealth;
    }

    // heal
    public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, totalHealth);
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

            if (rb2d != null && hitKick != null) 
                rb2d.linearVelocity = hitKick;
            if (animator != null) 
                animator.SetTrigger("hit");
            if (audioSource != null && hitSound != null) 
                audioSource.PlayOneShot(hitSound);
        }
        // else Destroy(this.gameObject);
    }
    // end hit
    void EndHit()
    {
        isHit = false;
    }
}
