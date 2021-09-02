using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopCamera : MonoBehaviour
{
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _firstTargetToLook;
    [SerializeField] private float _tpThreshold;
    [SerializeField] private float _speed;
    private bool _isMoving;

    private CustomButton[] _buttons;

    private void Awake()
    {
        transform.position = _startPosition.position;
        transform.LookAt(_firstTargetToLook);
    }

    private void Start()
    {
        _buttons = FindObjectsOfType<CustomButton>();
    }

    public void Move(Transform t)
    {
        _isMoving = true;
        ChangeButtonInteraction(false);
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
}
