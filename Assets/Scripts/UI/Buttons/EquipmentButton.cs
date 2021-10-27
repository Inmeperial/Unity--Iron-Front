using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EquipmentButton : CustomButton
{
    private TextMeshProUGUI _buttonText;
    private Character _character;

    protected override void Start()
    {
        _buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetItemButtonName(string name)
    {
        if (!_buttonText)
        {
            _buttonText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        _buttonText.text = name;
    }

    public void AddLeftClick(UnityAction action)
    {
        OnLeftClick.AddListener(action);
    }

    public void ClearLeftClick()
    {
        OnLeftClick.RemoveAllListeners();
    }

    public void AddRightClick(UnityAction action)
    {
        OnRightClick.AddListener(action);
    }

    public void ClearRightClick()
    {
        OnRightClick.RemoveAllListeners();
    }

    public void SetCharacter(Character character)
    {
        _character = character;
    }
}
