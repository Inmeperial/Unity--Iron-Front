using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _gameCam;
    private Vector3 _camInitialPos;
    [Header("Manual Control")]
    public float manualMovementSpeed;
    public float speed;
    public float rotationSpeed;
    //public TextMeshProUGUI cameraSpeedText;
    
    [Header("Transition")]
    public float fixationThreshold;
    public float distanceToStartRotation;
    public float transitionRotationSpeed;
    public float transitionRotationWatchdog;
    private bool _cameraLocked;
    private Vector3 _initialPos;
    private Quaternion _initialRot;
    private float _watchdogCounter;
    private Rigidbody _rb;

    private float _yPos;
    private TurnManager _turnManager;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _initialPos = transform.position;
        _initialRot = transform.rotation;
        _yPos = transform.position.y;
       // cameraSpeedText.text = "CAMERA SPEED: " + manualMovementSpeed;
        _gameCam = Camera.main;
        _camInitialPos = _gameCam.transform.localPosition;
        _turnManager = FindObjectOfType<TurnManager>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            manualMovementSpeed++;
            //cameraSpeedText.text = "CAMERA SPEED: " + manualMovementSpeed;
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            manualMovementSpeed--;
            //cameraSpeedText.text = "CAMERA SPEED: " + manualMovementSpeed;
        }
            
        
        
        if (_cameraLocked)
        {
            _rb.velocity = Vector3.zero;
            return;
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        var dir = transform.right * x + transform.forward * z;
        _rb.velocity = dir * manualMovementSpeed;


        if (Input.GetKey(KeyCode.E))
            transform.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));

        if (_gameCam.transform.localPosition != _camInitialPos)
            _gameCam.transform.localPosition = _camInitialPos;
    }

    public void LockCamera(bool status)
    {
        _cameraLocked = status;
    }

    public void ResetCamera()
    {
        transform.position = _initialPos;
        transform.rotation = _initialRot;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Limit"))
        {
            _rb.velocity = Vector3.zero;
        }

    }

    public void MoveTo(Transform transformToMove, Action callback = null, Transform lookAt = null)
    {
        var pos = transformToMove.position;
        pos.y = _yPos;
        _cameraLocked = true;
        _turnManager.DeactivatePortraitsButtons();
        StartCoroutine(Move(pos, callback, lookAt));
    }

    IEnumerator Move(Vector3 pos, Action callback = null, Transform lookAt = null)
    {
        bool started = false;
        while ((pos - transform.position).magnitude >= fixationThreshold)
        {
            if (lookAt)
            {
                var angle = Vector3.Angle(transform.forward, lookAt.forward);
                if (angle != 0 && !started &&
                    (pos - transform.position).magnitude <= distanceToStartRotation)
                {
                    started = true;
                    StartCoroutine(Rotate(lookAt));
                }
            }
                
            var dir = pos - transform.position;

            transform.position += dir * (speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        transform.position = pos;
        _cameraLocked = false;
        _turnManager.ActivatePortraitsButtons();
        
        if (callback != null)
            callback();
    }

    IEnumerator Rotate(Transform lookAt)
    {
        _watchdogCounter = 0;
        var pos = lookAt.position;
        pos.y = transform.position.y;
        pos += lookAt.forward;
        while (!CheckIfFacing(pos) && _watchdogCounter < transitionRotationWatchdog)
        {
            float step = transitionRotationSpeed * Time.deltaTime;
            Vector3 dir = (pos - transform.position).normalized;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, step, 0f);
            transform.rotation = Quaternion.LookRotation(newDir);
            _watchdogCounter++;
            yield return new WaitForEndOfFrame();
        }
        
        transform.LookAt(pos);
        yield return null;
    }

    private bool CheckIfFacing(Vector3 pos)
    {
        var dir = pos - transform.position;
        var thresholdPlus = new Vector3(dir.x + 0.1f, dir.y, dir.z + 0.1f);
        var thresholdMin = new Vector3(dir.x - 0.1f, dir.y, dir.z - 0.1f);
        return transform.forward == dir.normalized || transform.forward == thresholdPlus || transform.forward == thresholdMin;
    }
}
