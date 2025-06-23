using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] int damage;

    public int GetDamage() // Returns the damage value
    {
        return damage;
    }

    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Damage dealer");

        DamageReceiver damageReceiver = other.GetComponent<DamageReceiver>();
        if (damageReceiver != null)
        {
            damageReceiver.Hit(damage);
        }

    }
    */
}
