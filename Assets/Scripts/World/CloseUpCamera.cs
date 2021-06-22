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
    [SerializeField] private Camera _uiCam;
    [Header("1 closest to player")]
    [Header("0 closest to enemy")]
    [Header("0 to 1 (float)")]
    public float lerp = .9f;
    private  Camera _cam;
    private  Camera _mainCam;
	public TextMeshProUGUI cameraLerpText;
	private void Start()
    {
        _mainCam = transform.parent.GetComponent<Camera>();
        _cam = GetComponent<Camera>();
        _cam.enabled = false;
		cameraLerpText.text = "CAMERA LERP: " + lerp;
    }

	private void Update()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			lerp = Mathf.Clamp(lerp + 0.01f, 0, 1);
			cameraLerpText.text = "CAMERA LERP: " + Math.Round(lerp, 2);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			lerp = Mathf.Clamp(lerp - 0.01f, 0, 1);
			cameraLerpText.text = "CAMERA LERP: " + Math.Round(lerp, 2);
		}
	}

	public void MoveCameraWithLerp(Vector3 enemyPosToLerp, Vector3 playerPosToLerp, Action callback = null)
    {
        FindObjectOfType<CameraMovement>().LockCamera(true);
        _uiCam.enabled = true;
        _cam.enabled = true;
        _mainCam.enabled = false;
		//Calcular el height según la distancia de las dos unidades y, clampearla en un min y max
		var distanceHeight = Vector3.Distance(enemyPosToLerp, playerPosToLerp);
		var clampedHeight = Mathf.Clamp(distanceHeight, minHeight, maxHeight);
		enemyPosToLerp.y = clampedHeight;
		playerPosToLerp.y = clampedHeight;
		
        var destination = Vector3.Lerp(enemyPosToLerp, playerPosToLerp, lerp);
        StartCoroutine(Move(destination, enemyPosToLerp, callback));
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
        }
        transform.LookAt(targetToLook);
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
        _uiCam.enabled = false;
    }
}
