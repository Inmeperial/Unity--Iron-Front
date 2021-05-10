using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : MonoBehaviour
{
    [SerializeField] private bool isWalkable;
    public MeshRenderer render;
    public List<Tile> neighboursForMove = new List<Tile>();
    public List<Tile> allNeighbours = new List<Tile>();
    public LayerMask obstacle;
    public LayerMask character;
    [SerializeField] private bool _isFree = true;
    Material _mat;
    public bool showLineGizmo = true;

    private bool _hasTileAbove;

    private Character _unitAbove;

    public Color colorForMouse;
    public Color colorForMove;
    public Color colorForAttack;
    public Color colorDefaultClear;


    public GameObject planeForMove;
    private Material _planeForMoveMat;
    private MeshRenderer _planeForMoveRender;
    public GameObject planeForAttack;
    private Material _planeForAttackMat;
    private MeshRenderer _planeForAttackRender;
    public GameObject planeForMouse;
    private Material _planeForMouseMat;
    private MeshRenderer _planeForMouseRender;

    private TileMaterialhandler _materialHandler;

    public bool inMoveRange;
    public bool inAttackRange;

    private void Awake()
    {
        _isFree = true;
    }
    private void Start()
    {
        //Materiales & Renders
        _mat = new Material(render.sharedMaterial);
        _planeForMoveRender = planeForMove.GetComponent<MeshRenderer>();
        _planeForMoveMat = _planeForMoveRender.material;
        _planeForAttackRender = planeForAttack.GetComponent<MeshRenderer>();
        _planeForAttackMat = _planeForAttackRender.material;
        _planeForMouseRender = planeForMouse.GetComponent<MeshRenderer>();
        _planeForMouseMat = _planeForMouseRender.material;

        _materialHandler = GetComponent<TileMaterialhandler>();

        inMoveRange = false;
        inAttackRange = false;
}

    public void GetNeighbours()
    {
        RayForMoveNeighbours(Vector3.right);
        RayForMoveNeighbours(Vector3.left);
        RayForMoveNeighbours(Vector3.forward);
        RayForMoveNeighbours(Vector3.back);
        RayForMoveNeighbours((Vector3.up + Vector3.right + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.up + Vector3.back + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.up + Vector3.left + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.up + Vector3.forward + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.down + Vector3.right + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.down + Vector3.back + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.down + Vector3.left + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.down + Vector3.forward + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours(Vector3.right);
        RayToAllNeighbours(Vector3.left);
        RayToAllNeighbours(Vector3.forward);
        RayToAllNeighbours(Vector3.back);
        RayToAllNeighbours((Vector3.up + Vector3.right + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.up + Vector3.back + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.up + Vector3.left + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.up + Vector3.forward + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.down + Vector3.right + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.down + Vector3.back + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.down + Vector3.left + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours((Vector3.down + Vector3.forward + new Vector3(0, 0.01f, 0)).normalized);
    }

    //Cast a ray to neighbouring tiles and check if they are walkable.
    void RayForMoveNeighbours(Vector3 dir)
    {
        float d;
        if (transform.localScale.x >= transform.localScale.z)
            d = transform.localScale.x;
        else d = transform.localScale.z;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, d))
        {
            var neighbour = hit.collider.GetComponent<Tile>();
            if (neighbour != null && neighbour.isWalkable)
                if (!neighboursForMove.Contains(neighbour))
                    neighboursForMove.Add(neighbour);
        }
    }

    void RayToAllNeighbours(Vector3 dir)
    {
        RaycastHit hit;
        float d;
        if (transform.localScale.x >= transform.localScale.z)
            d = transform.localScale.x;
        else d = transform.localScale.z;
        if (Physics.Raycast(transform.position, dir, out hit, d))
        {
            var neighbour = hit.collider.GetComponent<Tile>();
            if (neighbour != null)
                if (!allNeighbours.Contains(neighbour) && !_hasTileAbove)
                    allNeighbours.Add(neighbour);
        }
    }

    //Check if there is a tile above.
    public bool CheckIfTileAbove()
    {
        return _hasTileAbove = Physics.Raycast(transform.position, transform.up, 1, obstacle);
    }

    public bool HasTileAbove()
    {
        return _hasTileAbove;
    }

    bool IsCharacterAbove()
    {
        return Physics.Raycast(transform.position, transform.up, 1, character);
    }

    public void RemoveMoveNeighbour(Tile tile)
    {
        if (neighboursForMove.Contains(tile))
            neighboursForMove.Remove(tile);
    }

    public void AddMoveNeighbour(Tile tile)
    {
        neighboursForMove.Add(tile);
    }

    #region Color methods.
    //Make this tile walkable.
    public void MakeWalkableColor()
    {
        isWalkable = true;
        _hasTileAbove = false;
        var mat = new Material(render.sharedMaterial);
        mat.color = Color.green;

        render.sharedMaterial = mat;
    }
    //Make this tile not walkable.
    public void MakeNotWalkableColor()
    {
        isWalkable = false;
        var mat = new Material(render.sharedMaterial);
        mat.color = Color.red;

        render.sharedMaterial = mat;
    }

    //Change tile color for pathfinding preview.
    public void InMoveRangeColor()
    {
        //_planeForMoveMat.color = colorForMove;

        //_planeForMoveRender.material = _planeForMoveMat;
        //_planeForAttackRender.enabled = true;

        _materialHandler.StatusToMove();
        _materialHandler.DiseableAndEnableStatus(true);
    }

    public void EndInMoveRangeColor()
    {
        //_planeForMoveMat.color = colorDefaultClear;

        //_planeForMoveRender.material = _planeForMoveMat;
        //_planeForAttackRender.enabled = false;
        _materialHandler.DiseableAndEnableStatus(false);
    }

    //Revert tile color when pathfinding preview ends.
    public void ResetColor()
    {
        _planeForMoveMat.color = colorDefaultClear;
        _planeForAttackMat.color = colorDefaultClear;
        _planeForMoveRender.material = _planeForMoveMat;
        _planeForAttackRender.material = _planeForAttackMat;
    }

    public void MouseOverColor()
    {
        //_planeForMouseMat.color = colorForMouse;

        //_planeForMouseRender.material = _planeForMouseMat;
        //_planeForMouseRender.enabled = true;
        _materialHandler.DiseableAndEnableSelectedNode(true);
    }

    public void EndMouseOverColor()
    {
        //_planeForMouseMat.color = colorDefaultClear;

        //_planeForMouseRender.material = _planeForMouseMat;
        //_planeForMouseRender.enabled = false;
        inMoveRange = false;
        _materialHandler.DiseableAndEnableSelectedNode(false);
    }

    public void CanBeAttackedColor()
    {
        //_planeForAttackMat.color = colorForAttack;

        //_planeForAttackRender.material = _planeForAttackMat;
        //_planeForAttackRender.enabled = true;
        _materialHandler.StatusToAttack();
        _materialHandler.DiseableAndEnableSelectedNode(true);
    }

    public void EndCanBeAttackedColor()
    {
        inAttackRange = false;
        _materialHandler.DiseableAndEnableSelectedNode(false);
    }

    public void CanMoveAndAttackColor()
    {
        _materialHandler.StatusToAttackAndMove();
        _materialHandler.DiseableAndEnableSelectedNode(true);
    }

    public void EndCanMoveAndAttackColor()
    {
        inMoveRange = false;
        inAttackRange = false;
        _materialHandler.DiseableAndEnableSelectedNode(false);
    }
    #endregion

    public bool IsFree()
    {
        return _isFree;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void MakeTileOccupied()
    {
        _isFree = false;
        foreach (var tile in neighboursForMove)
        {
            tile.RemoveMoveNeighbour(this);
        }
    }

    public void MakeTileFree()
    {
        _isFree = true;
        foreach (var tile in neighboursForMove)
        {
            tile.AddMoveNeighbour(this);
        }
    }

    public void ShowGizmo()
    {
        showLineGizmo = !showLineGizmo;
    }

    public void SetUnitAbove(Character unit)
    {
        _unitAbove = unit;
    }

    public Character GetUnitAbove()
    {
        return _unitAbove;
    }
    private void OnDrawGizmos()
    {
        if (showLineGizmo)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
