using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
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
        //StartCoroutine(Move(_tilesIndex));
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
            Debug.Log("pos2: " + targetDir.normalized);
            if ((newPos - transform.position).magnitude <= 1.25f)
            {
                transform.position = newPos;
                _tilesIndex++;
            }
            else
            {
                transform.position += targetDir.normalized * _moveSpeed * Time.deltaTime;
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
        if (CheckIfFacing(_posToRotate))
        {
            _rotate = false;
            _posToRotate = Vector3.zero;
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
        if (transform.forward == dir.normalized)
            return true;
        else return false;
    }
    // IEnumerator Rotate(Vector3 pos)
    // {
    //     // var targetDir = (pos - transform.position).normalized;
    //     // Debug.Log(targetDir);
    //     // float interpolation = 1;
    //     // Debug.Log("Angle: " + Vector3.SignedAngle(transform.forward, pos - transform.position, transform.forward));
    //     // while(transform.forward != targetDir)
    //     // {
    //     //     interpolation -= Time.deltaTime / rotationSpeed;
    //     //     if (interpolation <= 0)
    //     //     {
    //     //         transform.forward = targetDir;
    //     //         break;
    //     //     }
    //     //     Vector3 newRot = Vector3.Lerp(targetDir, transform.forward, interpolation);
    //     //     //transform.Rotate(0, 1 * rotationSpeed, 0);
    //     //     transform.Rotate(0,1,0);
    //     //     yield return new WaitForEndOfFrame();
    //     // }
    //     Vector3 targetDir = pos - transform.position;
    //     while (transform.forward != targetDir.normalized)
    //     {
    //         float step = rotationSpeed * Time.deltaTime;
    //     
    //         Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0f);
    //         transform.rotation = Quaternion.LookRotation(newDir);
    //
    //         yield return new WaitForEndOfFrame();
    //     }
    //     yield return null;
    // }

    // IEnumerator Move(int tilesIndex)
    // {
    //     if(transform.position != _tilesList[_tilesList.Count-1].transform.position)
    //     {
    //         var newPos = _tilesList[tilesIndex].transform.position;
    //         newPos.y += transform.position.y;
    //         Vector3 targetDir = newPos - transform.position;
    //         if (transform.forward != targetDir.normalized)
    //             yield return StartCoroutine(Rotate(newPos));
    //         if ((newPos - transform.position).magnitude <= 1.25f)
    //         {
    //             transform.position = newPos;
    //             tilesIndex++;
    //         }
    //         else
    //         {
    //             var dir = (newPos - transform.position).normalized;
    //             transform.position += dir * moveSpeed * Time.deltaTime;
    //         }
    //         yield return new WaitForEndOfFrame();
    //
    //         if (tilesIndex < _tilesList.Count)
    //             StartCoroutine(Move(tilesIndex));
    //         else 
    //         {
    //             _character.ReachedEnd();
    //         }
    //     }
    // }

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
}
