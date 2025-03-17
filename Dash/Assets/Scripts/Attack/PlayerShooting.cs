using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    private float nextFireTime = 0f;

    public PlayerDataSO playerData; // Uses consolidated stats from PlayerDataSO

    private void Update()
    {
        // Check if the left mouse button is held down and if enough time has passed based on AttackSpeed.
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            // Use the computed AttackSpeed from PlayerDataSO, which is baseAttackSpeed + attackSpeedModifier.
            nextFireTime = Time.time + playerData.AttackSpeed;
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Apply the player's damage stat (which is baseDamage + damageModifier) to the projectile.
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            // Optionally, you might add or set the projectile's damage.
            projectileScript.damage += playerData.Damage;
        }
    }

    // Debugging: Draw aim line in the Scene view.
    private void OnDrawGizmos()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.green;
        Vector3 direction = firePoint.right;
        Gizmos.DrawLine(firePoint.position, firePoint.position + direction * 3f);
    }
}