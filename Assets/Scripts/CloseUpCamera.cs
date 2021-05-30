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

    public void MoveCameraWithLerp(Vector3 enemyPosToLerp, Vector3 playerPosToLerp)
    {
        FindObjectOfType<CameraMovement>().LockCamera(true);
        _cam.enabled = true;
        _mainCam.enabled = false;
        enemyPosToLerp.y = height;
        playerPosToLerp.y = height;
        var destination = Vector3.Lerp(enemyPosToLerp, playerPosToLerp, lerp);
        StartCoroutine(Move(destination, enemyPosToLerp));
    }

    public void MoveCameraToParent(Vector3 destination, Vector3 targetToLook, Action callback = null)
    {
        StartCoroutine(MoveToParent(destination, targetToLook, callback));
    }

    IEnumerator Move(Vector3 destination, Vector3 targetToLook, Action callback = null)
    {
        while ((destination - transform.position).magnitude >= threshold)
        {
            var dir = (destination - transform.position).normalized;
            transform.position += dir * (speed * Time.deltaTime);
            transform.LookAt(targetToLook);
            yield return new WaitForEndOfFrame();
        }
        transform.position = destination;
        if (callback != null)
        {
            callback();
            transform.localRotation = Quaternion.identity;
        }
        else
        {
            transform.LookAt(targetToLook);    
        }
    }
    IEnumerator MoveToParent(Vector3 destination, Vector3 targetToLook, Action callback = null)
    {
        while ((destination - transform.position).magnitude >= threshold)
        {
            var dir = (destination - transform.position).normalized;
            transform.position += dir * (speed * Time.deltaTime);
            transform.LookAt(targetToLook);
            yield return new WaitForEndOfFrame();
        }
        transform.position = destination;
        transform.localRotation = Quaternion.identity;
        
        if (callback != null)
        {
            callback();
        }
    }

    public void ResetCamera()
    {
        FindObjectOfType<CameraMovement>().LockCamera(false);
        StopAllCoroutines();
        transform.localPosition = Vector3.zero;
        _mainCam.enabled = true;
        _cam.enabled = false;
    }
}
