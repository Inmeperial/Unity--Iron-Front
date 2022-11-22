using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private float _showDelay = 0.5f;
	private string _header;
	
	private string _content;

    public void OnPointerEnter(PointerEventData eventData)
	{
		if (string.IsNullOrEmpty(_header) && string.IsNullOrEmpty(_content))
			return;

		StopAllCoroutines();
		StartCoroutine(TooltipDelay());
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		StopAllCoroutines();
		TooltipSystem.Hide();
	}

	IEnumerator TooltipDelay()
	{
		yield return new WaitForSecondsRealtime(_showDelay);
		TooltipSystem.Show(_content, _header);
	}

	public void SetData(string header = "", string content = "")
    {
		_header = header;
		_content = content;
    }
}
