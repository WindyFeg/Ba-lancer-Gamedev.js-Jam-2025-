using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public float launchAngle = 45f;
    public float damage = 10f;
    public float impactRadius = 4f;

    private Vector3 startPoint;
    private Vector3 targetPoint;
    private float gravity;
    private float launchVelocity;
    private float flightDuration;
    private float elapsedTime = 0f;
    private bool launched = false;

    [SerializeField] private GameObject explosionParticles;

    public void Launch(Transform targetTransform)
    {
        target = targetTransform;
        startPoint = transform.position;
        targetPoint = target.position;
        gravity = Mathf.Abs(Physics.gravity.y);

        Vector3 toTarget = targetPoint - startPoint;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
        float distance = toTargetXZ.magnitude;
        float heightDifference = toTarget.y;

        float angleRad = launchAngle * Mathf.Deg2Rad;

        float v2 = gravity * distance * distance / (2 * (distance * Mathf.Tan(angleRad) - heightDifference) * Mathf.Pow(Mathf.Cos(angleRad), 2));
        launchVelocity = Mathf.Sqrt(Mathf.Max(0, v2));

        flightDuration = distance / (launchVelocity * Mathf.Cos(angleRad));

        launched = true;
    }

    private void Update()
    {
        if (!launched) return;

        elapsedTime += Time.deltaTime;
        float t = elapsedTime;

        if (t > flightDuration)
        {
            OnHitGround();
            return;
        }

        float xzDistance = launchVelocity * Mathf.Cos(launchAngle * Mathf.Deg2Rad) * t;
        float yOffset = launchVelocity * Mathf.Sin(launchAngle * Mathf.Deg2Rad) * t - 0.5f * gravity * t * t;

        Vector3 dir = (targetPoint - startPoint);
        dir.y = 0f;
        dir.Normalize();

        Vector3 currentPosition = startPoint + dir * xzDistance;
        currentPosition.y = startPoint.y + yOffset;

        transform.position = currentPosition;
    }

private void OnHitGround()
{

    Vector3 landedPosition = transform.position;
    Vector3 playerXZ = new Vector3(Player.instance.transform.position.x, 0f, Player.instance.transform.position.z);
    Vector3 impactXZ = new Vector3(landedPosition.x, 0f, landedPosition.z);

    float distanceXZ = Vector3.Distance(playerXZ, impactXZ);

    if (distanceXZ <= impactRadius)
    {
        Player.instance.TakeDamage(damage);
        Destroy(gameObject);
        return;
    }
    Instantiate(explosionParticles, new Vector3(landedPosition.x, 0f, landedPosition.z), Quaternion.identity);
    Destroy(gameObject);
}
}
