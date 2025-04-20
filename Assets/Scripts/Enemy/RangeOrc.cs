using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class RangeOrg : IMonster
{
    public float speed;
    public float lineOfSight;
    public float shootingRange;
    public float fireRate;
    private float nextFireTime;
    public GameObject bullet;
    private Transform playerPos;

    void Start()
    {
        playerPos = Player.instance.transform; // FIND THE PLAYER
    }

    public override void OnIdle()
    {
        // Idle logic
    }

    public override void OnAttack()
    {
        // Attack logic
    }

    public override void OnTrigger()
    {
        // Trigger logic
    }

    public override void OnDead()
    {
        // Death logic
    }

    public override void Update()
    {
        float distanceFromPlayer = Vector3.Distance(playerPos.position, transform.position);

        // Move towards player if in line of sight but not too close
        if (distanceFromPlayer < lineOfSight && distanceFromPlayer > shootingRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
        }
        else if (distanceFromPlayer <= shootingRange && nextFireTime < Time.time)
        {
            Instantiate(bullet, transform.position + transform.forward, transform.rotation);
            nextFireTime = Time.time + fireRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
