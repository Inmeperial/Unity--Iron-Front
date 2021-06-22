using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSwitch : MonoBehaviour
{
    public GameObject obj1, obj2;
    private Vector3 _obj1OriginalPos, _obj2OriginalPos;
    private float _obj1OriginalPosHeight, _obj2OriginalPosHeight;
    private bool _isSwitchObjectOn = false;
    private bool _isMoveObjInY = false;
    private bool _isMoveObjInX = false;
    private bool _isMoveObjInYToOrigin = false;

    //Move to Y location
    //Fix the Y location
    //Move to X location
    //Fix the X location
    //Move to Y Origin location
    //Fix the Y Origin location

    //The 3 fixed locations are coz i dont like that the position is not exactly equal to the origin that is going to be swaped.

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetForSwitchObjects(obj1, obj2);
        }
        if (_isSwitchObjectOn)
        {
            if (_isMoveObjInY)
            {
                MoveObjectsInYPos();
            }
            if (_isMoveObjInX)
            {
                MoveObjectsInXPos();
            }
            if (_isMoveObjInYToOrigin)
            {
                MoveObjectsInYToNewPos();
            }

        }
    }

    private void SetForSwitchObjects(GameObject obj1, GameObject obj2)
    {
        _obj1OriginalPos = obj1.transform.position;
        _obj2OriginalPos = obj2.transform.position;

        _obj1OriginalPosHeight = obj1.GetComponent<MeshFilter>().mesh.bounds.extents.y * 2;
        _obj2OriginalPosHeight = obj2.GetComponent<MeshFilter>().mesh.bounds.extents.y * 4;

        _isSwitchObjectOn = true;
        _isMoveObjInY = true;
    }

    private void MoveObjectsInYPos()
    {
        bool inLocationNoFixed1 = false;
        bool inLocationNoFixed2 = false;

        if (obj1.transform.position.y <= _obj1OriginalPosHeight)
        {
            obj1.transform.position += obj1.transform.up * 1 * Time.deltaTime;
        }
        else
        {
            inLocationNoFixed1 = true;
        }

        if (obj2.transform.position.y <= _obj2OriginalPosHeight)
        {
            obj2.transform.position += obj2.transform.up * 1 * Time.deltaTime;
        }
        else
        {
            inLocationNoFixed2 = true;
        }
        if (inLocationNoFixed1 && inLocationNoFixed2)
        {
            FixObjectsLocationInY();
        }

    }

    private void FixObjectsLocationInY()
    {
        obj1.transform.position = new Vector3(0, _obj1OriginalPosHeight, 0);
        obj2.transform.position = new Vector3(0, _obj2OriginalPosHeight, 0);
        _isMoveObjInY = false;
        _isMoveObjInX = true;
    }

    private void MoveObjectsInXPos()
    {
        bool inLocationNoFixed1 = false;
        bool inLocationNoFixed2 = false;

        if (obj1.transform.position.x <= _obj2OriginalPos.x)
        {
            obj1.transform.position += obj1.transform.right * 1 * Time.deltaTime;
        }
        else
        {
            inLocationNoFixed1 = true;
        }

        if (obj2.transform.position.x >= _obj1OriginalPos.x)
        {
            obj2.transform.position -= obj2.transform.right * 1 * Time.deltaTime;
        }
        else
        {
            inLocationNoFixed2 = true;
        }
        if (inLocationNoFixed1 && inLocationNoFixed2)
        {
            FixObjectsLocationInX();
        }

    }

    private void FixObjectsLocationInX()
    {
        obj1.transform.position = new Vector3(_obj2OriginalPos.x, obj1.transform.position.y, obj1.transform.position.z);
        obj2.transform.position = new Vector3(_obj1OriginalPos.x, obj1.transform.position.y, obj1.transform.position.z);
        _isMoveObjInX = false;
        _isMoveObjInYToOrigin = true;
    }

    private void MoveObjectsInYToNewPos()
    {
        bool inLocationNoFixed1 = false;
        bool inLocationNoFixed2 = false;

        if (obj1.transform.position.y >= _obj2OriginalPos.y)
        {
            obj1.transform.position -= obj1.transform.up * 1 * Time.deltaTime;
        }
        else
        {
            inLocationNoFixed1 = true;
        }

        if (obj2.transform.position.y >= _obj1OriginalPos.y)
        {
            obj2.transform.position -= obj2.transform.up * 1 * Time.deltaTime;
        }
        else
        {
            inLocationNoFixed2 = true;
        }
        if (inLocationNoFixed1 && inLocationNoFixed2)
        {
            FixObjectsLocationInYInYToNewPos();
        }

    }

    private void FixObjectsLocationInYInYToNewPos()
    {
        obj1.transform.position = _obj2OriginalPos;
        obj2.transform.position = _obj1OriginalPos;
        _isMoveObjInYToOrigin = false;
        _isSwitchObjectOn = false;
    }
}
