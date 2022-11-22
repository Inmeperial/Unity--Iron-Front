using UnityEngine;

public class BillboardWithForward : Billboard
{
    private float _forwardLerp;
    private Transform _cam;
    private Vector3 _originalPos;
    private readonly float _blockWidth = 7;
    private void Start()
    {
        _cam = FindObjectOfType<CloseUpCamera>().transform;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        var dist = (_cam.position - _originalPos).magnitude;
        _forwardLerp = 1 - (_blockWidth / dist);
        transform.position = Vector3.Lerp(_originalPos, _cam.position, _forwardLerp);
    }

    private void OnEnable()
    {
        _originalPos = transform.position;
    }

    private void OnDisable()
    {
        transform.position = _originalPos;
    }
}
