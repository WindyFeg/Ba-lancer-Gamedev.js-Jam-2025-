using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stats
{
    public class StatItem : MonoBehaviour
    {
        [SerializeField] private string statName;
        
        // Fake values for demonstration
        // Remove later
        [SerializeField] private int minValue;
        [SerializeField] private int maxValue;
        [SerializeField] private int currentValue;
        
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI statNameText;
        [SerializeField] private TextMeshProUGUI minValueText;
        [SerializeField] private TextMeshProUGUI curValueText;
        [SerializeField] private TextMeshProUGUI maxValueText;
        [SerializeField] private Slider statSlider;

        private int _minValue;
        private int _maxValue;
        private int _currentValue;
        private void Start()
        {
            // Get data from GameConfig to replace the fake values above
            _minValue = minValue;
            _maxValue = maxValue;
            _currentValue = currentValue;
            if (statSlider != null)
            {
                statSlider.onValueChanged.AddListener(OnSliderValueChanged);
                OnSliderValueChanged(statSlider.value);
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            statNameText.text = statName;
            minValueText.text = _minValue.ToString();
            maxValueText.text = _maxValue.ToString();
            curValueText.text = _currentValue.ToString();
            
            statSlider.minValue = _minValue;
            statSlider.maxValue = _maxValue;
            statSlider.value = _currentValue;
        }
        
        void OnSliderValueChanged(float value)
        {
            curValueText.text = _currentValue.ToString();
            // Call other function to store or calculate to BALANCE the stats
        }

    }
}