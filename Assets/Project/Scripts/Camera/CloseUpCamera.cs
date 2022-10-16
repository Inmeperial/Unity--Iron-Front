using System;
using System.Collections;
using UnityEngine;

public class CloseUpCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _mainCamWorld;
    [SerializeField] private Camera _closeUpCameraUI;
    private Camera _closeCameraWorld;
    public float speed = 25f;

	public float minHeight = 5f;
	public float maxHeight = 8f;
    public float threshold = 15f;
    public float moveBackInZ;
    
    [Range(0,1)]
    public float lerp = .9f;
    [SerializeField] private Camera _mainCamUI;
    
    
	private void Start()
    {
        _mainCamWorld = transform.parent.GetComponent<Camera>();
        _closeCameraWorld = GetComponent<Camera>();
        _closeCameraWorld.enabled = false;
    }

	public void MoveCameraWithLerp(Vector3 enemyPosToLerp, Vector3 playerPosToLerp, Action callback = null)
    {
        FindObjectOfType<CameraMovement>().LockCamera(true);
        _closeUpCameraUI.enabled = true;
        _closeCameraWorld.enabled = true;
        _mainCamWorld.enabled = false;
        _mainCamUI.enabled = false;
		//Calcular el height según la distancia de las dos unidades y, clampearla en un min y max
		float distanceHeight = Vector3.Distance(enemyPosToLerp, playerPosToLerp);
		float clampedHeight = Mathf.Clamp(distanceHeight, minHeight, maxHeight);

        float heightRelation = 0;

        bool elevated = false;

        if (enemyPosToLerp.y < playerPosToLerp.y)
        {
            heightRelation = enemyPosToLerp.y / playerPosToLerp.y;
            elevated = true;
        }
        else if (enemyPosToLerp.y > playerPosToLerp.y)
        {
            heightRelation = playerPosToLerp.y / enemyPosToLerp.y;
            elevated = true;
        }
        
        enemyPosToLerp.y = clampedHeight;
		playerPosToLerp.y = clampedHeight;
		
		Vector3 destination = Vector3.Lerp(enemyPosToLerp, playerPosToLerp, lerp - heightRelation);
        
        if (elevated) destination.z += moveBackInZ;
        
        StartCoroutine(Move(destination, enemyPosToLerp, callback));
    }

    public void MoveCameraToParent(Vector3 targetToLook, Action callback = null, float callbackDelay = 0)
    {
        StartCoroutine(MoveToParent(targetToLook, callback, callbackDelay));
    }

    IEnumerator Move(Vector3 destination, Vector3 targetToLook, Action callback = null)
    {
        while ((destination - transform.position).magnitude >= threshold)
        {
            Vector3 dir = (destination - transform.position).normalized;
            transform.position += dir * (speed * Time.deltaTime);
            transform.LookAt(targetToLook);
            yield return new WaitForEndOfFrame();
        }
        transform.position = destination;
        transform.LookAt(targetToLook);
        if (callback != null)
        {
            callback();
        }
        
    }
    IEnumerator MoveToParent(Vector3 targetToLook, Action callback = null, float callbackDelay = 0)
    {
        Vector3 parentPos = transform.parent.position;

        while ((parentPos - transform.position).magnitude >= threshold)
        {
            Vector3 dir = (parentPos - transform.position).normalized;
            transform.position += dir * (speed * Time.deltaTime);
            transform.LookAt(targetToLook);
            yield return new WaitForEndOfFrame();
        }
        transform.position = parentPos;
        transform.localRotation = Quaternion.identity;
        
        if (callback != null)
        {
            yield return new WaitForSeconds(callbackDelay);
            callback();
        }
        FindObjectOfType<CameraMovement>().LockCamera(false);
    }

    public void ResetCamera()
    {
        FindObjectOfType<CameraMovement>().LockCamera(false);
        StopAllCoroutines();
        transform.localPosition = Vector3.zero;
        _mainCamWorld.enabled = true;
        _mainCamUI.enabled = true;
        _closeCameraWorld.enabled = false;
        _closeUpCameraUI.enabled = false;
    }
}
