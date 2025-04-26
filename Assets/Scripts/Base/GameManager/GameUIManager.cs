using System;
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

    private void Update()
    {
        // Update logic for the UI manager can be added here if needed
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("UI Manager Key Pressed");
            ToggleStatUI();
        }
    }

    private void ToggleStatUI()
    {
        if (statUI.activeSelf)
        {
            // Use a combination of DoMove and DoFade to make it disappear more fancy
            CanvasGroup canvasGroup = statUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = statUI.AddComponent<CanvasGroup>();
            }

            statUI.transform.DOMoveX(-500, 0.5f).SetEase(Ease.OutCubic);
            canvasGroup.DOFade(0, 1f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                statUI.SetActive(false);
            });
        }
        else
        {
            // Use a combination of DoMove and DoFade to make it appear more fancy
            CanvasGroup canvasGroup = statUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = statUI.AddComponent<CanvasGroup>();
            }

            statUI.SetActive(true);
            canvasGroup.alpha = 0;
            statUI.transform.DOMoveX(300, 0.5f).SetEase(Ease.OutCubic);
            canvasGroup.DOFade(1, 1f).SetEase(Ease.OutCubic);
        }
    }

    public void OnPlayerUIClicked(PlayerUIController playerUIController)
    {
        ToggleStatUI();
    }
}