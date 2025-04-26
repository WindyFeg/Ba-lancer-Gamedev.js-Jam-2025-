using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }
    public GameObject statUI;

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

    private void ToggleStatUI()
    {
        CanvasGroup canvasGroup = statUI.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = statUI.AddComponent<CanvasGroup>();
        }

        if (statUI.activeSelf)
        {
            statUI.transform.DOMoveX(-500, 0.2f).SetEase(Ease.OutCubic);
            canvasGroup.DOFade(0, 1f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                statUI.SetActive(false);
            });
            CameraManager.Instance.ZoomOutFromPlayer();
            EndSlowMotion();
        }
        else
        {
            statUI.SetActive(true);
            canvasGroup.alpha = 0;
            statUI.transform.DOMoveX(300, 0.2f).SetEase(Ease.OutCubic);
            canvasGroup.DOFade(1, 1f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
            });
            CameraManager.Instance.ZoomIntoPlayer();
            StartSlowMotion();
        }
    }

    public float slowMotionScale = 0.2f;

    public void StartSlowMotion()
    {
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust fixedDeltaTime for physics
    }

    public void EndSlowMotion()
    {
        Time.timeScale = 1f; // Directly reset time scale to normal
        Time.fixedDeltaTime = 0.02f; // Reset fixedDeltaTime to its default value
    }
    public void OnPlayerUIClicked(PlayerUIController playerUIController)
    {
        ToggleStatUI();
    }
}