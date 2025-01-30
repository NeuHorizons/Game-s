using UnityEngine;
using System.Collections.Generic;

public class GravityObject : MonoBehaviour
{
    public float mass = 1f; // The object's mass
    public Vector3 velocity; // Initial velocity
    public bool isStatic = false; // If true, the object won't move

    private static List<GravityObject> allGravityObjects = new List<GravityObject>();
    private const float G = 0.1f; // Gravitational constant

    void Start()
    {
        allGravityObjects.Add(this);
    }

    void OnDestroy()
    {
        allGravityObjects.Remove(this);
    }

    void FixedUpdate()
    {
        if (!isStatic) // Only apply movement if the object is NOT static
        {
            ApplyGravity();
            MoveObject();
        }
    }

    void ApplyGravity()
    {
        Vector3 acceleration = Vector3.zero;

        foreach (GravityObject other in allGravityObjects)
        {
            if (other == this) continue; // Skip self
            if (other.isStatic && this.isStatic) continue; // Skip if both are static

            Vector3 direction = other.transform.position - transform.position;
            float distance = direction.magnitude;
            if (distance < 0.1f) continue; // Avoid extreme forces when objects are too close

            float forceMagnitude = G * (mass * other.mass) / (distance * distance);
            Vector3 force = direction.normalized * forceMagnitude;

            acceleration += force / mass; // F = ma ? a = F/m
        }

        velocity += acceleration * Time.fixedDeltaTime; // Update velocity
    }

    void MoveObject()
    {
        transform.position += velocity * Time.fixedDeltaTime; // Apply movement
    }
}
