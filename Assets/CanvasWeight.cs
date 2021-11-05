using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasWeight : MonoBehaviour
{
    public int currentWeight, maxWeight;
    public Vector3 maxZRot;
    public TextMeshProUGUI text;
    RectTransform myRect;

    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            UpdateRotation();
    }

    public void UpdateRotation()
	{
        Vector3 RotateTo = (currentWeight * maxZRot) / maxWeight;//tengo que convertir el valor de currentWeight / maxWeight, en una escala donde 0 = minZRot y 1 = maxZRot
        Debug.Log("Rotate to: " + RotateTo);

        if (RotateTo.z != myRect.rotation.z)
            StartCoroutine(RotateLerp(Quaternion.Euler(RotateTo), .5f));
	}

    IEnumerator RotateLerp(Quaternion endValue, float duration)
	{
        text.text = currentWeight.ToString();
        float lerpTime = 0;
        Quaternion startValue = myRect.rotation;

        while (lerpTime < duration)
		{
            myRect.rotation = Quaternion.Lerp(startValue, endValue, lerpTime / duration);
            lerpTime += Time.deltaTime;
            yield return null;
		}
        myRect.rotation = endValue;
	}

}
