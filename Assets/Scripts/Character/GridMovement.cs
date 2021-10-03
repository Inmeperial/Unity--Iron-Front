using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float tpThreshold;
    public float rotationWatchdog;
    private float _watchdogCounter;
    private bool _forcedForward;
    private bool _checkedDirToLast;
    private List<Tile> _tilesList = new List<Tile>();
    int _tilesIndex;
    Character _character;
    private float _moveSpeed;
    private float _rotationSpeed;
    private bool _move;
    private bool _rotate;
    private int _index;
    private float _yPosition;
    private Vector3 _posToRotate;
    private Vector3 _prevPos;

    private Action _callback;
    private Vector3 _nextPos;
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

    

    /// <summary>
    /// Set tiles for Character to move and start the movement.
    /// </summary>
    public void StartMovement(List<Tile> tilesList)
    {
        _forcedForward = false;
        _tilesList = tilesList;
        _tilesIndex = 1;
        _move = true;
    }

    
    private void Movement()
    {
        if(_tilesIndex < _tilesList.Count)
        {
            Vector3 newPos = _tilesList[_tilesIndex].transform.position;
            newPos.y = _yPosition;
            if (_forcedForward == false)
            {
                if (!CheckIfFacing(newPos))
                {
                    _rotate = true;
                    _posToRotate = newPos;
                    Vector3 prev;
                    
                    if (_tilesIndex - 1 >= 0)
                    {
                        prev = _tilesList[_tilesIndex - 1].transform.position;
                    }
                    else
                    {
                        prev = _character.GetMyPositionTile().transform.position;
                    }
                    
                    prev.y = _yPosition;
                    _prevPos = prev;
                        
                    return;
                }  
            }



            Vector3 targetDir = newPos - transform.position;
            if ((newPos - transform.position).magnitude <= tpThreshold)
            {
                _forcedForward = false;
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

            Vector3 forwardDir = Vector3.zero;
            Vector3 last = Vector3.zero;
            Vector3 preLast = Vector3.zero;
            last = _tilesList[_tilesList.Count - 1].transform.position;
            if (_tilesList.Count >= 2)
            {
                preLast = _tilesList[_tilesList.Count - 2].transform.position;
                
            }
            else
            {
                preLast = _character.GetMyPositionTile().transform.position;
            }
            
            last.y = _yPosition;
            preLast.y = _yPosition;
            forwardDir = (last - preLast).normalized;
            transform.LookAt(transform.position + forwardDir);
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
            _forcedForward = true;
            _watchdogCounter = 0;
            _rotate = false;
            _posToRotate = Vector3.zero;
            if (_callback == null) return;
            
            _callback?.Invoke();
            _callback = null;

            return;
        }
        
        float step = _rotationSpeed * Time.deltaTime;
        Vector3 dir = (_posToRotate - _prevPos).normalized;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, step, 0f);
        transform.rotation = Quaternion.LookRotation(newDir);

    }

    private bool CheckIfFacing(Vector3 pos)
    {
        Vector3 dir = pos - transform.position;
        Vector3 thresholdPlus = new Vector3(dir.x + 0.1f, dir.y, dir.z + 0.1f);
        Vector3 thresholdMin = new Vector3(dir.x - 0.1f, dir.y, dir.z - 0.1f);
        return transform.forward == dir.normalized || transform.forward == thresholdPlus || transform.forward == thresholdMin;
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

    public void StartRotation(Action callback)
    {
        _rotate = true;
        _callback = callback;
    }
}
