using System;
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

    public async void ChangeState(GameBaseState nextState)
    {
        CurrentState?.ExitState(this);

        PreviousState = CurrentState;
        CurrentState = nextState;
        await SwitchScene(nextState);
        CurrentState.EnterState(this);
    }

    private async System.Threading.Tasks.Task SwitchScene(GameBaseState nextState)
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
            await System.Threading.Tasks.Task.Yield();
        }
    }
}