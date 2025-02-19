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
        if (playerData.soulCount >= 15)
        {
            playerData.soulCount -= 15;
            playerData.attackDamageUpgrade += 1;
        }
    }

    void ToggleUpgradeMenu()
    {
        isMenuOpen = !isMenuOpen;
        upgradePanel.SetActive(isMenuOpen);

        if (isMenuOpen)
        {
            Time.timeScale = 0; // Pause the game
        }
        else
        {
            Time.timeScale = 1; // Resume the game
        }
    }

    void UpdateUI()
    {
        soulText.text = $"Souls: {playerData.soulCount}";
        speedText.text = $"Speed: {playerData.playerSpeed}";
        dashText.text = $"Dash: {(playerData.dashUnlocked ? "Unlocked" : "Locked")}";

        speedButton.interactable = playerData.soulCount >= 10;
        dashButton.interactable = playerData.soulCount >= 20;
    }

    public void SetMerchantProximity(bool isNear)
    {
        isNearMerchant = isNear;
    }
}
