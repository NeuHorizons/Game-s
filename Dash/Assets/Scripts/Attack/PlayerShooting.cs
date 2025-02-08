using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint; // Make sure this is a child of the Player
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    public PlayerDataSO playerData; // Reference to upgrade system
    public float aimLineLength = 3f; // Length of the trajectory line

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime) // Left Mouse Click to shoot
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation); // Correct rotation

        // Apply player upgrades
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.damage += playerData.attackDamageUpgrade; // Apply damage upgrade
        }
    }

    // This function draws an aim line in the Scene view
    private void OnDrawGizmos()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.green; // Color of the line
        Vector3 direction = firePoint.right; // Direction the projectile will travel
        Gizmos.DrawLine(firePoint.position, firePoint.position + direction * aimLineLength);
    }
}