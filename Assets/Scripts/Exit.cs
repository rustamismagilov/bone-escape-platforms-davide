using UnityEngine;

public class Exit : MonoBehaviour
{
    // on enter (works only with player because I disabled the other interactions
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") 
            && FindFirstObjectByType<EnemyController>() == null)
        {
            FindFirstObjectByType<GameSessionController>().ProcessPlayerWin();
        }
    }
}
