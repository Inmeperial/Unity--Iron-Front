using System;
using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera _gameCam;
    private Vector3 _camInitialPos;
    [Header("Manual Control")]
    public float manualMovementSpeed;
    public float speed;
    public float rotationSpeed;

    [Header("Transition")]
    public float fixationThreshold;
    public float distanceToStartRotation;
    public float transitionRotationSpeed;
    public float transitionRotationWatchdog;
    private bool _cameraLocked;
    private Vector3 _initialPos;
    private Quaternion _initialRot;
    private float _watchdogCounter;
    private bool _rotating;
    private Rigidbody _rb;

    private float _yPos;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _initialPos = transform.position;
        _initialRot = transform.rotation;
        _yPos = transform.position.y;
        _gameCam = Camera.main;
        _camInitialPos = _gameCam.transform.localPosition;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            manualMovementSpeed++;
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            manualMovementSpeed--;
        }
            
        
        
        if (_cameraLocked)
        {
            _rb.velocity = Vector3.zero;
            return;
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 dir = transform.right * x + transform.forward * z;
        _rb.velocity = dir * manualMovementSpeed;


        if (Input.GetKey(KeyCode.E))
            transform.Rotate(new Vector3(0, -rotationSpeed * Time.fixedDeltaTime, 0));
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(new Vector3(0, rotationSpeed * Time.fixedDeltaTime, 0));

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
        Vector3 pos = transformToMove.position;
        pos.y = _yPos;
        _cameraLocked = true;
        //PortraitsController.Instance.DeactivatePortraitsButtons();
        StartCoroutine(Move(pos, callback, lookAt));
    }

    private IEnumerator Move(Vector3 pos, Action callback = null, Transform lookAt = null)
    {
        float timer = 0;
        while ((pos - transform.position).magnitude >= fixationThreshold)
        {
            timer += Time.deltaTime;
            
            if (timer >= 5) 
                break;
            
            if (lookAt)
            {
                float angle = Vector3.Angle(transform.forward, lookAt.forward);
                if (angle != 0 && !_rotating &&
                    (pos - transform.position).magnitude <= distanceToStartRotation)
                {
                    _rotating = true;
                    StartCoroutine(Rotate(lookAt));
                }
            }
                
            Vector3 dir = pos - transform.position;

            transform.position += dir * (speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        transform.position = pos;
        
        if (!_rotating) _cameraLocked = false;
        
        //PortraitsController.Instance.ActivatePortraitsButtons();


        yield return new WaitWhile(() => _rotating);
        
        if (callback != null)
            callback();
    }

    IEnumerator Rotate(Transform lookAt)
    {
        _watchdogCounter = 0;
        Vector3 pos = GetPositionToLook(lookAt);
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
        _cameraLocked = false;
        _rotating = false;
        yield return null;
    }

    private bool CheckIfFacing(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        Vector3 thresholdPlus = new Vector3(dir.x + 0.1f, dir.y, dir.z + 0.1f);
        Vector3 thresholdMin = new Vector3(dir.x - 0.1f, dir.y, dir.z - 0.1f);
        return transform.forward == dir.normalized || transform.forward == thresholdPlus || transform.forward == thresholdMin;
    }

    private Vector3 GetPositionToLook(Transform t)
    {
        Vector3 pos = t.position;
        pos.y = transform.position.y;
        pos += t.forward;
        return pos;
    }
}
