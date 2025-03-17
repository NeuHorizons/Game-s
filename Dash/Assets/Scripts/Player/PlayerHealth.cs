using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public PlayerDataSO playerData; // Uses consolidated stats from PlayerDataSO
    // The maximum health is now derived from playerData.Health (baseHealth + healthModifier)
    public Slider healthSlider; // Reference to the UI Slider

    // Track current health locally (separate from the maximum health provided by PlayerDataSO)
    private int currentHealth;

    private void Start()
    {
        if (playerData == null)
        {
            Debug.LogError("PlayerDataSO is not assigned to PlayerHealth!");
            return;
        }

        // If healthSlider is not assigned, attempt to find it automatically.
        if (healthSlider == null)
        {
            healthSlider = FindObjectOfType<Slider>();
            if (healthSlider == null)
            {
                Debug.LogError("No Health Slider found in the scene!");
                return;
            }
        }

        // Set currentHealth based on the player's maximum health from PlayerDataSO.
        currentHealth = playerData.Health;

        // Setup the slider with the maximum health.
        healthSlider.maxValue = playerData.Health;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // Clamp current health between 0 and the maximum health from playerData.
        currentHealth = Mathf.Clamp(currentHealth, 0, playerData.Health);

        Debug.Log("Player took " + damage + " damage. Current Health: " + currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Reset enemy behavior before handling death (or after, depending on your GameManager logic)
        EnemyDetection.ResetHiveMind();
        GameManager.Instance.PlayerDied(); // This could also reload the scene
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            // In case the player's maximum health has changed (via upgrades), update the slider's max value.
            healthSlider.maxValue = playerData.Health;
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogError("Health Slider not assigned in PlayerHealth!");
        }
    }
}
