using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    GridMovement _move;
    public float speed;
    public LayerMask mask;
    public bool _selected;
    Tile _endTile;
    MeshRenderer _render;
    // Start is called before the first frame update
    void Start()
    {
        _selected = false;
        _move = GetComponent<GridMovement>();
        _render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_selected && Input.GetMouseButtonDown(0))
        {
            GetTargetToMove();
        }
    }

    public void GetTargetToMove()
    {
        Transform target = MouseRay.GetTarget(mask);
        if (IsValidTarget(target))
        {
            _move.StartMovement(GetTileBelow(), target, speed);
            _endTile = target.GetComponent<Tile>();
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

    public void SelectThisUnit()
    {
        Material mat = new Material(_render.sharedMaterial);
        mat.color = Color.blue;

        _render.sharedMaterial = mat;
        _selected = true;
    }

    public void DeselectThisUnit()
    {
        Material mat = new Material(_render.sharedMaterial);
        mat.color = Color.white;

        _render.sharedMaterial = mat;
        _selected = false;
    }

    public Tile GetEndTile()
    {
        return _endTile;
    }
}
