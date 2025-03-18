using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data")]
public class PlayerDataSO : ScriptableObject
{
    // Base Player Data (non-progression related)
    public int soulCount = 0;
    public bool dashUnlocked = false;
    
    // Character Progression Data
    public int currentLevel = 1;
    public float currentExp = 0f;
    public float expToNextLevel = 100f;
    public int statPointsAvailable = 0;

    // NEW FIELDS FOR FLOOR MANAGER
    public int currentFloor = 1;
    public int highestFloor = 1;

    // Consolidated Stats:
    // Base values represent points allocated via level-ups.
    // Modifiers represent natural progression earned through gameplay.
    [Header("Base Stats (Level-Up Allocated)")]
    public float baseMovementSpeed = 5f;
    public int baseDamage = 10;
    public int baseHealth = 100;
    public float baseAttackSpeed = 0.5f;
    public float baseStamina = 100f;

    [Header("Natural Progression Modifiers")]
    public float movementSpeedModifier = 0f;
    public int damageModifier = 0;
    public int healthModifier = 0;
    public float attackSpeedModifier = 0f;
    public float staminaModifier = 0f;

    // Computed properties to get the overall stat values.
    public float MovementSpeed { get { return baseMovementSpeed + movementSpeedModifier; } }
    public int Damage { get { return baseDamage + damageModifier; } }
    public int Health { get { return baseHealth + healthModifier; } }
    public float AttackSpeed { get { return baseAttackSpeed + attackSpeedModifier; } }
    public float Stamina { get { return baseStamina + staminaModifier; } }

    /// <summary>
    /// Resets all player data back to its default values.
    /// </summary>
    public void ResetData()
    {
        // Reset base player data
        soulCount = 0;
        dashUnlocked = false;

        // Reset progression data
        currentLevel = 1;
        currentExp = 0f;
        expToNextLevel = 100f;
        statPointsAvailable = 0;

        // Reset floor data
        currentFloor = 1;
        highestFloor = 1;

        // Reset base stats (level-up allocated)
        baseMovementSpeed = 5f;
        baseDamage = 10;
        baseHealth = 100;
        baseAttackSpeed = 0.5f;
        baseStamina = 100f;

        // Reset natural progression modifiers
        movementSpeedModifier = 0f;
        damageModifier = 0;
        healthModifier = 0;
        attackSpeedModifier = 0f;
        staminaModifier = 0f;
    }
}
