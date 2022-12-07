using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class CustomButton : Selectable, IPointerClickHandler
{
    public UnityEvent OnLeftClick;
    public UnityEvent OnRightClick;
    
    protected override void Start()
    {
        if (Application.isEditor)
            return;

        base.Start();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
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

    protected virtual void PressRight()
    {
        if (!IsActive() || !IsInteractable())
            return;
        
        OnRightClick?.Invoke();
    }

    protected virtual void PressLeft()
    {
        if (!IsActive() || !IsInteractable())
            return;
        OnLeftClick?.Invoke();
    }
}
