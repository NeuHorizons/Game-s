using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    public float lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy after a few seconds
    }

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        if (collision.CompareTag("Spawner")) // Check if it hits a Spawner
        {
            BeaconHealth beacon = collision.GetComponent<BeaconHealth>();
            if (beacon != null)
            {
                beacon.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}