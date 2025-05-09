using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using Game;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private AgentMover agentMover;


    private Vector3 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector3 MovementInput { get => movementInput; set => movementInput = value; }
    private ModelSpine playerSpine;
    private AIData aiData;
    private EnemyAI enemyAI;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    private PlayerBehaviour playerBehaviour;
    void Start()
    {
        aiData = GetComponent<AIData>();
        enemyAI = GetComponent<EnemyAI>();
        playerSpine = GetComponent<ModelSpine>();
        playerBehaviour = GetComponent<PlayerBehaviour>();
    }
    private void Update()
    {
        //pointerInput = GetPointerInput();
        //movementInput = movement.action.ReadValue<Vector2>().normalized;

        // // agentMover.MovementInput = MovementInput;
        // agentMover.Move(MovementInput);
        // Debug.Log("Movement Input: " +  MovementInput);
        AnimateCharacter();
    }

    public void PerformAttack()
    {
        playerSpine.attack_start();
        float attackDuration = playerSpine.get_duration(playerSpine.attack_anim);
        StartCoroutine(Attack(attackDuration));
    }

    public void PerformRangeAttack()
    {
        playerSpine.attack_start();

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position + new Vector3(0, 2f), Quaternion.identity);
        Projectile proj = projectile.GetComponent<Projectile>();
        proj.Launch(aiData.currentTarget);
    }

    IEnumerator Attack(float attackDuration)
    {
        yield return new WaitForSeconds(attackDuration + 0.3f); // Delay the attack to match the animation duration

        if (aiData.currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, aiData.currentTarget.transform.position);
            if (distance <= playerBehaviour.Range)
            {
                aiData.currentTarget.GetComponent<PlayerBehaviour>().TakeDamage(playerBehaviour.AttackDamage);
            }
            else
            {
                Debug.Log("Target out of range, attack missed.");
            }
        }
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