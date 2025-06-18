using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 10;
    public bool canDealDamage = false;

    public int GetDamage() => damage;
    public bool IsActive() => canDealDamage;
}
