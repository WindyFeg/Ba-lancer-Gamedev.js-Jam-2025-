using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }
    public GameObject[] mapPrefabs; // Array of map prefabs to choose from}

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnMap()
    {
        // Spawn map
        int randomIndex = Random.Range(0, mapPrefabs.Length);
        GameObject selectedMap = mapPrefabs[randomIndex];

        // Deactivate all maps first
        foreach (GameObject map in mapPrefabs)
        {
            map.SetActive(false);
        }

        Debug.Log("Selected Map: " + selectedMap.name);
        selectedMap.SetActive(true);

        // Spawn Player at the center of the map
        selectedMap.GetComponent<SpawnPlayerAtHelper>().SpawnPlayerAt();

        // Spawn enemy at random position
    }

    public void NextMap()
    {
        SpawnMap();
    }

    public void Update()
    {
        // Update logic for the map manager can be added here if needed
        if (Input.GetKeyDown(KeyCode.M))
        {
            NextMap();
        }
    }
}