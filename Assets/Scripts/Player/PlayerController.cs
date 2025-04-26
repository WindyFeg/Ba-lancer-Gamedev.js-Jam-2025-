using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Base;
using UnityEngine;
using UnityEngine.UI;
namespace Game
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour playerBehaviour;
        // Start is called before the first frame update
        private ModelSpine playerSpine;
        private Rigidbody rb;
        private float xdir, zdir;

        private bool isMoving = false;
        private float _moveSpeedMultiplier = 2f;


        private void Awake()
        {

        }
        private void Start()
        {
            playerSpine = GetComponent<ModelSpine>();
            rb = gameObject.GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {

            xdir = Input.GetAxis("Horizontal");
            zdir = Input.GetAxis("Vertical");
            Walking();

        }

        void Walking()
        {
            if (Time.timeScale == 0) return;

            Vector3 movement = new Vector3(xdir, 0f, zdir).normalized;
            Vector3 newPosition = rb.position + movement * (playerBehaviour.Speed * _moveSpeedMultiplier * Time.deltaTime);
            rb.MovePosition(newPosition);

            if (movement.magnitude > 0)
            {
                if (Mathf.Abs(xdir) > Mathf.Abs(zdir))
                {
                    playerSpine.move_side_playing();
                    playerSpine.direction(xdir > 0 ? 1 : -1);
                }
                else if (zdir > 0)
                {
                    playerSpine.move_up_playing();
                }
                else
                {
                    playerSpine.move_down_playing();
                }

                isMoving = true;
            }
            else if (isMoving)
            {
                if (Mathf.Abs(xdir) > Mathf.Abs(zdir))
                {
                    playerSpine.side_idle_playing();
                }
                else if (zdir > 0)
                {
                    playerSpine.up_idle_playing();
                }
                else
                {
                    playerSpine.down_idle_playing();
                }

                isMoving = false;
            }
        }


    }
}