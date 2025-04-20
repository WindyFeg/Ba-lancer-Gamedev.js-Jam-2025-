using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBehaviour : SteeringBehaviour
{
    [SerializeField]
    private float radius = 2f, agentColliderSize = 0.6f;

    [SerializeField]
    private bool showGizmo = true;

    float[] dangersResultTemp = null;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        foreach (Collider obstacleCollider in aiData.obstacles)
        {
            Vector3 directionToObstacle = obstacleCollider.ClosestPoint(transform.position) - transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;

            float weight = distanceToObstacle <= agentColliderSize
                ? 1f
                : (radius - distanceToObstacle) / radius;

            Vector3 directionToObstacleNormalized = directionToObstacle.normalized;

            for (int i = 0; i < Directions.eightDirections3D.Count; i++)
            {
                float result = Vector3.Dot(directionToObstacleNormalized, Directions.eightDirections3D[i]);

                float valueToPutIn = result * weight;

                if (valueToPutIn > danger[i])
                {
                    danger[i] = valueToPutIn;
                }
            }
        }

        dangersResultTemp = danger;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo || dangersResultTemp == null)
            return;

        Gizmos.color = Color.red;
        for (int i = 0; i < dangersResultTemp.Length; i++)
        {
            Gizmos.DrawRay(
                transform.position,
                Directions.eightDirections3D[i] * dangersResultTemp[i] * 2
            );
        }
    }
}
public static class Directions
{
    public static List<Vector3> eightDirections3D = new List<Vector3>{
        new Vector3(0, 0, 1).normalized,
        new Vector3(1, 0, 1).normalized,
        new Vector3(1, 0, 0).normalized,
        new Vector3(1, 0, -1).normalized,
        new Vector3(0, 0, -1).normalized,
        new Vector3(-1, 0, -1).normalized,
        new Vector3(-1, 0, 0).normalized,
        new Vector3(-1, 0, 1).normalized
    };
}
