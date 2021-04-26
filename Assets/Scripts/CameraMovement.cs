using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    private bool _cameraLocked;
    private Vector3 _initialPos;
    private Quaternion _initialRot;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _initialPos = transform.position;
        _initialRot = transform.rotation;
    }
    // Update is called once per frame
    void Update()
    {
        if (!_cameraLocked)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            var dir = transform.right * x + transform.forward * z;
            _rb.velocity = dir * speed;


            if (Input.GetKey(KeyCode.E))
                transform.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
            if (Input.GetKey(KeyCode.Q))
                transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
        }
    }

    public void LockCamera()
    {
        _cameraLocked = !_cameraLocked;
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
}
