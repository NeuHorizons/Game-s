using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Main Menu UI")]
    public GameObject mainMenuPanel;

    [Header("Upgrade UI")]
    public GameObject upgradePanel;
    public TextMeshProUGUI soulText;
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI statPointsText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI characterNameText;

    public Button upgradeButton;

    public PlayerDataSO playerData;
    private bool isNearMerchant = false;
    private bool isUpgradeMenuOpen = false;

    void Start()
    {
        if (playerData == null)
        {
            Debug.LogError("PlayerDataSO is not assigned to UIManager!");
            return;
        }

        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(ToggleUpgradeMenu);

        if (upgradePanel != null)
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

    public void ToggleMainMenuPanel()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(!mainMenuPanel.activeSelf);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Death()
    {
        Time.timeScale = 1;
    }

    void UnlockDash()
    {
        if (playerData.soulCount >= 20)
        {
            playerData.soulCount -= 20;
            playerData.dashUnlocked = true;
            UpdateUI();
        }
    }

    void ToggleUpgradeMenu()
    {
        isUpgradeMenuOpen = !isUpgradeMenuOpen;
        if (upgradePanel != null)
            upgradePanel.SetActive(isUpgradeMenuOpen);

        Time.timeScale = isUpgradeMenuOpen ? 0 : 1;
    }

    void UpdateUI()
    {
        if (playerData == null)
            return;

        if (soulText != null)
            soulText.text = $"Souls: {playerData.soulCount}";
        if (dashText != null)
            dashText.text = $"Dash: {(playerData.dashUnlocked ? "Unlocked" : "Locked")}";
        if (statPointsText != null)
            statPointsText.text = $"Stat Points: {playerData.statPointsAvailable}";
        if (levelText != null)
            levelText.text = $"Level: {playerData.currentLevel}";
        if (characterNameText != null)
            characterNameText.text = $"Name: {playerData.characterName}";
    }

    public void SetMerchantProximity(bool isNear)
    {
        isNearMerchant = isNear;
    }
}
