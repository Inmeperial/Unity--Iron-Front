using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasWeight : MonoBehaviour
{
    public int currentWeight, maxWeight;
    public int minZRot, maxZRot;
    int currentRotation;
    RectTransform myRect;
    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRotation()
	{
        currentRotation = currentWeight / maxWeight;
        Debug.Log("Current Rot = " + currentRotation);
	}
}
