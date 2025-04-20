using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AgentMover : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float maxSpeed = 2f, acceleration = 50f, deacceleration = 100f;

    [SerializeField]
    private float currentSpeed = 0f;

    private Vector3 oldMovementInput;
    public Vector3 MovementInput { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Only XZ plane movement
        Vector3 flatInput = new Vector3(MovementInput.x, 0f, MovementInput.z);


        if (flatInput.magnitude > 0 && currentSpeed >= 0)
        {
            oldMovementInput = flatInput.normalized;
            currentSpeed += acceleration * maxSpeed * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deacceleration * maxSpeed * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

        Vector3 velocity = oldMovementInput * currentSpeed;

        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z); // maintain Y for gravity
    }
}
