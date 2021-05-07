using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BodyPartSelection : MonoBehaviour
{
	public Image counterUI;
	public int count = 0;
	List<Image> myCounters = new List<Image>();
	public GameObject bulletFillArea;

	public void GenerateCounter()
	{
		
		//var myButton = EventSystem.current.currentSelectedGameObject.transform;
		var tempCounterUI = Instantiate(counterUI);
		myCounters.Add(tempCounterUI);
		tempCounterUI.transform.SetParent(bulletFillArea.transform, false);
		if(count <= 0)
			tempCounterUI.rectTransform.anchorMin = new Vector2(0, 0);
		else
			tempCounterUI.rectTransform.anchorMin = new Vector2(0.1f * count, 0);

		count++;

		tempCounterUI.rectTransform.anchorMax = new Vector2(0.1f * count, 1f);
		//tempCounterUI.rectTransform.right += Vector3.right * 2;


	}

	public void RemoveCounter()
	{
		if (myCounters.Count <= 0)
		{
			return;
		}
		var bulletToRemove = myCounters[count-1];
		myCounters.Remove(bulletToRemove);
		DestroyImmediate(bulletToRemove);
		count--;
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
		count = 0;
	}
}
