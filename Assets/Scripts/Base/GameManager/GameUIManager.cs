    using System;
    using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }
    public GameObject statUI;

    private void Awake()
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

    private void Update()
    {
        // Update logic for the UI manager can be added here if needed
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("UI Manager Key Pressed");
            ToggleStatUI();
        }
    }
    
    private void ToggleStatUI()
    {
        if (statUI.activeSelf)
        {
            statUI.SetActive(false);
        }
        else
        {
            statUI.SetActive(true);
        }
    }
}