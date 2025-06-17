using UnityEngine;

public class Exit : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool isEnemyAlive;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckEnemyAlive();
    }

    // on enter (works only with player because I disabled the other interactions
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")
            && !isEnemyAlive)
        {
            FindFirstObjectByType<GameSessionController>().ProcessPlayerWin();
        }
    }

    void CheckEnemyAlive()
    {
        if (FindFirstObjectByType<EnemyController>() == null)
        {
            isEnemyAlive = false;
            spriteRenderer.color = Color.white;
        } else
        {
            isEnemyAlive = true;
            spriteRenderer.color = Color.gray;
        }
    }
}
