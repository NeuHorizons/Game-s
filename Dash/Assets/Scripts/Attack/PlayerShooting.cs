using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    private float nextFireTime = 0f;

    public PlayerDataSO playerData; // Pull fire rate from scriptable object

    private void Update()
    {
        // Check if left mouse button is held down
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + playerData.fireRate; // Get fire rate from PlayerDataSO
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Apply player upgrades
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.damage += playerData.attackDamageUpgrade; // Apply damage upgrade
        }
    }

    // Debugging: Draw aim line in Scene view
    private void OnDrawGizmos()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.green;
        Vector3 direction = firePoint.right;
        Gizmos.DrawLine(firePoint.position, firePoint.position + direction * 3f);
    }
}