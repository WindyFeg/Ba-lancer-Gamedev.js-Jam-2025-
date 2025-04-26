using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 1.5f;

    [SerializeField]
    public float attackDistance = 0.5f;

    // Inputs sent from the Enemy AI to the Enemy controller
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector3> OnMovementInput, OnPointerInput;

    [SerializeField]
    private Vector3 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    bool following = false;

    private void Start()
    {
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }

    private void Update()
    {
        if (aiData.currentTarget != null)
        {
            OnPointerInput?.Invoke(aiData.currentTarget.position);

            if (!following)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            aiData.currentTarget = aiData.targets[0];
        }

        OnMovementInput?.Invoke(movementInput);
    }

    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            Debug.Log("Stopping");
            movementInput = Vector3.zero;
            following = false;
            yield break;
        }

        float distance = Vector3.Distance(new Vector3(aiData.currentTarget.position.x, 0, aiData.currentTarget.position.z),
                                          new Vector3(transform.position.x, 0, transform.position.z));

        if (distance < attackDistance)
        {
            movementInput = Vector3.zero;
            OnAttackPressed?.Invoke();
            yield return new WaitForSeconds(attackDelay);
            StartCoroutine(ChaseAndAttack());
        }
        else
        {
            movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
            yield return new WaitForSeconds(aiUpdateDelay);
            StartCoroutine(ChaseAndAttack());
        }
    }
}
