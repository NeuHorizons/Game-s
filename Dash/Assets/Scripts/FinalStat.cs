using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalStat : MonoBehaviour
{
    public StatManager statManager;

    public TextMeshProUGUI finalDamageText;
    public TextMeshProUGUI finalMovementSpeedText;
    public TextMeshProUGUI finalHealthText;
    public TextMeshProUGUI finalAttackSpeedText;
    public TextMeshProUGUI finalStaminaText;

    void Start()
    {
        if (statManager == null)
        {
            Debug.LogError("StatManager is not assigned to FinalStat!");
            return;
        }

        UpdateFinalStats();
    }

    void Update()
    {
        UpdateFinalStats();
    }

    void UpdateFinalStats()
    {
        if (finalDamageText != null)
            finalDamageText.text = $"Damage: {statManager.FinalDamage}";
        if (finalMovementSpeedText != null)
            finalMovementSpeedText.text = $"Movement Speed: {statManager.FinalMovementSpeed}";
        if (finalHealthText != null)
            finalHealthText.text = $"Health: {statManager.FinalHealth}";
        if (finalAttackSpeedText != null)
            finalAttackSpeedText.text = $"Attack Speed: {statManager.FinalAttackSpeed}";
        if (finalStaminaText != null)
            finalStaminaText.text = $"Stamina: {statManager.FinalStamina}";
    }
}
