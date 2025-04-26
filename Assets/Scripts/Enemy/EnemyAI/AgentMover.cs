using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AgentMover : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float acceleration = 50f, deacceleration = 100f;

    [SerializeField]
    private float currentSpeed = 0f;

    private Vector3 oldMovementInput;
    public Vector3 MovementInput { get; set; }
    private ModelSpine modelSpine; // link to model spine
    [SerializeField] private PlayerBehaviour playerBehaviour; // link to player behaviour

    private void Awake()
    {
        modelSpine = GetComponentInChildren<ModelSpine>();
        rb = GetComponent<Rigidbody>();
    
    }
    void Start()
    {
        Debug.Log("Check Speed: "+playerBehaviour);

    }

    private void FixedUpdate()
    {
        // Vector3 flatInput = new Vector3(MovementInput.x, 0f, MovementInput.z);
        // Debug.Log("Flat Input: " + flatInput);
        // if (flatInput.magnitude > 0 && currentSpeed >= 0)
        // {
        //     oldMovementInput = flatInput.normalized;
        //     currentSpeed += acceleration * playerBehaviour.Speed * Time.deltaTime;
        // }
        // else
        // {
        //     currentSpeed -= deacceleration * playerBehaviour.Speed * Time.deltaTime;
        // }

        // currentSpeed = Mathf.Clamp(currentSpeed, 0, playerBehaviour.Speed);

        // Vector3 velocity = oldMovementInput * currentSpeed;
        // rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        // // Animation logic
        // PlayAnimationByDirection(flatInput);
    }
    public void Move(Vector3 flatInput)
    {
         if (flatInput.magnitude > 0 && currentSpeed >= 0)
        {
            oldMovementInput = flatInput.normalized;
            currentSpeed += acceleration * playerBehaviour.Speed * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deacceleration * playerBehaviour.Speed * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, playerBehaviour.Speed);

        Vector3 velocity = oldMovementInput * currentSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        PlayAnimationByDirection(flatInput);

    }

    private void PlayAnimationByDirection(Vector3 direction)
    {
        if (modelSpine == null) return;

        if (direction.magnitude <= 0.01f)
        {
            // Idle - use last direction to determine which idle anim to play
            if (Mathf.Abs(oldMovementInput.x) > Mathf.Abs(oldMovementInput.z))
            {
                modelSpine.side_idle_playing();
            }
            else if (oldMovementInput.z > 0)
            {
                modelSpine.up_idle_playing();
            }
            else
            {
                modelSpine.down_idle_playing();
            }
        }
        else
        {
            // Movement animations
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                modelSpine.move_side_playing();
                modelSpine.direction((int)Mathf.Sign(direction.x));
            }
            else if (direction.z > 0)
            {
                modelSpine.move_up_playing();
                modelSpine.direction(1); // facing forward
            }
            else
            {
                modelSpine.move_down_playing();
                modelSpine.direction(1); // facing forward
            }
        }
    }
}
