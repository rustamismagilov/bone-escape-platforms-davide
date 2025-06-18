using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage;

    public int GetDamage() // Returns the damage value
    {
        return damage;
    }

    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Ciaoneeeee 2");

    }
    */
}
