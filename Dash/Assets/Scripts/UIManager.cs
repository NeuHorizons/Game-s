using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject upgradePanel;
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI dashText;
    public Button speedButton;
    public Button dashButton;
    public Button upgradeButton; // Button to open/close menu

    public PlayerDataSO playerData; // Drag PlayerData.asset here in Inspector

    void Start()
    {
        if (playerData == null)
        {
            Debug.LogError("PlayerDataSO is not assigned to UIManager!");
            return;
        }

        // Add button listeners
        speedButton.onClick.AddListener(UpgradeSpeed);
        dashButton.onClick.AddListener(UnlockDash);
        upgradeButton.onClick.AddListener(ToggleUpgradeMenu);

        // Hide upgrade panel initially
        upgradePanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpgradeSpeed()
    {
        if (playerData.soulCount >= 10)
        {
            playerData.soulCount -= 10;
            playerData.playerSpeed += 1f;
        }
    }

    void UnlockDash()
    {
        if (playerData.soulCount >= 20)
        {
            playerData.soulCount -= 20;
            playerData.dashUnlocked = true;
        }
    }

    void UpgradeAttack()
    {
        if (playerData.soulCount >= 15) // Cost of upgrade
        {
            playerData.soulCount -= 15;
            playerData.attackDamageUpgrade += 1; // Increases projectile damage
        }
    }
    void ToggleUpgradeMenu()
    {
        upgradePanel.SetActive(!upgradePanel.activeSelf);
    }

    void UpdateUI()
    {
        // Update all text values based on PlayerDataSO
        soulText.text = $"Souls: {playerData.soulCount}";
        speedText.text = $"Speed: {playerData.playerSpeed}";
        dashText.text = $"Dash: {(playerData.dashUnlocked ? "Unlocked" : "Locked")}";

        // Disable buttons if the player doesn't have enough souls
        speedButton.interactable = playerData.soulCount >= 10;
        dashButton.interactable = playerData.soulCount >= 20;
    }
}