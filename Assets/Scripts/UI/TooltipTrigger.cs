using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public string header;
	
	[Multiline()]
	public string content;
	

	public void OnPointerEnter(PointerEventData eventData)
	{
		StartCoroutine(TooltipDelay());
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		StopCoroutine(TooltipDelay());
		TooltipSystem.Hide();
	}

	IEnumerator TooltipDelay()
	{
		yield return new WaitForSecondsRealtime(.5f);
		TooltipSystem.Show(content, header);
	}
}
