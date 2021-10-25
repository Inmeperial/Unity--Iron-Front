using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FramesUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Image mechaImage;
	public Image leftGunIcon;
	public Image rightGunIcon;
	public Image colorBorder;
	public Image colorNamePlate;
	public Color playerColor;
	public Color enemyColor;
	public TextMeshProUGUI mechaName;
	public CustomButton selectionButton;
	private Character _character;
	public MasterShaderScript masterShader;

	FramesUI(Image mecha, Image leftGun, Image rightGun, TextMeshProUGUI myName)
	{
		mechaImage = mecha;
		leftGunIcon = leftGun;
		rightGunIcon = rightGun;
		mechaName = myName;
	}

	public void ChangeData(Image newMechaImage, Image newMechaLeftIcon, Image newMechaRightIcon, TextMeshProUGUI newMechaName)
	{
		mechaImage = newMechaImage;
		leftGunIcon = newMechaLeftIcon;
		rightGunIcon = newMechaRightIcon;
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
		_character = character;
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

	public void OnPointerEnter(PointerEventData eventData)
	{
		//TODO: pintar mesh
		//_character.GetMaterialHandler().SetSelectedPartMaterialToBody()

		// esto es la carita de arriba con el orden de los turnos. activar el fresnel

		//masterShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureHighLight);
		
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		//TODO: pintar mesh
		//_character.GetMaterialHandler()

		// esto es la carita de arriba con el orden de los turnos. desActivar el fresnel 

		// masterShader.ConvertEnumToStringEnumForShader(SwitchTextureEnum.TextureClean);

	}
}
