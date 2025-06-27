using UnityEngine;
using UnityEngine.Audio;

public class DamageDealer : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] int damage;
    [SerializeField] AudioClip damageSound;

    AudioSource audioSource;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Damage damageDealer, this is: " + this.gameObject.name + ", and collide with: " + collider.gameObject.name);

        DamageReceiver damageReceiver = other.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            if (audioSource != null && damageSound != null) audioSource.PlayOneShot(damageSound);
            damageReceiver.Hit(damage);
        }

    }

    // get damage
    public int GetDamage() // Returns the damage value
    {
        return damage;
    }
}
