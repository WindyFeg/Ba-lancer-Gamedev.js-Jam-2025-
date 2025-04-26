using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private CinemachineVirtualCamera playerVirtualCamera;
    public void ZoomIntoPlayer()
    {
        playerVirtualCamera.m_Lens.FieldOfView = 15;
    }

    public void ZoomOutFromPlayer()
    {
        playerVirtualCamera.m_Lens.FieldOfView = 40;
    }
}
