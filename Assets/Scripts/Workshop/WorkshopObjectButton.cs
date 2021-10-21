using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class WorkshopObjectButton : CustomButton
{
    [SerializeField] private TextMeshProUGUI _objectName;
    [SerializeField] private Image _objectImage;
    private TextMeshProUGUI _description;

    public void SetObjectName(string name)
    {
        _objectName.text = name;
    }

    public void SetObjectSprite(Sprite sprite)
    {
        //TODO: sacar cuando haya sprite
        if (!sprite) return;
        _objectImage.sprite = sprite;
    }
    
    public void SetDescriptionTextField(TextMeshProUGUI textField)
    {
        _description = textField;
    }

    public void SetLeftClick(UnityAction action)
    {
        OnLeftClick.RemoveAllListeners();
        OnLeftClick.AddListener(action);
    }
}
