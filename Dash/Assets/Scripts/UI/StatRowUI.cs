using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Define the enum for stat types.
public enum StatType
{
    Damage,
    MovementSpeed,
    Health,
    AttackSpeed,
    Stamina
}

[ExecuteAlways]
public class StatRowUI : MonoBehaviour
{
    [Header("Cube Settings")]
    [Tooltip("Prefab for one cube. This prefab should be a UI element (with an Image component and RectTransform).")]
    public GameObject cubePrefab;
    [Tooltip("Total number of cubes in the row.")]
    public int maxCubes = 10;

    // List to store references to the instantiated cubes.
    private List<GameObject> cubes = new List<GameObject>();

    [Header("Visual Settings")]
    [Tooltip("Color when the cube is filled (active).")]
    public Color filledColor = Color.green;
    [Tooltip("Color when the cube is empty.")]
    public Color emptyColor = Color.gray;

    [Header("Stat Info")]
    [Tooltip("Select which stat this row represents.")]
    public StatType statType;
    [Tooltip("Reference to the PlayerData ScriptableObject to retrieve the stat level.")]
    public PlayerDataSO playerData;

    [Header("Upgrade Button Settings")]
    [Tooltip("Button at the end of the row that upgrades the stat if enough stat points are available.")]
    public Button upgradeButton;
    [Tooltip("Color for the upgrade button when an upgrade is available (glowing).")]
    public Color upgradeActiveColor = Color.yellow;
    [Tooltip("Color for the upgrade button when upgrade is unavailable (grey).")]
    public Color upgradeInactiveColor = Color.gray;

    [Header("Debug Settings")]
    [Tooltip("Tick this to force the UI row to reset in the Editor.")]
    public bool forceReset = false;

    private void OnEnable()
    {
        // In play mode, ensure cubes are adjusted to the desired count.
        if (Application.isPlaying)
        {
            // If needed, fully reset the cubes.
            if (forceReset)
            {
                forceReset = false;
                ResetCubes();
            }
            EnsureCubes();

            // Add listener to the upgrade button if assigned.
            if (upgradeButton != null)
            {
                upgradeButton.onClick.RemoveAllListeners();
                upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            }
        }
        UpdateUI();
    }

    private void OnValidate()
    {
        // In editor mode, if forceReset is checked, clear all cubes first.
        if (!Application.isPlaying)
        {
            if (forceReset)
            {
                forceReset = false;
                ResetCubes();
            }
            EnsureCubes();
        }
        UpdateUI();
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            UpdateUI();
        }
    }

    /// <summary>
    /// Ensures that there are exactly maxCubes cubes as children.
    /// If there are extras, they are removed.
    /// If there are too few, new cubes are added without altering the positions of the existing ones.
    /// </summary>
    private void EnsureCubes()
    {
        // Remove any extra cubes beyond maxCubes.
        for (int i = cubes.Count - 1; i >= 0; i--)
        {
            if (i >= maxCubes)
            {
                if (Application.isPlaying)
                    Destroy(cubes[i]);
                else
                    DestroyImmediate(cubes[i]);
                cubes.RemoveAt(i);
            }
        }

        // Add cubes if fewer than maxCubes exist.
        while (cubes.Count < maxCubes)
        {
            if (cubePrefab == null)
            {
                Debug.LogWarning("Cube Prefab is not assigned!");
                return;
            }
            // Instantiate the new cube as a child using the prefab's local settings.
            GameObject cube = Instantiate(cubePrefab, transform, false);
            cube.name = "Cube_" + cubes.Count;
            Image img = cube.GetComponent<Image>();
            if (img != null)
            {
                img.color = emptyColor;
            }
            cubes.Add(cube);
        }
    }

    /// <summary>
    /// Completely clears all cubes from the transform and resets the cubes list.
    /// Use this when a full reset is needed.
    /// </summary>
    private void ResetCubes()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
                Destroy(transform.GetChild(i).gameObject);
            else
                DestroyImmediate(transform.GetChild(i).gameObject);
        }
        cubes.Clear();
    }

    /// <summary>
    /// Retrieves the stat level from PlayerData based on the stat type and updates the UI.
    /// </summary>
    public void UpdateUI()
    {
        if (playerData == null)
            return;

        int statLevel = 0;
        switch (statType)
        {
            case StatType.Damage:
                statLevel = playerData.damageLevel;
                break;
            case StatType.MovementSpeed:
                statLevel = playerData.movementSpeedLevel;
                break;
            case StatType.Health:
                statLevel = playerData.healthLevel;
                break;
            case StatType.AttackSpeed:
                statLevel = playerData.attackSpeedLevel;
                break;
            case StatType.Stamina:
                statLevel = playerData.staminaLevel;
                break;
        }

        UpdateStatLevel(statLevel);
        UpdateUpgradeButton(statLevel);
    }

    /// <summary>
    /// Updates the row to fill in cubes based on the current stat level.
    /// </summary>
    /// <param name="statLevel">The current stat level (from 0 to maxCubes).</param>
    public void UpdateStatLevel(int statLevel)
    {
        statLevel = Mathf.Clamp(statLevel, 0, maxCubes);
        for (int i = 0; i < cubes.Count; i++)
        {
            Image img = cubes[i].GetComponent<Image>();
            if (img != null)
            {
                img.color = (i < statLevel) ? filledColor : emptyColor;
            }
        }
    }

    /// <summary>
    /// Updates the upgrade button's visual state based on whether an upgrade is available.
    /// The button is interactable (and glows) if the player has at least one stat point and the stat level is below maxCubes.
    /// </summary>
    /// <param name="statLevel">The current stat level.</param>
    private void UpdateUpgradeButton(int statLevel)
    {
        if (upgradeButton != null)
        {
            bool canUpgrade = (playerData.statPointsAvailable >= 1) && (statLevel < maxCubes);
            upgradeButton.interactable = canUpgrade;

            Image btnImg = upgradeButton.GetComponent<Image>();
            if (btnImg != null)
            {
                btnImg.color = canUpgrade ? upgradeActiveColor : upgradeInactiveColor;
            }
        }
    }

    /// <summary>
    /// Called when the upgrade button is clicked. If there are enough stat points, it upgrades the stat.
    /// </summary>
    private void OnUpgradeButtonClicked()
    {
        if (playerData == null)
            return;

        int statLevel = 0;
        switch (statType)
        {
            case StatType.Damage:
                statLevel = playerData.damageLevel;
                break;
            case StatType.MovementSpeed:
                statLevel = playerData.movementSpeedLevel;
                break;
            case StatType.Health:
                statLevel = playerData.healthLevel;
                break;
            case StatType.AttackSpeed:
                statLevel = playerData.attackSpeedLevel;
                break;
            case StatType.Stamina:
                statLevel = playerData.staminaLevel;
                break;
        }

        // Check if we can upgrade.
        if (playerData.statPointsAvailable >= 1 && statLevel < maxCubes)
        {
            playerData.statPointsAvailable--;  // Spend one stat point.
            switch (statType)
            {
                case StatType.Damage:
                    playerData.damageLevel++;
                    break;
                case StatType.MovementSpeed:
                    playerData.movementSpeedLevel++;
                    break;
                case StatType.Health:
                    playerData.healthLevel++;
                    break;
                case StatType.AttackSpeed:
                    playerData.attackSpeedLevel++;
                    break;
                case StatType.Stamina:
                    playerData.staminaLevel++;
                    break;
            }
            UpdateUI();
        }
    }
}
