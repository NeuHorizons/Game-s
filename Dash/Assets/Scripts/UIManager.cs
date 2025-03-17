using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // -------- Main Menu UI --------
    [Header("Main Menu UI")]
    public GameObject mainMenuPanel; // Panel for main menu (toggle on/off)

    // -------- Upgrade UI --------
    [Header("Upgrade UI")]
    public GameObject upgradePanel; // Panel for upgrades
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI attackSpeedText; // Replacing fireRateText with attackSpeedText

    public Button speedButton;
    public Button dashButton;
    public Button attackButton;
    public Button attackSpeedButton; // Replacing fireRateButton with attackSpeedButton
    public Button upgradeButton; // Optionally used to toggle the upgrade menu

    // -------- Player Data & Merchant Proximity --------
    public PlayerDataSO playerData; // Your ScriptableObject holding player data
    private bool isNearMerchant = false;
    private bool isUpgradeMenuOpen = false;

    void Start()
    {
        // Validate playerData assignment
        if (playerData == null)
        {
            Debug.LogError("PlayerDataSO is not assigned to UIManager!");
            return;
        }

        // Setup button listeners for upgrades
        speedButton.onClick.AddListener(UpgradeSpeed);
        dashButton.onClick.AddListener(UnlockDash);
        attackButton.onClick.AddListener(UpgradeAttack);
        attackSpeedButton.onClick.AddListener(UpgradeAttackSpeed);

        // Optionally assign the upgrade menu toggle to a button
        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(ToggleUpgradeMenu);

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        UpdateUI();
    }

    void Update()
    {
        // Update UI every frame
        UpdateUI();

        // If near merchant and the player presses "E", toggle the upgrade menu
        if (isNearMerchant && Input.GetKeyDown(KeyCode.E))
        {
            ToggleUpgradeMenu();
        }
    }

    // -------- Main Menu Methods --------

    // Toggle the main menu panel on/off
    public void ToggleMainMenuPanel()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(!mainMenuPanel.activeSelf);
    }

    // Change the scene by name (ensure the scene is in Build Settings)
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Quit the application
    public void QuitGame()
    {
        Application.Quit();
    }
    
    // -------- Death Option Method --------
    // Call this method (e.g., from a UI button) to reset time and load the death scene.
    public void Death()
    {
        Time.timeScale = 1;
        // You can load the death scene here if needed
    }

    // -------- Upgrade Methods --------

    // Upgrade movement speed by increasing the baseMovementSpeed.
    void UpgradeSpeed()
    {
        if (playerData.soulCount >= 10)
        {
            playerData.soulCount -= 10;
            playerData.baseMovementSpeed += 1f;
            UpdateUI();
        }
    }

    // Unlock dash ability.
    void UnlockDash()
    {
        if (playerData.soulCount >= 20)
        {
            playerData.soulCount -= 20;
            playerData.dashUnlocked = true;
            UpdateUI();
        }
    }

    // Upgrade attack by increasing baseDamage.
    void UpgradeAttack()
    {
        if (playerData.soulCount >= 20)
        {
            playerData.soulCount -= 20;
            playerData.baseDamage += 1;
            UpdateUI();
        }
    }

    // Upgrade attack speed by decreasing baseAttackSpeed (ensuring it doesn't drop below a limit).
    void UpgradeAttackSpeed()
    {
        if (playerData.soulCount >= 20)
        {
            playerData.soulCount -= 20;
            playerData.baseAttackSpeed = Mathf.Max(0.1f, playerData.baseAttackSpeed - 0.05f);
            UpdateUI();
        }
    }

    // Toggle the upgrade menu panel and pause/resume the game accordingly.
    void ToggleUpgradeMenu()
    {
        isUpgradeMenuOpen = !isUpgradeMenuOpen;
        if (upgradePanel != null)
            upgradePanel.SetActive(isUpgradeMenuOpen);

        // Pause the game when the upgrade menu is open.
        Time.timeScale = isUpgradeMenuOpen ? 0 : 1;
    }

    // Update UI text elements and button interactability based on player data.
    void UpdateUI()
    {
        if (playerData == null)
            return;

        if (soulText != null)
            soulText.text = $"Souls: {playerData.soulCount}";

        // Use the computed properties to display overall stat values.
        if (speedText != null)
            speedText.text = $"Speed: {playerData.MovementSpeed:F2}";
        if (dashText != null)
            dashText.text = $"Dash: {(playerData.dashUnlocked ? "Unlocked" : "Locked")}";
        if (attackText != null)
            attackText.text = $"Attack: {playerData.Damage}";
        if (attackSpeedText != null)
            attackSpeedText.text = $"Attack Speed: {playerData.AttackSpeed:F2} sec";

        // Update button interactability based on soul count.
        if (speedButton != null)
            speedButton.interactable = playerData.soulCount >= 10;
        if (dashButton != null)
            dashButton.interactable = playerData.soulCount >= 20;
        if (attackButton != null)
            attackButton.interactable = playerData.soulCount >= 20;
        if (attackSpeedButton != null)
            attackSpeedButton.interactable = playerData.soulCount >= 20;
    }

    // This method can be called by external triggers (e.g., collision triggers) to update merchant proximity status.
    public void SetMerchantProximity(bool isNear)
    {
        isNearMerchant = isNear;
    }
}
