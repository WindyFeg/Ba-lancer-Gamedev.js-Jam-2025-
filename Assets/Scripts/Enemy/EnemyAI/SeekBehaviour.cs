using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField]
    private float targetRechedThreshold = 0.5f;

    [SerializeField]
    private bool showGizmo = true;

    bool reachedLastTarget = true;

    // gizmo parameters
    private Vector3 targetPositionCached;
    private float[] interestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        if (reachedLastTarget)
        {
            if (aiData.targets == null || aiData.targets.Count <= 0)
            {
                aiData.currentTarget = null;
                return (danger, interest);
            }
            else
            {
                reachedLastTarget = false;
                aiData.currentTarget = aiData.targets
                    .OrderBy(target => Vector3.Distance(target.position, transform.position))
                    .FirstOrDefault();
            }
        }

        if (aiData.currentTarget != null && aiData.targets != null && aiData.targets.Contains(aiData.currentTarget))
        {
            targetPositionCached = aiData.currentTarget.position;
        }

        if (Vector3.Distance(transform.position, targetPositionCached) < targetRechedThreshold)
        {
            reachedLastTarget = true;
            aiData.currentTarget = null;
            return (danger, interest);
        }

        Vector3 directionToTarget = (targetPositionCached - transform.position);
        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector3.Dot(directionToTarget.normalized, Directions.eightDirections3D[i]);

            if (result > 0)
            {
                float valueToPutIn = result;
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }
            }
        }

        interestsTemp = interest;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmo)
            return;

        Gizmos.DrawSphere(targetPositionCached, 0.2f);

        if (Application.isPlaying && interestsTemp != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < interestsTemp.Length; i++)
            {
                Gizmos.DrawRay(transform.position, Directions.eightDirections3D[i] * interestsTemp[i] * 2);
            }

            if (!reachedLastTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(targetPositionCached, 0.1f);
            }
        }
    }
}
