using System;
using UnityEngine;

class GamePlayState : GameBaseState
{
    public override string StateName { get; set; } = "GamePlayState";

    public override void EnterState(GameManager state)
    {
        Debug.Log("Entering Game Play State");
        var mapManager = state.GetMapManager();
        if (mapManager != null)
        {
            mapManager.SpawnMap();
        }
        else
        {
            Debug.LogError("MapManager instance is null. Make sure it is initialized before calling this method.");
        }
    }

    public override void ExitState(GameManager state)
    {
        Debug.Log("Exiting Game Play State");
        // GameUIManager.Instance.HideCharmShopUI();
    }
}