using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject upgradePanel;
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI fireRateText; // NEW: Fire Rate Display
    public Button speedButton;
    public Button dashButton;
    public Button attackButton;
    public Button fireRateButton; // NEW: Fire Rate Upgrade Button
    public Button upgradeButton;

    public PlayerDataSO playerData;
    private bool isNearMerchant = false;
    private bool isMenuOpen = false;

    void Start()
    {
        if (playerData == null)
        {
            Debug.LogError("PlayerDataSO is not assigned to UIManager!");
            return;
        }

        speedButton.onClick.AddListener(UpgradeSpeed);
        dashButton.onClick.AddListener(UnlockDash);
        attackButton.onClick.AddListener(UpgradeAttack);
        fireRateButton.onClick.AddListener(UpgradeFireRate); // NEW: Fire Rate Upgrade
        upgradeButton.onClick.AddListener(ToggleUpgradeMenu);

        upgradePanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();

        if (isNearMerchant && Input.GetKeyDown(KeyCode.E))
        {
            ToggleUpgradeMenu();
        }
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
        if (playerData.soulCount >= 20)
        {
            playerData.soulCount -= 20;
            playerData.attackDamageUpgrade += 1;
        }
    }

    void UpgradeFireRate()
    {
        if (playerData.soulCount >= 20) // Fire Rate Upgrade Cost
        {
            playerData.soulCount -= 20;
            playerData.fireRate = Mathf.Max(0.1f, playerData.fireRate - 0.05f); // Decrease Fire Rate but prevent it from being too fast
        }
    }

    void ToggleUpgradeMenu()
    {
        isMenuOpen = !isMenuOpen;
        upgradePanel.SetActive(isMenuOpen);

        if (isMenuOpen)
        {
            Time.timeScale = 0; 
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void UpdateUI()
    {
        soulText.text = $"Souls: {playerData.soulCount}";
        speedText.text = $"Speed: {playerData.playerSpeed}";
        dashText.text = $"Dash: {(playerData.dashUnlocked ? "Unlocked" : "Locked")}";
        attackText.text = $"Attack: {playerData.attackDamageUpgrade}";
        fireRateText.text = $"Fire Rate: {playerData.fireRate:F2} sec"; // NEW: Displays Fire Rate

        speedButton.interactable = playerData.soulCount >= 10;
        dashButton.interactable = playerData.soulCount >= 20;
        attackButton.interactable = playerData.soulCount >= 15;
        fireRateButton.interactable = playerData.soulCount >= 20; // NEW: Fire Rate Upgrade Button
    }

    public void SetMerchantProximity(bool isNear)
    {
        isNearMerchant = isNear;
    }
}
