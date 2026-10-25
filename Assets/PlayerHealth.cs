using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Optional UI")]
    public Slider healthSlider;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log($"Player took {damageAmount} damage. Remaining HP: {currentHealth}");

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player has died. Triggering Game Over screen.");

        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.ShowGameOver();
        }
        else
        {
            Debug.LogError("PlayerHealth: MenuManager.Instance is null!");
        }
    }
}
