using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeMechaHandler : MonoBehaviour
{
    private ParticleSystem.MainModule[] _partSystemMain;
    private GameObject smokeObj;
    //private bool _isEffectOn = false;

    //Add for para el partSystem y que todos activen o desActiven.
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<Renderer>();
            if (transform.GetChild(i).gameObject.name == "SmokeMecha")
            {
                smokeObj = transform.GetChild(i).gameObject;
            }
        }

        if (smokeObj != null)
        {
            _partSystemMain = smokeObj.GetComponent<ParticleSystem>().main;
        }
        else
        {
            Debug.Log("SmokeMecha obj not found in mecha : " + this.gameObject.name.ToString());
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        SetMachineOn(true);
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        SetMachineOn(false);
    //    }
    //}

    public void SetMachineOn(bool boolEffect)
    {
        if (boolEffect)
        {
            _partSystemMain.startSize = new ParticleSystem.MinMaxCurve(2f, 4f);
            _partSystemMain.startLifetime = 4f;
        }
        else
        {
            _partSystemMain.startSize = new ParticleSystem.MinMaxCurve(1f, 2f);
            _partSystemMain.startLifetime = 2f;
        }
    }
}
