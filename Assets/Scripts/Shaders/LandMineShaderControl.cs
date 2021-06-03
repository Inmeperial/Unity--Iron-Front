using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineShaderControl : MonoBehaviour
{
    private float num = 0f;
    private float totalNum = 0f;
    private GameObject childObj;
    private Light lightChild;

    // Start is called before the first frame update
    void Start()
    {
        childObj = this.transform.GetChild(0).gameObject;
        
        lightChild = childObj.gameObject.GetComponent<Light>();
        lightChild.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AnimationLight();
    }

    /// <summary>
    /// Animation for the light in the mine.
    /// </summary>
    public void AnimationLight()
    {
        if (num >= 360)
        {
            num = 0;
        }
        num += Time.deltaTime;
        totalNum = 3 + Mathf.Sin(Time.time) * 3;
        lightChild.intensity = totalNum;
    }
}
