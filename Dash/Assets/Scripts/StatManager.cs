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

        // For testing: press the R key to simulate dealing damage (natural progression for damage).
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
    /// Also updates the stat level fields.
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

        // Increase stat levels accordingly.
        playerData.movementSpeedLevel += movementSpeedPoints;
        playerData.damageLevel += damagePoints;
        playerData.healthLevel += healthPoints;
        playerData.attackSpeedLevel += attackSpeedPoints;
        playerData.staminaLevel += staminaPoints;

        playerData.statPointsAvailable -= totalPoints;
    }

    /// <summary>
    /// Increases the natural progression for damage based on damage dealt.
    /// Also processes natural progression conversion.
    /// </summary>
    /// <param name="damage">Amount of damage dealt.</param>
    public void AddDamageDealt(float damage)
    {
        // Increase natural progression points for damage.
        playerData.damageNaturalPoints += Mathf.RoundToInt(damage * 0.01f);
        ProcessDamageNaturalProgression();
    }

    /// <summary>
    /// Processes natural progression for Damage.
    /// When accumulated damageNaturalPoints reach the threshold (current damage level + 1) * 10,
    /// they are converted into an increase in the damage stat level.
    /// </summary>
    public void ProcessDamageNaturalProgression()
    {
        int threshold = (playerData.damageLevel + 1) * 10;
        while (playerData.damageNaturalPoints >= threshold)
        {
            playerData.damageNaturalPoints -= threshold;
            playerData.damageLevel++;
            threshold = (playerData.damageLevel + 1) * 10;
            Debug.Log("Natural progression: Damage level increased to " + playerData.damageLevel);
        }
    }

    /// <summary>
    /// Increases the natural progression points for movement speed.
    /// </summary>
    /// <param name="distance">Distance travelled (e.g., in units).</param>
    public void AddDistanceTravelled(float distance)
    {
        playerData.movementSpeedNaturalPoints += distance * 0.05f;
        ProcessMovementSpeedNaturalProgression();
    }

    /// <summary>
    /// Processes natural progression for Movement Speed.
    /// When movementSpeedNaturalPoints reach the threshold (current movementSpeedLevel + 1) * 10,
    /// they are converted into an increase in the movement speed stat level.
    /// </summary>
    public void ProcessMovementSpeedNaturalProgression()
    {
        int threshold = (playerData.movementSpeedLevel + 1) * 10;
        while (playerData.movementSpeedNaturalPoints >= threshold)
        {
            playerData.movementSpeedNaturalPoints -= threshold;
            playerData.movementSpeedLevel++;
            threshold = (playerData.movementSpeedLevel + 1) * 10;
            Debug.Log("Natural progression: Movement Speed level increased to " + playerData.movementSpeedLevel);
        }
    }

    /// <summary>
    /// Increases the natural progression points for health.
    /// </summary>
    /// <param name="healthGain">Amount of health gained naturally.</param>
    public void AddHealthGain(int healthGain)
    {
        playerData.healthNaturalPoints += healthGain;
        ProcessHealthNaturalProgression();
    }

    /// <summary>
    /// Processes natural progression for Health.
    /// When healthNaturalPoints reach the threshold (current healthLevel + 1) * 10,
    /// they are converted into an increase in the health stat level.
    /// </summary>
    public void ProcessHealthNaturalProgression()
    {
        int threshold = (playerData.healthLevel + 1) * 10;
        while (playerData.healthNaturalPoints >= threshold)
        {
            playerData.healthNaturalPoints -= threshold;
            playerData.healthLevel++;
            threshold = (playerData.healthLevel + 1) * 10;
            Debug.Log("Natural progression: Health level increased to " + playerData.healthLevel);
        }
    }

    /// <summary>
    /// Increases the natural progression points for attack speed.
    /// </summary>
    public void AddAttackAction()
    {
        playerData.attackSpeedNaturalPoints += 0.1f;
        ProcessAttackSpeedNaturalProgression();
    }

    /// <summary>
    /// Processes natural progression for Attack Speed.
    /// When attackSpeedNaturalPoints reach the threshold (current attackSpeedLevel + 1) * 10,
    /// they are converted into an increase in the attack speed stat level.
    /// </summary>
    public void ProcessAttackSpeedNaturalProgression()
    {
        int threshold = (playerData.attackSpeedLevel + 1) * 10;
        while (playerData.attackSpeedNaturalPoints >= threshold)
        {
            playerData.attackSpeedNaturalPoints -= threshold;
            playerData.attackSpeedLevel++;
            threshold = (playerData.attackSpeedLevel + 1) * 10;
            Debug.Log("Natural progression: Attack Speed level increased to " + playerData.attackSpeedLevel);
        }
    }

    /// <summary>
    /// Increases the natural progression points for stamina.
    /// </summary>
    /// <param name="usage">Usage value (e.g., amount of stamina used).</param>
    public void AddStaminaUsage(float usage)
    {
        playerData.staminaNaturalPoints += usage * 0.03f;
        ProcessStaminaNaturalProgression();
    }

    /// <summary>
    /// Processes natural progression for Stamina.
    /// When staminaNaturalPoints reach the threshold (current staminaLevel + 1) * 10,
    /// they are converted into an increase in the stamina stat level.
    /// </summary>
    public void ProcessStaminaNaturalProgression()
    {
        int threshold = (playerData.staminaLevel + 1) * 10;
        while (playerData.staminaNaturalPoints >= threshold)
        {
            playerData.staminaNaturalPoints -= threshold;
            playerData.staminaLevel++;
            threshold = (playerData.staminaLevel + 1) * 10;
            Debug.Log("Natural progression: Stamina level increased to " + playerData.staminaLevel);
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
