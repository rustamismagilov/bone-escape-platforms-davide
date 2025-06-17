using UnityEngine;

public class Hazards : MonoBehaviour
{
    [SerializeField] int damageAmount = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // on collision
    /*void OnCollisionEnter2D(Collision2D collision)
    {
        // with capsuleCollider the player die
        if (!FindFirstObjectByType<GameSessionController>().isWinning
            && (capsuleCollider2d.IsTouchingLayers(LayerMask.GetMask("Hazards"))
            || boxCollider2d.IsTouchingLayers(LayerMask.GetMask("Hazards"))))
        {
            Die();
        }
    }*/
}
