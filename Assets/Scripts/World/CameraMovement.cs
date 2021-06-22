using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float fixationThreshold;
    public float speed;
    public float rotationSpeed;
    private bool _cameraLocked;
    private Vector3 _initialPos;
    private Quaternion _initialRot;

    private Rigidbody _rb;

    private float _yPos;

    public TextMeshProUGUI cameraSpeedText;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _initialPos = transform.position;
        _initialRot = transform.rotation;
        _yPos = transform.position.y;
        cameraSpeedText.text = "CAMERA SPEED: " + speed;
    }
    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            speed++;
            cameraSpeedText.text = "CAMERA SPEED: " + speed;
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            speed--;
            cameraSpeedText.text = "CAMERA SPEED: " + speed;
        }
            
        
        
        if (_cameraLocked)
        {
            _rb.velocity = Vector3.zero;
            return;
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        var dir = transform.right * x + transform.forward * z;
        _rb.velocity = dir * speed;


        if (Input.GetKey(KeyCode.E))
            transform.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
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

    public void MoveTo(Vector3 pos, Action callback = null)
    {
        pos.y = _yPos;
        _cameraLocked = true;
        StartCoroutine(Move(pos, callback));
    }

    IEnumerator Move(Vector3 pos, Action callback = null)
    {
        while ((pos - transform.position).magnitude >= fixationThreshold)
        {
            var dir = (pos - transform.position).normalized;

            transform.position += dir * (speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        transform.position = pos;
        _cameraLocked = false;
        
        if (callback != null)
            callback();
    }
}
