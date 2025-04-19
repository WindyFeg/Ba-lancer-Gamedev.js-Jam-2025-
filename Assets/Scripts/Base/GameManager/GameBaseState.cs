using System;
using UnityEngine;

[System.Serializable]
public abstract class GameBaseState
{
    public abstract string StateName { set; get; }
    public abstract void EnterState(GameManager state);
    public abstract void ExitState(GameManager state);
}