using UnityEngine;
using UnityEngine.UI;

public class ColorChangeSlider : MonoBehaviour
{

    public Slider red, green, blue;
    public Image image;
    public ColorChangeSlider otherSlider;

    // Update is called once per frame
    void Update()
    {
        image.color = new Color(red.value, green.value, blue.value);
    }

    public void ChangeOtherSlider()
	{
        otherSlider.red.value = red.value;
        otherSlider.green.value = green.value;
        otherSlider.blue.value = blue.value;
	}
}
