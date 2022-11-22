using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EquipmentButton : CustomButton
{
    [SerializeField] private Image _buttonImage;
    [SerializeField] private TextMeshProUGUI _itemUsesText;
    [SerializeField] private TextMeshProUGUI _abilityCooldownText;
    public void SetButtonText(string text, EquipableSO.EquipableType type)
    {
        switch (type)
        {
            case EquipableSO.EquipableType.Item:
                _abilityCooldownText.gameObject.SetActive(false);

                _itemUsesText.text = text;
                _itemUsesText.gameObject.SetActive(true);
                break;

            case EquipableSO.EquipableType.Active:
            case EquipableSO.EquipableType.Passive:
                _itemUsesText.gameObject.SetActive(false);

                _abilityCooldownText.text = text;
                _abilityCooldownText.gameObject.SetActive(true);
                break;
        }
    }

    public void SetButtonIcon(Sprite sprite)
    {
        _buttonImage.sprite = sprite;
    }

    public void HideItemUsesText()
    {
        _itemUsesText.gameObject.SetActive(false);
    }

    public void HideAbilityCooldownText()
    {
        _abilityCooldownText.gameObject.SetActive(false);
    }

    public void AddLeftClick(UnityAction action) => OnLeftClick.AddListener(action);

    public void ClearLeftClick() => OnLeftClick.RemoveAllListeners();

    public void AddRightClick(UnityAction action) => OnRightClick.AddListener(action);

    public void ClearRightClick() => OnRightClick.RemoveAllListeners();

    public void ResetButton()
    {
        ClearRightClick();
        ClearLeftClick();
    }
}
