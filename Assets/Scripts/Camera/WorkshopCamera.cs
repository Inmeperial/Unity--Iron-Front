using System;
using System.Collections;
using System.Collections.Generic;
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
    private int _index;
    private CustomButton[] _buttons;

    private void Awake()
    {
        _index = 3;
        transform.position = _startPosition.position;
        transform.LookAt(_mechasToLook[_index]);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Start()
    {
        _buttons = FindObjectsOfType<CustomButton>();
    }

    public void Move(Transform t)
    {
        _isMoving = true;
        ChangeButtonInteraction(false);
        StopAllCoroutines();
        StartCoroutine(StartMovement(t));
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

    void ChangeButtonInteraction(bool state)
    {
        foreach (var button in _buttons)
        {
            button.interactable = state;
        }
    }

    public void NextUnitToLookAt()
    {
        if (_index >= _cameraPositions.Length - 1)
            _index = 0;
        else _index++;
        
        Move(_cameraPositions[_index]);
	}

    public void PreviusUnitToLookAt()
    {
        if (_index == 0)
            _index = _cameraPositions.Length-1;
        else _index--;
        
        Move(_cameraPositions[_index]);
    }

    public void EditMechaCameraMove()
	{
        transform.LookAt(_mechasToLook[_index]);
        Move(_mechaEditCameraPositions[_index]);
	}
}
