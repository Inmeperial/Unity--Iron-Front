using System.Collections;
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
    public UnityEvent OnRightClick;
    public UnityEvent OnLeftClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            PressRight();
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PressLeft();
        }
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
