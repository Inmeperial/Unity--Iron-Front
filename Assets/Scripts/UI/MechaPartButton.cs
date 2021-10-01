using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private MaterialMechaHandler _materialHandler;
    private MechaParts _part;
    public override void OnPointerEnter(PointerEventData eventData)
    {

        switch (_part)
        {
            case MechaParts.Body:
                Debug.Log("mouse over body");
                break;
            case MechaParts.Legs:
                Debug.Log("mouse over legs");
                break;
            case MechaParts.RArm:
                Debug.Log("mouse over right arm");
                break;
            case MechaParts.LArm:
                Debug.Log("mouse over left arm");
                break;
        }
        
        UpdateDamagePreviewSlider();
        
        _damagePreviewSlider.gameObject.SetActive(true);
    }
    
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (_bulletsCount <= 0)
        {
            _damagePreviewSlider.gameObject.SetActive(false);
            
            switch (_part)
            {
                case MechaParts.Body:
                    Debug.Log("mouse exits body");
                    break;
                case MechaParts.Legs:
                    Debug.Log("mouse exits legs");
                    break;
                case MechaParts.RArm:
                    Debug.Log("mouse exits right arm");
                    break;
                case MechaParts.LArm:
                    Debug.Log("mouse exits left arm");
                    break;
            }
        }
            
    }

    protected override void PressRight()
    {
        base.PressRight();
        UpdateDamagePreviewSlider();
    }

    protected override void PressLeft()
    {
        base.PressLeft();
        UpdateDamagePreviewSlider();
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

    public void UpdateDamagePreviewSlider()
    {
        Character selectedCharacter = CharacterSelection.Instance.GetSelectedCharacter();

        Gun gun = selectedCharacter.GetSelectedGun();

        float estimatedDamage = gun.GetBulletDamage() * _bulletsCount;

        _damagePreviewSlider.value = _currentHPSlider.value - estimatedDamage;
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

    public void SetPart(MechaParts part)
    {
        _part = part;
    }
}
