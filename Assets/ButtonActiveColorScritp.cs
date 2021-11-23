using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonActiveColorScritp : MonoBehaviour
{
	[SerializeField]Button _partsTabButton;
	Button _currentSelectedPart;
	Button _currentSelectedTab;
	Image _currentSelectedAbilityIcon;
	TextMeshProUGUI _tabText;
	TextMeshProUGUI _partText;
	Color _partNormalColor, _tabNormalColor;
	Color _partSelectedColor;

	private void Start()
	{
		ChangeTabColors(_partsTabButton);
	}

	
	public void SetColorToActive(Button buttonToChange)
	{
		if (buttonToChange == null) return;

		//Devuelvo el color del boton que antes tenia seleccionado al normal y cambio el color del nuevo boton que seleccione.
		var tempColors = buttonToChange.colors;
		_partNormalColor = tempColors.normalColor;
		_partSelectedColor = tempColors.selectedColor;
		if (_currentSelectedPart && _partText || _currentSelectedAbilityIcon) //Si ya tengo una parte seleccionada la deselecciono
		{
			DeactivatePartsButtonColor();
		}

		_currentSelectedPart = buttonToChange;
		tempColors.normalColor = _currentSelectedPart.colors.selectedColor;
		_currentSelectedPart.colors = tempColors;
		
		//Cambio el color del texto del boton
		_partText = _currentSelectedPart.GetComponentInChildren<TextMeshProUGUI>();
		_partText.color = Color.white;
	}

	public void ChangeAbilityIconColor(Image abilityIconToChange)
	{
		_currentSelectedAbilityIcon = abilityIconToChange;
		_currentSelectedAbilityIcon.color = _partSelectedColor;
	}

	public void DeactivatePartsButtonColor()
	{
		if (_currentSelectedPart)
		{
			var tempColors = _currentSelectedPart.colors;
			tempColors.normalColor = _partNormalColor;
			_currentSelectedPart.colors = tempColors;
			_partText.color = Color.black;
		}
		

		if (_currentSelectedAbilityIcon)
		{
			_currentSelectedAbilityIcon.color = Color.white;
			_currentSelectedAbilityIcon = null;
		}
	}

	public void ChangeTabColors(Button buttonToChange)
	{
		if (buttonToChange == null) buttonToChange = _partsTabButton;

		//Devuelvo el color del boton que antes tenia seleccionado al normal y cambio el color del nuevo boton que seleccione.

		var tempColors = buttonToChange.colors;
		_tabNormalColor = tempColors.normalColor;
		

		if(_currentSelectedTab && _tabText)//si ya tengo una tab seleccionada la deselecciono
		{
			DeactivateTabButtonColor();
		}

		_currentSelectedTab = buttonToChange;
		tempColors.normalColor = _currentSelectedTab.colors.selectedColor;
		_currentSelectedTab.colors = tempColors;

		//Cambio el color del texto del boton
		_tabText = _currentSelectedTab.GetComponentInChildren<TextMeshProUGUI>();
		_tabText.color = Color.white;
	}

	public void DeactivateTabButtonColor()
	{
		var tempColors = _currentSelectedTab.colors;
		tempColors.normalColor = _tabNormalColor;
		_currentSelectedTab.colors = tempColors;
		_tabText.color = Color.black;
	}

	public void SelectPartsTab()
	{
		ChangeTabColors(_partsTabButton);
	}
}
