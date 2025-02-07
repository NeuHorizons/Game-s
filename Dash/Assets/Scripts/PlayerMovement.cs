using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerDataSO playerData; // Drag PlayerData.asset here in Inspector

    private Vector2 moveInput;

    private void Start()
    {
        if (playerData == null)
        {
            Debug.LogError("PlayerDataSO is not assigned in PlayerMovement!");
            return;
        }
    }

    void Update()
    {
        MovePlayer();

        if (playerData.dashUnlocked && Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;
        transform.position += (Vector3)moveInput * playerData.playerSpeed * Time.deltaTime;
    }

    void Dash()
    {
        transform.position += (Vector3)moveInput * 2f; // Dash distance
    }

    public void CollectSoul(int amount)
    {
        playerData.soulCount += amount;
    }
}