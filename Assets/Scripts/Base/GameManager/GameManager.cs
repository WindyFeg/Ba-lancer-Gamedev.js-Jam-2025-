using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameBaseState CurrentState { get; private set; }
    public GameBaseState PreviousState { get; private set; }

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

    private void Start()
    {
        // Initialize the game state to the main menu or any other initial state
        ChangeState(new MainMenuState());
    }

    public void ChangeState(GameBaseState nextState)
    {
        CurrentState?.ExitState(this);

        PreviousState = CurrentState;
        CurrentState = nextState;
        StartCoroutine(SwitchScene(nextState));
    }

    public MapManager GetMapManager()
    {
        if (MapManager.Instance == null)
        {
            Debug.LogError("MapManager instance is null. Make sure it is initialized before calling this method.");
        }
        return MapManager.Instance;
    }

    private IEnumerator SwitchScene(GameBaseState nextState)
    {
        string sceneName = nextState.StateName switch
        {
            "MainMenuState" => "Start",
            "CharmShopState" => "CharmShop",
            "GamePlayState" => "GamePlay",
            _ => throw new ArgumentException("Invalid state name")
        };

        var sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!sceneLoadOperation.isDone)
        {
            yield return null;
        }

        CurrentState.EnterState(this);
    }

    public void CheckNextLevel()
    {
        // if there is tag enemy return
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 1)
        {
            Debug.Log("Cannot proceed to the next level. Enemies are still present.");
            return; // Exit the method if enemies are present
        }

        Debug.Log("NEXT MAP");
        MapManager.Instance.NextMap();
      
    }
}