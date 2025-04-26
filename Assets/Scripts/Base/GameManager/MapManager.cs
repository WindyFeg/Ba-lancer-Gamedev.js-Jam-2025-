using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }
    public GameObject[] mapPrefabs;
    public GameObject[] enemyPrefabs;

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
        NextMap();
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
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        // Get every GameObject with the tag "Enemy" in the scene and destroy them
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject ene in enemies)
        {
            Destroy(ene);
        }

        // Spawn a random number of enemies (1 to 3)
        int enemyCount = Random.Range(1, 4); // Random.Range(1, 4) generates 1, 2, or 3
        for (int i = 0; i < enemyCount; i++)
        {
            // Spawn a random enemy at a random position
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject selectedEnemy = enemyPrefabs[randomIndex];
            Vector3 randomPosition = new Vector3(Random.Range(-20, 10), 0, Random.Range(-10, 15));
            Instantiate(selectedEnemy, randomPosition, Quaternion.identity);
        }
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