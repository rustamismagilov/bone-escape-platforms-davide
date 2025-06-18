using Unity.VisualScripting;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] int health;
    //[SerializeField] Collider2D myCollider;



    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Ciaoneeeee, this is: " + this.gameObject.name+ ", and collide with: " + other.gameObject.name);


        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            Hit(damageDealer.GetDamage());
        }

        /*
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player coll");
            DamageDealer damageDealer = other.GetComponentInChildren<DamageDealer>();
            if (damageDealer != null)
            {
                Hit(damageDealer.GetDamage());
            }
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy coll");
            DamageDealer damageDealer = other.GetComponentInChildren<DamageDealer>();
            if (damageDealer != null)
            {
                Hit(damageDealer.GetDamage());
            }
        }
        */

    }
    public void Hit(int damage) // Destroys the game object that this script is attached to
    {
        health -= damage;
        if (health > 0)
        {
            gameObject.GetComponentInParent<Animator>().SetTrigger("hit");
        } else
        {
            gameObject.GetComponentInParent<Animator>().SetTrigger("die");
            Destroy(this.gameObject, 2f);
        }
    }
}
