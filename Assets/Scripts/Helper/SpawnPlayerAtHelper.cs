using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayerAtHelper : MonoBehaviour
{
    public Transform spawnPosition;
    public void SpawnPlayerAt()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {

            Vector3 adjustedPosition = spawnPosition.position;
            adjustedPosition.y += 1.0f; // Adjust the Y position to avoid clipping into the ground
            player.transform.position = adjustedPosition;

            Debug.Log("Player spawned at: " + spawnPosition);
        }
        else
        {
            Debug.LogError("Player not found in the scene.");
        }
    }
}