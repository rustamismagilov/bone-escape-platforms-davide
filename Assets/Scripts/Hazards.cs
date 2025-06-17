using UnityEngine;

public class Hazards : MonoBehaviour
{
    [SerializeField] int damageAmount = 10;

    // on collision
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindFirstObjectByType<PlayerController>().Hit(damageAmount);
        }
    }
}
