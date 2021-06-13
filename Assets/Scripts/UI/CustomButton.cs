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
    public enum ButtonPart
    {
        Body,
        Legs,
        RightArm,
        LeftArm
    };
    
    public PointerEventData.InputButton button { get; set; }
    public ButtonPart buttonPart;
    public UnityEvent OnLeftClick;
    public UnityEvent OnRightClick;

    public void SetListeners()
    {
        var mng = FindObjectOfType<ButtonsUIManager>();

        switch (buttonPart)
        {
            case ButtonPart.Body:
                OnRightClick.AddListener(mng.BodySelection);
                OnLeftClick.AddListener(mng.BodyMinus);
                break;
            
            case ButtonPart.Legs:
                OnRightClick.AddListener(mng.LegsSelection);
                OnLeftClick.AddListener(mng.LegsMinus);
                break;
            
            case ButtonPart.LeftArm:
                OnRightClick.AddListener(mng.LeftArmSelection);
                OnLeftClick.AddListener(mng.LeftArmMinus);
                break;
            
            case ButtonPart.RightArm:
                OnRightClick.AddListener(mng.RightArmSelection);
                OnLeftClick.AddListener(mng.RightArmMinus);
                break;
        }
    }

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

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("boton");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("salgo del boton");
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
