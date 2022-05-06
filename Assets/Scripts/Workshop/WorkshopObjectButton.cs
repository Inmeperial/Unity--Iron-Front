using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class WorkshopObjectButton : CustomButton
{
    [SerializeField] private TextMeshProUGUI _objectName;
    [SerializeField] private Image _objectImage;
    //private TextMeshProUGUI _description;

    public void SetObjectName(string name)
    {
        _objectName.text = name;
    }

    public void SetObjectSprite(Sprite sprite)
    {
        _objectImage.sprite = sprite;
    }
    
    // public void SetDescriptionTextField(TextMeshProUGUI textField)
    // {
    //     _description = textField;
    // }

    public void SetLeftClick(UnityAction action)
    {
        OnLeftClick.RemoveAllListeners();
        OnLeftClick.AddListener(action);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        FindObjectOfType<NotSelectable>().previousSelection = this;
        base.OnDeselect(eventData);
    }
}
