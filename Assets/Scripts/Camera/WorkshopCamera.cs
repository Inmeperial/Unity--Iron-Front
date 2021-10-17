using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WorkshopCamera : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform[] _mechasToLook;
    [SerializeField] private Transform[] _cameraPositions;
    [SerializeField] private Transform[] _mechaEditCameraPositions;
    [SerializeField] private float _tpThreshold;
    [SerializeField] private float _speed;
    private bool _isMoving;
    private CustomButton[] _buttons;

    private void Awake()
    {
        transform.position = _startPosition.position;
        transform.LookAt(_mechasToLook[_mechasToLook.Length-1]);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Start()
    {
        _buttons = FindObjectsOfType<CustomButton>();
        WorkshopManager.OnClickPrevious += OverviewCameraMove;
        WorkshopManager.OnClickNext += OverviewCameraMove;
        WorkshopManager.OnClickEdit += FocusCameraMove;
        WorkshopManager.OnClickCloseEdit += UnfocusCameraMove;
    }

    public void OverviewCameraMove(int mechaIndex)
    {
        Debug.Log("overview index: " + mechaIndex);
        OnMove();
        var t = _cameraPositions[mechaIndex];
        StartCoroutine(StartMovement(t));
    }
    

    public void FocusCameraMove(int mechaIndex)
    {
        Debug.Log("focus index: " + mechaIndex);
        OnMove();
        var t = _mechaEditCameraPositions[mechaIndex];
        StartCoroutine(StartMovement(t));
        StartCoroutine(LookWhileMoving(_mechasToLook[mechaIndex]));
    }

    public void UnfocusCameraMove(int mechaIndex)
    {
        Debug.Log("unfocus index: " + mechaIndex);
        OnMove();
        var t = _cameraPositions[mechaIndex];
        StartCoroutine(StartMovement(t));
        StartCoroutine(LookWhileMoving(_mechasToLook[mechaIndex]));
    }

    private void OnMove()
    {
        _isMoving = true;
        ChangeButtonInteraction(false);
        StopAllCoroutines();
    }
    
    IEnumerator StartMovement(Transform t)
    {
        Vector3 endPos = t.position;
        while (_isMoving)
        {
            var dir = endPos - transform.position;
            transform.position += dir.normalized * (_speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, endPos) <= _tpThreshold)
            {
                transform.position = endPos;
                _isMoving = false;
            }
            yield return new WaitForEndOfFrame();
        }

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
}
