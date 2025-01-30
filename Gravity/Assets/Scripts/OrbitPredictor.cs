using UnityEngine;
using System.Collections.Generic;

public class OrbitPredictor : MonoBehaviour
{
    public int predictionSteps = 300; // How far into the future we predict
    public float timeStep = 0.02f; // How fast each prediction step is simulated
    public LineRenderer lineRenderer;

    private GravityObject gravityObject;

    void Start()
    {
        gravityObject = GetComponent<GravityObject>();
        lineRenderer.positionCount = predictionSteps;
    }

    void Update()
    {
        PredictOrbit();
    }

    void PredictOrbit()
    {
        Vector3 simulatedPosition = transform.position;
        Vector3 simulatedVelocity = gravityObject.velocity;

        Vector3[] positions = new Vector3[predictionSteps];

        for (int i = 0; i < predictionSteps; i++)
        {
            simulatedVelocity += CalculateGravity(simulatedPosition) * timeStep;
            simulatedPosition += simulatedVelocity * timeStep;
            positions[i] = simulatedPosition;
        }

        lineRenderer.SetPositions(positions);
    }

    Vector3 CalculateGravity(Vector3 position)
    {
        Vector3 acceleration = Vector3.zero;

        foreach (GravityObject other in FindObjectsOfType<GravityObject>())
        {
            if (other == gravityObject) continue;

            Vector3 direction = other.transform.position - position;
            float distance = direction.magnitude;
            if (distance < 0.1f) continue; // Avoid extreme forces

            float forceMagnitude = 0.1f * (gravityObject.mass * other.mass) / (distance * distance);
            acceleration += direction.normalized * forceMagnitude / gravityObject.mass;
        }

        return acceleration;
    }
}
