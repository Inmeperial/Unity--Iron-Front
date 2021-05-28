using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpCamera : MonoBehaviour
{
    public float speed;

    public float threshold;

    private Camera _cam;
    private Camera _mainCam;
    private void Start()
    {
        _mainCam = Camera.main;
        _cam = GetComponent<Camera>();
        _cam.enabled = false;
    }

    public void MoveCamera(Vector3 destination, Transform targetToLook)
    {
        _cam.enabled = true;
        _mainCam.enabled = false;
        
        StartCoroutine(Move(destination, targetToLook));
    }

    IEnumerator Move(Vector3 destination, Transform targetToLook)
    {
        while ((destination - transform.position).magnitude <= threshold)
        {
            var dir = (destination - transform.position).normalized;
            transform.position += dir * (speed * Time.deltaTime);
            transform.LookAt(targetToLook);
            yield return new WaitForEndOfFrame();
        }

        transform.position = destination;
        transform.LookAt(targetToLook);
    }

    public void ResetCamera()
    {
        transform.localPosition = Vector3.zero;
        _mainCam.enabled = true;
        _cam.enabled = false;
    }
}
