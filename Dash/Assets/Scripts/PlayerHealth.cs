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

        // ✅ If healthSlider is not assigned, find it automatically
        if (healthSlider == null)
        {
            healthSlider = FindObjectOfType<Slider>();

            if (healthSlider == null)
            {
                Debug.LogError("⚠️ No Health Slider found in the scene!");
                return;
            }
        }

        playerData.Health = maxHealth; // Set initial health

        // ✅ Ensure the slider is set up correctly
        healthSlider.maxValue = maxHealth;
        healthSlider.value = (float)playerData.Health;
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
            healthSlider.value = (float)playerData.Health; // ✅ Fix: Convert to float
        }
        else
        {
            Debug.LogError("Health Slider not assigned in PlayerHealth!");
        }
    }
}