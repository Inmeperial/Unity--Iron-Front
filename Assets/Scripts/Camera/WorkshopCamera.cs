﻿using System;
using System.Collections;
using UnityEngine;

public class WorkshopCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorkshopManager _workshopManager;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform[] _mechasToLook;
    [SerializeField] private Transform[] _cameraPositions;
    [SerializeField] private Transform[] _mechaEditCameraPositions;
    private bool _isMoving;
    private CustomButton[] _buttons;

    private void Awake()
    {
        transform.position = _startPosition.position;
        transform.LookAt(_mechasToLook[_mechasToLook.Length-1]);
    }

    private void Start()
    {
        _buttons = FindObjectsOfType<CustomButton>();

        _workshopManager.OnClickPrevious += OverviewCameraMove;
        _workshopManager.OnClickNext += OverviewCameraMove;
        _workshopManager.OnClickEdit += FocusCameraMove;
        _workshopManager.OnClickCloseEdit += UnfocusCameraMove;
        _workshopManager.OnClickMecha += OverviewCameraMove;
    }

    public void OverviewCameraMove(int mechaIndex)
    {
        SetMove();
        var t = _cameraPositions[mechaIndex];
        StartCoroutine(StartMovement(t));
    }
    

    public void FocusCameraMove(int mechaIndex)
    {
        SetMove();
        var t = _mechaEditCameraPositions[mechaIndex];
        StartCoroutine(StartMovement(t));
        StartCoroutine(LookWhileMoving(_mechasToLook[mechaIndex]));
    }

    public void UnfocusCameraMove(int mechaIndex)
    {
        SetMove();
        var t = _cameraPositions[mechaIndex];
        StartCoroutine(StartMovement(t));
        StartCoroutine(LookWhileMoving(_mechasToLook[mechaIndex]));
    }

    private void SetMove()
    {
        _isMoving = true;
        ChangeButtonInteraction(false);
        StopAllCoroutines();
    }
    
    IEnumerator StartMovement(Transform t)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = t.position;
        float lerpTime = 0;
        float duration = .5f;
        
        while (lerpTime < duration)
		{
            float smoothness = lerpTime / duration;
            smoothness = smoothness * smoothness * smoothness * (smoothness * (6f * smoothness - 15f) + 10f);//Cuenta para que frene un poco cuando arranca y cuando termina.
            transform.position = Vector3.Lerp(startPos, endPos, smoothness);
            lerpTime += Time.deltaTime;
            yield return null;
		}
        transform.position = endPos;


        ChangeButtonInteraction(true);

        if (t.childCount > 0)
        {
            transform.LookAt(t.GetChild(0));
        }
    }
    
    IEnumerator LookWhileMoving(Transform t)
    {
        while (_isMoving)
        {
            transform.LookAt(t);
            yield return new WaitForEndOfFrame();
        }
    }
    
    void ChangeButtonInteraction(bool state)
    {
        foreach (var button in _buttons)
        {
            button.interactable = state;
        }
    }

    private void OnDestroy()
    {
        _workshopManager.OnClickPrevious -= OverviewCameraMove;
        _workshopManager.OnClickNext -= OverviewCameraMove;
        _workshopManager.OnClickEdit -= FocusCameraMove;
        _workshopManager.OnClickCloseEdit -= UnfocusCameraMove;
        _workshopManager.OnClickMecha -= OverviewCameraMove;
    }
}
