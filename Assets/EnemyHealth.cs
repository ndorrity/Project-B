using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 30f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Called by RaycastShootComplete when a bullet hits
    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Remaining: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Tell MenuManager a kill happened if you want to track it later
        if (MenuManager.Instance != null)
        {
            // Optional: call MenuManager.Instance.RecordEnemyKill(); if added
        }

        Destroy(gameObject);
    }
}
