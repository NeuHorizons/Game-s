using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data")]
public class PlayerDataSO : ScriptableObject
{
    public int Health = 100;
    public int soulCount = 0;
    public bool dashUnlocked = false;
    public float playerSpeed = 5f;
    public int attackDamageUpgrade = 0;
    public float fireRate = 0.5f;

    public void ResetData()
    {
        Health = 100;
        soulCount = 0;
        dashUnlocked = false;
        playerSpeed = 5f;
        attackDamageUpgrade = 0;
        fireRate = 0.5f;
    }
}