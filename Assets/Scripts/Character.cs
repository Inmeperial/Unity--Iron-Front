using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    GridMovement _move;
    public float speed;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        _move = GetComponent<GridMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetTargetToMove();
        }
    }

    void GetTargetToMove()
    {
        Transform target = MouseRay.GetTarget(mask);
        if (IsValidTarget(target))
        {
            _move.StartMovement(GetTileBelow(), target, speed);
        }
        else
        {
            Debug.Log("sin target");
        }
    }

    bool IsValidTarget(Transform target)
    {
        if (target != null && target.CompareTag("GridBlock") && target.gameObject.GetComponent<Tile>().isWalkable)
            return true;
        else return false;
    }

    public Tile GetTileBelow()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, LayerMask.NameToLayer("GridBlock"));
        return hit.transform.gameObject.GetComponent<Tile>();
    }
}
