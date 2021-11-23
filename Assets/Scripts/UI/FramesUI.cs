using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FramesUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Image mechaImage;
	// public Image leftGunIcon;
	// public Image rightGunIcon;
	public Image overWeight;
	public Image colorBorder;
	public Image colorNamePlate;
	public Color playerColor;
	public Color enemyColor;
	public TextMeshProUGUI mechaName;
	public CustomButton selectionButton;
	public MasterShaderScript masterShader;
	public Character _characterSelected;

	// FramesUI(Image mecha, Image leftGun, Image rightGun, TextMeshProUGUI myName)
	// {
	// 	mechaImage = mecha;
	// 	leftGunIcon = leftGun;
	// 	rightGunIcon = rightGun;
	// 	mechaName = myName;
	// }
	
	FramesUI(Image mecha, TextMeshProUGUI myName)
	{
		mechaImage = mecha;
		mechaName = myName;
	}

	// public void ChangeData(Image newMechaImage, Image newMechaLeftIcon, Image newMechaRightIcon, TextMeshProUGUI newMechaName)
	// {
	// 	mechaImage = newMechaImage;
	// 	leftGunIcon = newMechaLeftIcon;
	// 	rightGunIcon = newMechaRightIcon;
	// 	mechaName.text = newMechaName.text;
	// }
	
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
		//TODO: pintar mesh
		_characterSelected.SetShaderForAllParts(SwitchTextureEnum.TextureFresnel);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//TODO: pintar mesh
		_characterSelected.SetShaderForAllParts(SwitchTextureEnum.TextureClean);
	}
}
