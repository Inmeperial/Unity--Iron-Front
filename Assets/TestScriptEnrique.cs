using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptEnrique : MonoBehaviour
{
    public GameObject mecha;
    //private bool _isUp = false;
    private float test3 = 0f;
    private Vector3 testMechacenter;
    private Vector3 testMechasize;

    // Start is called before the first frame update
    void Start()
    {
        
        //GameObject mechaBody = mecha.transform.Find("Body GO").gameObject;
        //GameObject mechaLegs = mecha.transform.Find("Legs GO").gameObject;
        //Vector3 boxSize = mecha.GetComponent<BoxCollider>().bounds.size;

        //boxSize2 = mecha.GetComponent<BoxCollider>().center;

        //pos real base del collider = obj.transform.position.y + obj.GetComponent<BoxCollider>().center.y;

        var test = this.GetComponent<BoxCollider>().center.y;
        var test2 = this.GetComponent<BoxCollider>().bounds.size.x;

        // si el colission del objeto del nivel, la parte de arriba es mas alta que el colission del mecha body o mecha legs.
        // chequear primero que la escala del obj sea 1.
        // size = cuanto va para cada lado positivo y negativo.
        // center = se mueve el centro, osea que mueve toda la estructura.
        // 

    }

    // Update is called once per frame
    void Update()
    {
        testMechacenter = mecha.GetComponent<BoxCollider>().center;
        testMechasize = mecha.GetComponent<BoxCollider>().size;

        float num = ((testMechasize.y / 2) + testMechacenter.y) * mecha.transform.localScale.y;
        float num2 = (((testMechasize.y / 2) * -1) + testMechacenter.y) * mecha.transform.localScale.y;
        Debug.Log("Colision Ar = " + num);
        Debug.Log("Colision Ab = " + num2);


        //if (myBox.boun)
        //{

        //}

        //if (_isUp)
        //{
        //    Debug.Log("Arr");
        //}
        //else
        //{
        //    Debug.Log("Ab");
        //}

    }
}
