using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptList : MonoBehaviour
{
    public GameObject objForScale = default;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            objForScale.transform.position = child.transform.position;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (Transform child in transform)
            {
                child.transform.GetComponent<TestScriptEnrique>().SetCover(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            foreach (Transform child in transform)
            {
                child.transform.GetComponent<TestScriptEnrique>().SetCover(false);
            }
        }
    }

}
