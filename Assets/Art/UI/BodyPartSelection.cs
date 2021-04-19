using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BodyPartSelection : MonoBehaviour
{
	public Image squareBracket;

	public void GenerateBracket()
	{
		var myButton = EventSystem.current.currentSelectedGameObject.transform;
		var tempSquareBracket = Instantiate(squareBracket);
		tempSquareBracket.transform.SetParent(myButton, false);
		tempSquareBracket.rectTransform.anchorMin = new Vector2(0, 0);
		tempSquareBracket.rectTransform.anchorMax = new Vector2(1, 1);
	}
}
