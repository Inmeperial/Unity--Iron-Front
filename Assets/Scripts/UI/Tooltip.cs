using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;

    public int characterWrapLimit;

	private RectTransform _rectTransform;

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		gameObject.SetActive(false);
	}

	public void SetText(string contet, string header = "")
	{
		if (string.IsNullOrEmpty(header))
			headerField.gameObject.SetActive(false);
		else
		{
			headerField.gameObject.SetActive(true);
			headerField.text = header;
		}

		contentField.text = contet;

		int headerLenght = headerField.text.Length;
		int contentLenght = contentField.text.Length;

		layoutElement.enabled = (headerLenght > characterWrapLimit || contentLenght > characterWrapLimit) ? true : false;
	}

	private void Update()
	{
		if (Application.isEditor)
		{
			int headerLenght = headerField.text.Length;
			int contentLenght = contentField.text.Length;

			layoutElement.enabled = (headerLenght > characterWrapLimit || contentLenght > characterWrapLimit) ? true : false;
		}

		TooltipPosition();
	}

	void TooltipPosition()
	{
		Vector2 position = Input.mousePosition;

		position.x += position.x > Screen.width / 2 ? -25 : 25;
		position.y += position.y > Screen.height / 2 ? -25 : 25;

		float pivotX = position.x / Screen.width;
		float pivotY = position.y / Screen.height;

		_rectTransform.pivot = new Vector2(pivotX, pivotY);
		transform.position = position;
	}
}
