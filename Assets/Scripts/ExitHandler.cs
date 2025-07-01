using UnityEngine;

public class ExitHandler : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    bool isEnemiesAlive;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckEnemiesAlive();
    }

    // on enter (works only with player because I disabled the other interactions
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isEnemiesAlive)
        {
            FindFirstObjectByType<GameSession>().ProcessPlayerWin();
        }
    }

    void CheckEnemiesAlive()
    {
        EnemyController[] enemies = GameObject.FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        foreach (EnemyController enemy in enemies)
        {
            if (enemy.isAlive)
            {
                isEnemiesAlive = true;
                spriteRenderer.color = Color.gray;
                return;
            }
        }
        isEnemiesAlive = false;
        spriteRenderer.color = Color.white;
    }
}
