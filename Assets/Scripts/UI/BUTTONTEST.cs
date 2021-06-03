using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BUTTONTEST : MonoBehaviour
{
    public EffectsController effects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            effects.CreateDamageText("Miss", 0, Camera.main.transform.forward * 10, true);
        if (Input.GetKeyDown(KeyCode.S))
            effects.CreateDamageText("100", 1, Camera.main.transform.forward * 10, true);
        if (Input.GetKeyDown(KeyCode.D))
            effects.CreateDamageText("300", 2, Camera.main.transform.forward * 10, true);
    }

    public void LeftClick()
    {
        Debug.Log("Left Click");
    }

    public void RightClick()
    {
        Debug.Log("RightClick");
    }
}
