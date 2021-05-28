using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUpCamera : MonoBehaviour
{
    public float speed;

    public float height;
    
    public float threshold;
    
    [Header("1 closest to player")]
    [Header("0 closest to enemy")]
    [Header("0 to 1 (float)")]
    public float lerp;
    private  Camera _cam;
    private  Camera _mainCam;
    private void Start()
    {
        _mainCam = transform.parent.GetComponent<Camera>();
        _cam = GetComponent<Camera>();
        _cam.enabled = false;
    }

    public void MoveCamera(Vector3 posToLerpA, Vector3 posToLerpB)
    {
        FindObjectOfType<CameraMovement>().LockCamera();
        _cam.enabled = true;
        _mainCam.enabled = false;
        posToLerpA.y = height;
        posToLerpB.y = height;
        var destination = Vector3.Lerp(posToLerpA, posToLerpB, lerp);
        StartCoroutine(Move(destination, posToLerpA));
    }

    IEnumerator Move(Vector3 destination, Vector3 targetToLook)
    {
        while ((destination - transform.position).magnitude >= threshold)
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
        FindObjectOfType<CameraMovement>().LockCamera();
        StopAllCoroutines();
        transform.localPosition = Vector3.zero;
        _mainCam.enabled = true;
        _cam.enabled = false;
    }
}
