using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CloseUpCamera : MonoBehaviour
{
    public float speed = 25f;

	public float minHeight = 5f;
	public float maxHeight = 8f;
    public float threshold = 15f;
    public float moveBackInZ;
    
    [Header("1 closest to player")]
    [Header("0 closest to enemy")]
    [Header("0 to 1 (float)")]
    public float lerp = .9f;
    private  Camera _mainCamWorld;
    [SerializeField] private Camera _mainCamUI;
    private  Camera _closeCameraWorld;
    [SerializeField] private Camera _closeCameraUI;
    
	private void Start()
    {
        _mainCamWorld = transform.parent.GetComponent<Camera>();
        _closeCameraWorld = GetComponent<Camera>();
        _closeCameraWorld.enabled = false;
    }

	public void MoveCameraWithLerp(Vector3 enemyPosToLerp, Vector3 playerPosToLerp, Action callback = null)
    {
        FindObjectOfType<CameraMovement>().LockCamera(true);
        _closeCameraUI.enabled = true;
        _closeCameraWorld.enabled = true;
        _mainCamWorld.enabled = false;
        _mainCamUI.enabled = false;
		//Calcular el height según la distancia de las dos unidades y, clampearla en un min y max
		var distanceHeight = Vector3.Distance(enemyPosToLerp, playerPosToLerp);
		var clampedHeight = Mathf.Clamp(distanceHeight, minHeight, maxHeight);
		enemyPosToLerp.y = clampedHeight;
		playerPosToLerp.y = clampedHeight;
		
		var destination = Vector3.Lerp(enemyPosToLerp, playerPosToLerp, lerp);
		destination.z += moveBackInZ;
        StartCoroutine(Move(destination, enemyPosToLerp, callback));
    }

    public void MoveCameraToParent(Vector3 destination, Vector3 targetToLook, Action callback = null, float callbackDelay = 0)
    {
        StartCoroutine(MoveToParent(destination, targetToLook, callback, callbackDelay));
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
        }
        transform.LookAt(targetToLook);
    }
    IEnumerator MoveToParent(Vector3 destination, Vector3 targetToLook, Action callback = null, float callbackDelay = 0)
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
            yield return new WaitForSeconds(callbackDelay);
            callback();
        }
    }

    public void ResetCamera()
    {
        FindObjectOfType<CameraMovement>().LockCamera(false);
        StopAllCoroutines();
        transform.localPosition = Vector3.zero;
        _mainCamWorld.enabled = true;
        _mainCamUI.enabled = true;
        _closeCameraWorld.enabled = false;
        _closeCameraUI.enabled = false;
    }
}
