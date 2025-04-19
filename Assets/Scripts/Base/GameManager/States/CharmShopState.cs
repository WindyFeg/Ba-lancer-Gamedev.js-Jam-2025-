using System;
using UnityEngine;

class CharmShopState : GameBaseState
{
    public override string StateName { get; set; } = "CharmShopState";

    public override void EnterState(GameManager state)
    {
        Debug.Log("Entering Charm Shop State");
        // GameUIManager.Instance.ShowCharmShopUI();
    }

    public override void ExitState(GameManager state)
    {
        Debug.Log("Exiting Charm Shop State");
        // GameUIManager.Instance.HideCharmShopUI();
    }
}