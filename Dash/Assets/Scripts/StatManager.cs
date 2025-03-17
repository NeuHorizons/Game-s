using UnityEngine;
using System;

public class StatManager : MonoBehaviour
{
    [Header("Player Data Reference")]
    // Assign your PlayerDataSO asset via the Inspector.
    public PlayerDataSO playerData;

    // Optional event for level-up notifications (for UI or other systems)
    public event Action<int> OnLevelUp;

    void Update()
    {
        // For testing: press the E key to simulate gaining experience.
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddExperience(20);
        }

        // For testing: press the D key to simulate dealing damage (natural progression).
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddDamageDealt(50);
        }
    }

    /// <summary>
    /// Adds experience and checks for level up.
    /// </summary>
    /// <param name="expAmount">Amount of experience to add.</param>
    public void AddExperience(float expAmount)
    {
        playerData.currentExp += expAmount;
        while (playerData.currentExp >= playerData.expToNextLevel && playerData.currentLevel < 100)
        {
            playerData.currentExp -= playerData.expToNextLevel;
            LevelUp();
        }
    }

    /// <summary>
    /// Handles leveling up: increases level, grants stat points, and scales the experience requirement.
    /// </summary>
    void LevelUp()
    {
        playerData.currentLevel++;
        playerData.statPointsAvailable += 5; // Example: grant 5 points per level.
        playerData.expToNextLevel *= 1.1f;

        OnLevelUp?.Invoke(playerData.currentLevel);
        Debug.Log("Leveled Up! New level: " + playerData.currentLevel);
    }

    /// <summary>
    /// Allocates stat points to upgrade base stats. 
    /// For example, to upgrade damage directly from level-ups.
    /// </summary>
    public void AllocateStatPoints(int movementSpeedPoints, int damagePoints, int healthPoints, int attackSpeedPoints, int staminaPoints)
    {
        int totalPoints = movementSpeedPoints + damagePoints + healthPoints + attackSpeedPoints + staminaPoints;
        if (totalPoints > playerData.statPointsAvailable)
        {
            Debug.LogWarning("Not enough stat points available!");
            return;
        }

        // Upgrade base stats directly.
        playerData.baseMovementSpeed += movementSpeedPoints;
        playerData.baseDamage += damagePoints;
        playerData.baseHealth += healthPoints;
        playerData.baseAttackSpeed += attackSpeedPoints;
        playerData.baseStamina += staminaPoints;

        playerData.statPointsAvailable -= totalPoints;
    }

    /// <summary>
    /// Increases the natural progression for damage based on damage dealt.
    /// Also processes natural progression conversion.
    /// </summary>
    /// <param name="damage">Amount of damage dealt.</param>
    public void AddDamageDealt(float damage)
    {
        // Increase natural damage modifier (adjust the multiplier as needed).
        playerData.damageModifier += Mathf.RoundToInt(damage * 0.01f);
        ProcessDamageNaturalProgression();
    }

    /// <summary>
    /// Processes natural progression for Damage.
    /// As natural damage points (damageModifier) accumulate,
    /// when they reach a threshold (baseDamage * 10), they are converted
    /// into a direct increase in baseDamage.
    /// </summary>
    public void ProcessDamageNaturalProgression()
    {
        // Example threshold: required natural points equal to baseDamage * 10.
        int threshold = Mathf.CeilToInt(playerData.baseDamage * 10f);

        // Convert natural progression points into base damage upgrades.
        while (playerData.damageModifier >= threshold)
        {
            playerData.damageModifier -= threshold;
            playerData.baseDamage += 1;
            // Recalculate threshold for the next upgrade.
            threshold = Mathf.CeilToInt(playerData.baseDamage * 10f);
        }
    }

    /// <summary>
    /// Increases the natural progression modifier for movement speed.
    /// </summary>
    /// <param name="distance">Distance travelled (e.g., in units).</param>
    public void AddDistanceTravelled(float distance)
    {
        playerData.movementSpeedModifier += distance * 0.05f;
    }

    /// <summary>
    /// Increases the natural progression modifier for health.
    /// </summary>
    /// <param name="healthGain">Amount of health gained naturally.</param>
    public void AddHealthGain(int healthGain)
    {
        playerData.healthModifier += healthGain;
    }

    /// <summary>
    /// Increases the natural progression modifier for attack speed.
    /// </summary>
    public void AddAttackAction()
    {
        playerData.attackSpeedModifier += 0.1f;
    }

    /// <summary>
    /// Increases the natural progression modifier for stamina.
    /// </summary>
    /// <param name="usage">Usage value (e.g., amount of stamina used).</param>
    public void AddStaminaUsage(float usage)
    {
        playerData.staminaModifier += usage * 0.03f;
    }

    // --- NEW NATURAL PROGRESSION PROCESSING METHODS ---

    /// <summary>
    /// Processes natural progression for Movement Speed.
    /// When movementSpeedModifier reaches (baseMovementSpeed * 10),
    /// automatically increases baseMovementSpeed by 1 and subtracts the threshold.
    /// </summary>
    public void ProcessMovementSpeedNaturalProgression()
    {
        float threshold = playerData.baseMovementSpeed * 10f;
        while (playerData.movementSpeedModifier >= threshold)
        {
            playerData.movementSpeedModifier -= threshold;
            playerData.baseMovementSpeed += 1f;
            threshold = playerData.baseMovementSpeed * 10f;
        }
    }

    /// <summary>
    /// Processes natural progression for Health.
    /// When healthModifier reaches (baseHealth * 10),
    /// automatically increases baseHealth by 1 and subtracts the threshold.
    /// </summary>
    public void ProcessHealthNaturalProgression()
    {
        int threshold = Mathf.CeilToInt(playerData.baseHealth * 10f);
        while (playerData.healthModifier >= threshold)
        {
            playerData.healthModifier -= threshold;
            playerData.baseHealth += 1;
            threshold = Mathf.CeilToInt(playerData.baseHealth * 10f);
        }
    }

    /// <summary>
    /// Processes natural progression for Attack Speed.
    /// When attackSpeedModifier reaches (baseAttackSpeed * 10),
    /// automatically increases baseAttackSpeed by 1 and subtracts the threshold.
    /// </summary>
    public void ProcessAttackSpeedNaturalProgression()
    {
        float threshold = playerData.baseAttackSpeed * 10f;
        while (playerData.attackSpeedModifier >= threshold)
        {
            playerData.attackSpeedModifier -= threshold;
            playerData.baseAttackSpeed += 1f;
            threshold = playerData.baseAttackSpeed * 10f;
        }
    }

    /// <summary>
    /// Processes natural progression for Stamina.
    /// When staminaModifier reaches (baseStamina * 10),
    /// automatically increases baseStamina by 1 and subtracts the threshold.
    /// </summary>
    public void ProcessStaminaNaturalProgression()
    {
        float threshold = playerData.baseStamina * 10f;
        while (playerData.staminaModifier >= threshold)
        {
            playerData.staminaModifier -= threshold;
            playerData.baseStamina += 1f;
            threshold = playerData.baseStamina * 10f;
        }
    }

    /// <summary>
    /// Processes all natural progression upgrades for every stat.
    /// </summary>
    public void ProcessAllNaturalProgressions()
    {
        ProcessDamageNaturalProgression();
        ProcessMovementSpeedNaturalProgression();
        ProcessHealthNaturalProgression();
        ProcessAttackSpeedNaturalProgression();
        ProcessStaminaNaturalProgression();
    }
}
