using UnityEngine;
using System.Collections;

public class EnemyCircling : MonoBehaviour
{
    public float speed = 2f;
    public float circleRadius = 3f;
    public float rotationSpeed = 2f;
    public float lungeSpeed = 8f; // Speed during the lunge
    public float lungeDuration = 0.5f; // How long the lunge lasts
    public float minLungeDelay = 2f; // Min time between lunges
    public float maxLungeDelay = 5f; // Max time between lunges

    private Transform player;
    private Vector2 circleCenter;
    private bool isLunging = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player != null)
        {
            circleCenter = player.position;
        }
        StartCoroutine(LungeRoutine());
    }

    private void Update()
    {
        if (player != null && !isLunging)
        {
            // Keep updating the center position to follow the player
            circleCenter = player.position;

            // Move in a circular motion around the player
            float angle = Time.time * rotationSpeed;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * circleRadius;
            transform.position = Vector2.Lerp(transform.position, (Vector2)circleCenter + offset, speed * Time.deltaTime);
        }
    }

    IEnumerator LungeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minLungeDelay, maxLungeDelay)); // Wait for a random time
            if (player != null)
            {
                StartCoroutine(LungeAtPlayer());
            }
        }
    }

    IEnumerator LungeAtPlayer()
    {
        isLunging = true;
        Vector2 lungeDirection = (player.position - transform.position).normalized; // Get direction to player

        float lungeTime = 0f;
        while (lungeTime < lungeDuration)
        {
            transform.position += (Vector3)(lungeDirection * lungeSpeed * Time.deltaTime);
            lungeTime += Time.deltaTime;
            yield return null;
        }

        isLunging = false;
    }
}
