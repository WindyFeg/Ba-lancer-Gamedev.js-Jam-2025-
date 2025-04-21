using System;
using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        aiData = GetComponent<AIData>();
        enemyAI = GetComponent<EnemyAI>();
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
        float attackDuration = playerSpine.get_duration(playerSpine.attack_anim);
        Invoke("Attack", attackDuration);
    }

    public void PerformRangeAttack()
    {
        playerSpine.attack_start();

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position + new Vector3(0, 2f), Quaternion.identity);
        Projectile proj = projectile.GetComponent<Projectile>();
        proj.Launch(aiData.currentTarget);
    }

    public void Attack()
    {
        if (aiData.currentTarget == null) return;

        float distance = Vector3.Distance(transform.position, aiData.currentTarget.transform.position);
        if (distance <= enemyAI.attackDistance)
        {
            aiData.currentTarget.GetComponent<Player>().TakeDamage(10);
        }
        else
        {
            Debug.Log("Target out of range, attack missed.");
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