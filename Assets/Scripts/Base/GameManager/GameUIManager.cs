using System;
using System.Collections;
using Base;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }
    public GameObject statUI;
    public GameObject enemyUI;

    [Header("Enemy Sliders")]
    [SerializeField] private Slider attackSlider;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider defSlider;
    [SerializeField] private Slider atkSpeedSlider;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider rangeSlider;

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

    private void ToggleEnemyUI(PlayerBehaviour enemyUIController)
    {
        CanvasGroup canvasGroup = enemyUI.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = enemyUI.AddComponent<CanvasGroup>();
        }

        if (enemyUI.activeSelf)
        {
            UpdateEnemyStats(enemyUIController);
            enemyUI.transform.DOMoveX(-500, 0.2f).SetEase(Ease.OutCubic);
            canvasGroup.DOFade(0, 1f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                enemyUI.SetActive(false);
            });
            CameraManager.Instance.ZoomOutFromPlayer();
            EndSlowMotion();
        }
        else
        {
            UpdateEnemyUI(enemyUIController);
            enemyUI.SetActive(true);
            canvasGroup.alpha = 0;
            enemyUI.transform.DOMoveX(900, 0.2f).SetEase(Ease.OutCubic);
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

    public void OnEnemyUIClicked(PlayerBehaviour enemyUIController)
    {

        ToggleEnemyUI(enemyUIController);
    }

    private void UpdateEnemyUI(PlayerBehaviour enemyUIController){
        attackSlider.value = enemyUIController.AttackDamage;
        hpSlider.value = enemyUIController.CurrentHealth;
        defSlider.value = enemyUIController.Armor;
        atkSpeedSlider.value = enemyUIController.AttackSpeed;
        speedSlider.value = enemyUIController.Speed;
        rangeSlider.value = enemyUIController.Range;

    }

    private void UpdateEnemyStats(PlayerBehaviour enemyUIController)
    {
        enemyUIController.AttackDamage = attackSlider.value;
        enemyUIController.CurrentHealth = hpSlider.value;
        enemyUIController.Armor = defSlider.value;
        enemyUIController.AttackSpeed = atkSpeedSlider.value;
        enemyUIController.Speed = speedSlider.value;
        enemyUIController.Range = rangeSlider.value;
    }


}