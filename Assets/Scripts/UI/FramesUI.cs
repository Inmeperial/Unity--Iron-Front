using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FramesUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Image mechaImage;
	public Image overWeight;
	public Image colorBorder;
	public Image colorNamePlate;
	public Color playerColor;
	public Color enemyColor;
	public TextMeshProUGUI mechaName;
	public CustomButton selectionButton;
	public Character _characterSelected;

	FramesUI(Image mecha, TextMeshProUGUI myName)
	{
		mechaImage = mecha;
		mechaName = myName;
	}

	public void ChangeData(Image newMechaImage, TextMeshProUGUI newMechaName)
	{
		mechaImage = newMechaImage;
		mechaName.text = newMechaName.text;
	}

	public void RemoveButtonLeftClickListeners()
	{
		selectionButton.OnLeftClick.RemoveAllListeners();
	}
	
	public void AddButtonLeftClickListener(UnityAction action)
	{
		selectionButton.OnLeftClick.AddListener(action);
	}
	
	public void RemoveButtonRightClickListeners()
	{
		selectionButton.OnRightClick.RemoveAllListeners();
	}
	
	public void AddButtonRightClickListener(UnityAction action)
	{
		selectionButton.OnRightClick.AddListener(action);
	}

	public FramesUI SetCharacter(Character character)
	{
		_characterSelected = character;

		_characterSelected.OnOverweight += OverweightIconState;
		return this;
	}
	public FramesUI SetSprite(Sprite sprite)
	{
		mechaImage.sprite = sprite;
		return this;
	}

	public FramesUI SetName(string unitName)
	{
		mechaName.text = unitName;
		return this;
	}

	public FramesUI SetBorderColor(EnumsClass.Team team)
	{
		var color =  team == EnumsClass.Team.Green ? playerColor : enemyColor;
		colorBorder.color = color;
		colorNamePlate.color = color;
		
		return this;
	}

	public void OverweightIconState(bool state)
	{
		overWeight.gameObject.SetActive(state);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (_characterSelected.IsDead()) return;
		_characterSelected.SetShaderForAllParts(SwitchTextureEnum.TextureFresnel);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (_characterSelected.IsDead()) return;
		_characterSelected.SetShaderForAllParts(SwitchTextureEnum.TextureClean);
	}
}
