using System;
using UnityEngine;

class GamePlayState : GameBaseState
{
    public override string StateName { get; set; } = "GamePlayState";

    public override void EnterState(GameManager state)
    {
        Debug.Log("Entering Game Play State");
        // GameUIManager.Instance.ShowCharmShopUI();
    }

    public override void ExitState(GameManager state)
    {
        Debug.Log("Exiting Game Play State");
        // GameUIManager.Instance.HideCharmShopUI();
    }
}