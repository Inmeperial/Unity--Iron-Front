using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MechaPartButton : CustomButton
{
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Slider _currentHPSlider;
    [SerializeField] private Slider _damagePreviewSlider;


    [SerializeField] private TextMeshProUGUI _bulletsCountText;
    private int _bulletsCount;
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        _damagePreviewSlider.gameObject.SetActive(true);
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        _damagePreviewSlider.gameObject.SetActive(false);
    }

    public void SetSlider(float minValue, float maxValue)
    {
        _currentHPSlider.minValue = minValue;
        _currentHPSlider.maxValue = maxValue;
        
        _damagePreviewSlider.minValue = minValue;
        _damagePreviewSlider.maxValue = maxValue;
    }

    public void UpdateHpSlider(float value)
    {
        _currentHPSlider.value = value;
    }

    public void UpdateDamagePreviewSlider(float value)
    {
        _damagePreviewSlider.value = value;
    }

    public void SetHpText(string text)
    {
        _hpText.text = text;
    }

    public void SetBulletsCount(int value)
    {
        _bulletsCountText.text = value.ToString();
        _bulletsCount = value;
    }
    
}
