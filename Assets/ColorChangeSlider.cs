using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChangeSlider : MonoBehaviour
{

    public Slider red, green, blue;
    public Image image;

    // Update is called once per frame
    void Update()
    {
        image.color = new Color(red.value, green.value, blue.value);
    }
}
