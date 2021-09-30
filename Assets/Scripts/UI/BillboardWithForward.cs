using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BillboardWithForward : Billboard
{
    [Range(0,1)]
    [SerializeField] private float _forwardLerp;
    private Transform _cam;
    private Vector3 _originalPos;
    private void Start()
    {
        _cam = FindObjectOfType<CloseUpCamera>().transform;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        transform.position = Vector3.Lerp(_originalPos, _cam.position, _forwardLerp);
    }

    private void OnEnable()
    {
        _originalPos = transform.position;
    }
}
