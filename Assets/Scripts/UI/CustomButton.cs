﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CustomButton : Selectable, IPointerClickHandler
{
    public PointerEventData.InputButton button { get; set; }
    public UnityEvent OnLeftClick;
    public UnityEvent OnRightClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Right:
                PressRight();
                break;
            case PointerEventData.InputButton.Left:
                PressLeft();
                break;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
       
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
       
    }

    private void PressRight()
    {
        if (!IsActive() || !IsInteractable())
            return;
        
        OnRightClick?.Invoke();
    }

    private void PressLeft()
    {
        if (!IsActive() || !IsInteractable())
            return;
        OnLeftClick?.Invoke();
    }
}
