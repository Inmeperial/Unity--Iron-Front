using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float tpThreshold;
    public float rotationWatchdog;
    private float _watchdogCounter;
    List<Tile> _tilesList;
    int _tilesIndex;
    Character _character;
    private float _moveSpeed;
    private float _rotationSpeed;
    private bool _move;
    private bool _rotate;
    private int _index;
    private float _yPosition;
    private Vector3 _posToRotate;

    private Action _callback;
    private void Start()
    {
        _character = GetComponent<Character>();
        _yPosition = transform.position.y;
    }

    private void Update()
    {
        if (_rotate)
        {
            Rotation();
            return;
        }
        if (_move)
            Movement();
    }

    

    public void StartMovement(List<Tile> tilesList)
    {
        _tilesList = tilesList;
        _tilesIndex = 1;
        _move = true;
    }

    void Movement()
    {
        if(_tilesIndex != _tilesList.Count)
        {
            var newPos = _tilesList[_tilesIndex].transform.position;
            newPos.y = _yPosition;
            if (!CheckIfFacing(newPos))
            {
                _rotate = true;
                _posToRotate = newPos;
                return;
            }
            Vector3 targetDir = newPos - transform.position;
            if ((newPos - transform.position).magnitude <= tpThreshold)
            {
                transform.position = newPos;
                _tilesIndex++;
            }
            else
            {
                transform.position += targetDir.normalized * (_moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            _character.ReachedEnd();
            _move = false;
            _tilesIndex = 0;
        }
    }
    
    private void Rotation()
    {
        
        _watchdogCounter += 1;
        if (CheckIfFacing(_posToRotate) || _watchdogCounter >= rotationWatchdog)
        {
            _watchdogCounter = 0;
            _rotate = false;
            _posToRotate = Vector3.zero;
            if (_callback != null)
                _callback();
            return;
        }
        
        float step = _rotationSpeed * Time.deltaTime;
        Vector3 dir = (_posToRotate - transform.position).normalized;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, step, 0f);
        transform.rotation = Quaternion.LookRotation(newDir);

        }

    private bool CheckIfFacing(Vector3 pos)
    {
        var dir = pos - transform.position;
        var tresholdPlus = dir;
        tresholdPlus = new Vector3(dir.x + 0.1f, dir.y, dir.z + 0.1f);
        var tresholdMin = dir;
        tresholdMin= new Vector3(dir.x - 0.1f, dir.y, dir.z - 0.1f);
        if (transform.forward == dir.normalized || transform.forward == tresholdPlus || transform.forward == tresholdMin)
            return true;
        else return false;
    }

    public void SetMoveSpeed(float speed)
    {
        _moveSpeed = speed;
    }

    public void SetRotationSpeed(float speed)
    {
        _rotationSpeed = speed;
    }

    public void SetPosToRotate(Vector3 pos)
    {
        _posToRotate = pos;
    }

    public void StartRotation()
    {
        _rotate = true;
    }
    public void StartRotation(Action callback)
    {
        _rotate = true;
        _callback = callback;
    }
}
