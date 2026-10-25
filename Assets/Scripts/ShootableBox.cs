using UnityEngine;

public class ShootableBox : MonoBehaviour 
{
    [Header("Health Settings")]
    public int currentHealth = 3;

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            // Inform the spawner to decrement active count and check for victory
            if (EnemySpawner.Instance != null)
            {
                EnemySpawner.Instance.OnEnemyKilled();
            }

            // Remove the eliminated NPC or target from the scene
            Destroy(gameObject);
        }
    }
}
