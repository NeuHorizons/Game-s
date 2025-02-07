using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Game/Player Data")]
public class PlayerDataSO : ScriptableObject
{
    public int soulCount = 0;
    public bool dashUnlocked = false;
    public float playerSpeed = 5f;

    public void ResetData()
    {
        soulCount = 0;
        dashUnlocked = false;
        playerSpeed = 5f;
    }
}