using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public PlayerDataSO playerData; // Assign PlayerData.asset in Inspector

    public void UpgradeSpeed()
    {
        if (playerData.soulCount >= 10)
        {
            playerData.soulCount -= 10;
            playerData.playerSpeed += 1f;
        }
    }

    public void UnlockDash()
    {
        if (playerData.soulCount >= 20)
        {
            playerData.soulCount -= 20;
            playerData.dashUnlocked = true;
        }
    }
}