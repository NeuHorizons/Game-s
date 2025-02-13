using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public PlayerDataSO playerData;
    public int maxHealth = 100;
    public Slider healthSlider; // Reference to the UI Slider

    private void Start()
    {
        if (playerData == null)
        {
            Debug.LogError("PlayerDataSO is not assigned to PlayerHealth!");
            return;
        }

        playerData.Health = maxHealth; // Set initial health
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        playerData.Health -= damage;
        playerData.Health = Mathf.Clamp(playerData.Health, 0, maxHealth); // Prevent negative health

        Debug.Log("Player took " + damage + " damage. Current Health: " + playerData.Health);

        UpdateHealthUI();

        if (playerData.Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Implement death behavior (e.g., respawn, game over screen, restart level)
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = playerData.Health;
        }
        else
        {
            Debug.LogError("Health Slider not assigned in PlayerHealth!");
        }
    }
}