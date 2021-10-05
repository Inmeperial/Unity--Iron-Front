using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScriptEnrique2 : MonoBehaviour
{
    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.name == "FeetsScale")
        {
            transform.parent.GetComponent<TestScriptEnrique>().SetIsHalfCover();
            coll.transform.parent.transform.position = new Vector3(300, 300, 300);
        }
        if (coll.transform.name == "BodyScale")
        {
            transform.parent.GetComponent<TestScriptEnrique>().SetIsFullCover();
            coll.transform.parent.transform.position = new Vector3(300, 300, 300);
        }
    }
}
