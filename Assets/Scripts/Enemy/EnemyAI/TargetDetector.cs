using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField] private float targetDetectionRange = 5;

    [SerializeField] private LayerMask obstaclesLayerMask, playerLayerMask;

    [SerializeField] private bool showGizmos = false;

    //gizmo parameters
    private List<Transform> colliders;

    public override void Detect(AIData aiData)
    {
        // Find player in range
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, targetDetectionRange, playerLayerMask);

        if (playerColliders.Length > 0)
        {
            Debug.Log("Player detected");

            Transform playerTransform = playerColliders[0].transform;
            Vector3 direction = (playerTransform.position - transform.position).normalized;

            // Raycast to check visibility (line of sight)
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, targetDetectionRange, obstaclesLayerMask | playerLayerMask))
            {
                if (((1 << hit.collider.gameObject.layer) & playerLayerMask) != 0)
                {
                    Debug.DrawRay(transform.position, direction * targetDetectionRange, Color.magenta);
                    colliders = new List<Transform>() { playerTransform };
                }
                else
                {
                    colliders = null;
                }
            }
            else
            {
                colliders = null;
            }
        }
        else
        {
            colliders = null;
        }

        aiData.targets = colliders;
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        if (colliders == null)
            return;

        Gizmos.color = Color.magenta;
        foreach (var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }
}
