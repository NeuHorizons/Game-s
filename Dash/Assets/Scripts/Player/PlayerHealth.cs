using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public PlayerDataSO playerData;
    public StatManager statManager;
    public Slider healthSlider;
    private int currentHealth;
    private TextMeshProUGUI healthText;

    void Start()
    {
        if (playerData == null)
        {
            Debug.LogError("PlayerDataSO is not assigned to PlayerHealth!");
            return;
        }
        if (statManager == null)
        {
            Debug.LogError("StatManager is not assigned to PlayerHealth!");
            return;
        }
        if (healthSlider == null)
        {
            healthSlider = FindObjectOfType<Slider>();
            if (healthSlider == null)
            {
                Debug.LogError("No Health Slider found in the scene!");
                return;
            }
        }

        healthText = GameObject.Find("Player Health").GetComponent<TextMeshProUGUI>();
        if (healthText == null)
        {
            Debug.LogError("No TextMeshProUGUI component found on the Player Health object!");
            return;
        }

        currentHealth = statManager.FinalHealth;
        healthSlider.maxValue = statManager.FinalHealth;
        healthSlider.value = currentHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, statManager.FinalHealth);
        Debug.Log("Player took " + damage + " damage. Current Health: " + currentHealth);
        UpdateHealthUI();
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        EnemyDetection.ResetHiveMind();
        GameManager.Instance.PlayerDied();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = statManager.FinalHealth;
            healthSlider.value = currentHealth;
        }
        else
        {
            Debug.LogError("Health Slider not assigned in PlayerHealth!");
        }

        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}";
        }
        else
        {
            Debug.LogError("Health Text not assigned in PlayerHealth!");
        }
    }
}
