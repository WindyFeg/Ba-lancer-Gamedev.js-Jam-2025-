using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    public float maxX = 0, minX = 0, maxY = 0, minY = 0;
    public float moveSpeed;
    public bool isFollowPlayer;

    static public GameObject targetObj;
    Vector3 target;
    // Start is called before the first frame update
    void Awake()
    {

    }

    private void Start()
    {
        isFollowPlayer = true;
        // target = obj.transform.position;
        targetObj = player.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null /*&& isFollowPlayer*//*&& isScoll == false*/)
        {
            target.x = targetObj.transform.position.x;
            target.y = 10f;
            target.z = targetObj.transform.position.z - 5f;

            if (target.x < minX) target.x = minX;
            if (target.x > maxX) target.x = maxX;
            if (target.y < minY) target.y = minY;
            if (target.y > maxY) target.y = maxY;

            this.transform.position = Vector3.Lerp(this.transform.position, target, moveSpeed);
        }

    }

    public Vector2 CameraPos
    {
        get { return transform.position; }
        set { 
                transform.position = value;
            transform.position -= new Vector3(0, 0, 10);
            }
    }
}