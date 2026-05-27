using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 1;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            GameManager.Instance.AddKill();
            Destroy(gameObject);
        }
    }
}