using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isAlive { get; private set; } = true;
    public int healthAmount { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            CheckHealth();
            CheckDead();
        }
    }

    // take the sum of health of all DamageReceiver inside di game object
    void CheckHealth()
    {
        int health = 0;
        DamageReceiver[] receivers = GetComponentsInChildren<DamageReceiver>();
        foreach (DamageReceiver receiver in receivers)
        {
            health += receiver.GetHelth();
        }
        healthAmount = health;
    }
    // check if is alive
    void CheckDead()
    {
        if (healthAmount > 0)
        {
            isAlive = true;
        }
        else
        {
            isAlive = false;
        }
    }
}
