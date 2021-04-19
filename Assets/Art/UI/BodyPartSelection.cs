using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BodyPartSelection : MonoBehaviour
{
	public Image counterUI;
	public int _count = 0;
	public List<Image> myCounters = new List<Image>();

	public void GenerateCounter()
	{
		
		var myButton = EventSystem.current.currentSelectedGameObject.transform;
		var tempCounterUI = Instantiate(counterUI);
		myCounters.Add(tempCounterUI);
		tempCounterUI.transform.SetParent(myButton, false);
		if(_count <= 0)
			tempCounterUI.rectTransform.anchorMin = new Vector2(0, 0);
		else
			tempCounterUI.rectTransform.anchorMin = new Vector2(0.1f * _count, 0);

		_count++;

		tempCounterUI.rectTransform.anchorMax = new Vector2(0.1f * _count, 0.15f);
		
	}

	public void RemoveCounter()
	{
		if (myCounters.Count <= 0)
		{
			return;
		}

		_count--;
		var bulletToRemove = myCounters[_count];
		myCounters.Remove(bulletToRemove);
		DestroyImmediate(bulletToRemove);
	}

	public void ClearCounters()
	{
		if (myCounters.Count <= 0)
		{
			return;
		}

		for (int i = myCounters.Count - 1; i >= 0; i--)
		{
			var tempItem = myCounters[i];
			myCounters.Remove(tempItem);
			DestroyImmediate(tempItem);
		}
		_count = 0;
	}
}
