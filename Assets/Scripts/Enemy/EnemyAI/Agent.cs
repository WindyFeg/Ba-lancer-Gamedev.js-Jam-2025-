using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private AgentMover agentMover;


    private Vector3 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector3 MovementInput { get => movementInput; set => movementInput = value; }
    private ModelSpine playerSpine;
    void Start()
    {
        playerSpine = GetComponent<ModelSpine>();
    }
    private void Update()
    {
        //pointerInput = GetPointerInput();
        //movementInput = movement.action.ReadValue<Vector2>().normalized;

        agentMover.MovementInput = MovementInput;
        AnimateCharacter();
    }

    public void PerformAttack()
    {
        playerSpine.attack_start();
    }

    private void Awake()
    {
        agentMover = GetComponent<AgentMover>();
    }

    private void AnimateCharacter()
    {
        // Vector2 lookDirection = pointerInput - (Vector2)transform.position;
        // agentAnimations.RotateToPointer(lookDirection);
        // agentAnimations.PlayAnimation(MovementInput);
    }

    

}