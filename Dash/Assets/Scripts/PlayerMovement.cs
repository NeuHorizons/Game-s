using UnityEngine;
using System.Collections; 

public class PlayerMovement : MonoBehaviour
{
    public PlayerDataSO playerData; 
    private Vector2 moveInput;
    private Rigidbody2D rb; 

    public float dashSpeed = 10f; 
    public float dashDuration = 0.2f; 
    public float dashCooldown = 1f; 
    private bool isDashing = false;
    private float nextDashTime = 0f;

    private void Start()
    {
        if (playerData == null)
        {
            Debug.LogError("PlayerDataSO is not assigned in PlayerMovement!");
            return;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MovePlayer();
        RotateTowardsMouse();

        if (playerData.dashUnlocked && Input.GetKeyDown(KeyCode.Space) && Time.time >= nextDashTime)
        {
            StartCoroutine(Dash()); 
        }
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;
    }

    private void FixedUpdate()
    {
        if (!isDashing) 
        {
            rb.velocity = moveInput * playerData.playerSpeed;
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void CollectSoul(int amount)
    {
        playerData.soulCount += amount;
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        nextDashTime = Time.time + dashCooldown; 

        Vector2 dashDirection = moveInput; 
        if (dashDirection == Vector2.zero) 
        {
            dashDirection = transform.right;
        }

        rb.velocity = dashDirection * dashSpeed; 

        yield return new WaitForSeconds(dashDuration); 

        isDashing = false;
    }
}
