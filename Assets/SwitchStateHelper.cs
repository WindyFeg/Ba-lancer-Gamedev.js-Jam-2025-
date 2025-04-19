using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStateHelper : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        CharmShop,
        GamePlay
    }
    public GameState currentState = GameState.MainMenu;

    public void SwitchState()
    {
        GameManager.Instance.ChangeState(currentState switch
        {
            GameState.MainMenu => new MainMenuState(),
            GameState.CharmShop => new CharmShopState(),
            GameState.GamePlay => new GamePlayState(),
            _ => throw new System.ArgumentOutOfRangeException()
        });
    }
}