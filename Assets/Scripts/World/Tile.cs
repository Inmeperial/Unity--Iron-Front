using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX.Utility;


public class Tile : MonoBehaviour
{
    [SerializeField] private bool isWalkable;
    public MeshRenderer render;
    public List<Tile> neighboursForMove = new List<Tile>();
    public List<Tile> allNeighbours = new List<Tile>();
    public LayerMask obstacle;
    public LayerMask characterMask;
    private bool _isFree = true;
    private Material _mat;
    public bool showLineGizmo = true;

    private bool _hasTileAbove;

    private Character _unitAbove;

    private TileMaterialhandler _materialHandler;

    public bool inMoveRange;
    public bool inAttackRange;
    public bool inPreviewRange;
    public bool unitAboveSelected;
    
    private void Awake()
    {
        _isFree = true;
        _materialHandler = GetComponent<TileMaterialhandler>();

        inMoveRange = false;
        inAttackRange = false;
        inPreviewRange = false;
        unitAboveSelected = false;
    }

    public void GetNeighbours()
    {
        RayForMoveNeighbours(Vector3.forward);
        RayForMoveNeighbours(Vector3.right);
        RayForMoveNeighbours(Vector3.back);
        RayForMoveNeighbours(Vector3.left);
        RayForMoveNeighbours((Vector3.up + Vector3.right + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.up + Vector3.back + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.up + Vector3.left + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.up + Vector3.forward + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.down + Vector3.right + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.down + Vector3.back + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.down + Vector3.left + new Vector3(0, 0.01f, 0)).normalized);
        RayForMoveNeighbours((Vector3.down + Vector3.forward + new Vector3(0, 0.01f, 0)).normalized);
        RayToAllNeighbours(Vector3.forward);
        RayToAllNeighbours(Vector3.right);
        RayToAllNeighbours(Vector3.back);
        RayToAllNeighbours(Vector3.left);
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
    private void RayForMoveNeighbours(Vector3 dir)
    {
        float d;
        if (transform.localScale.x >= transform.localScale.z)
            d = transform.localScale.x;
        else d = transform.localScale.z;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, d))
        {
            var neighbour = hit.collider.GetComponent<Tile>();
            if (neighbour && neighbour.isWalkable)
                if (!neighboursForMove.Contains(neighbour))
                    neighboursForMove.Add(neighbour);
        }
    }

    private void RayToAllNeighbours(Vector3 dir)
    {
        RaycastHit hit;
        float d;
        if (transform.localScale.x >= transform.localScale.z)
            d = transform.localScale.x;
        else d = transform.localScale.z;
        if (Physics.Raycast(transform.position, dir, out hit, d))
        {
            var neighbour = hit.collider.GetComponent<Tile>();
            if (neighbour)
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

    private bool IsCharacterAbove()
    {
        return Physics.Raycast(transform.position, transform.up, 1, characterMask);
    }

    private void RemoveMoveNeighbour(Tile tile)
    {
        if (neighboursForMove.Contains(tile))
            neighboursForMove.Remove(tile);
    }

    private void AddMoveNeighbour(Tile tile)
    {
        neighboursForMove.Add(tile);
    }

    #region Color methods.
    //Make this tile walkable.
    public void MakeWalkableColor()
    {
        isWalkable = true;
        _hasTileAbove = false;
    }
    //Make this tile not walkable.
    public void MakeNotWalkableColor()
    {
        isWalkable = false;
    }

    //Pinta el tile en rango de movimiento
    public void InMoveRangeColor()
    {
        _materialHandler.StatusToMove();
        _materialHandler.DiseableAndEnableStatus(true);
    }

    //Despinta el tile en rango de movimiento
    public void EndInMoveRangeColor()
    {
        inMoveRange = false;
        _materialHandler.DiseableAndEnableStatus(false);
    }


    private void OnMouseOver()
    {
        MouseOverColor();
    }

    private void OnMouseExit()
    {
        EndMouseOverColor();
    }

    //Pinta el tile cuando el mouse esta encima
    public void MouseOverColor()
    {
        _materialHandler.DiseableAndEnableSelectedNode(true);
    }

    //Despinta el tile cuando el mouse esta encima
    public void EndMouseOverColor()
    {
        if(!unitAboveSelected)
            _materialHandler.DiseableAndEnableSelectedNode(false);
    }

    //Pinta el tile en rango de ataque
    public void CanBeAttackedColor()
    {
        _materialHandler.StatusToAttack();
        _materialHandler.DiseableAndEnableStatus(true);
    }

    //Despinta el tile en rango de ataque
    public void EndCanBeAttackedColor()
    {
        inAttackRange = false;
        _materialHandler.DiseableAndEnableStatus(false);
    }
    
    public void MortarCanBeAttackedColor()
    {
        //_materialHandler.StatusAoEOfAttackForMortar(true);
        _materialHandler.DiseableAndEnableSelectedNodeForMortar(true);
    }

    //Despinta el tile en rango de ataque
    public void MortarEndCanBeAttackedColor()
    {
        inAttackRange = false;
        _materialHandler.DiseableAndEnableSelectedNodeForMortar(false);
    }

    //Pinta el tile en rango de ataque y movimiento
    public void CanMoveAndAttackColor()
    {
        _materialHandler.StatusToAttackAndMove();
        _materialHandler.DiseableAndEnableStatus(true);
    }

    //Despinta el tile en rango de ataque y movimiento
    public void EndCanMoveAndAttackColor()
    {
        inMoveRange = false;
        inAttackRange = false;
        _materialHandler.DiseableAndEnableStatus(false);
    }
    
    //Pinta el tile para el preview de ataque
    public void InAttackPreviewColor()
    {
        _materialHandler.StatusBulletAoEOfAttackForMortar(true);
        //MouseOverColor();
    }
    
    //Despinta el tile para el preview de ataque
    public void EndAttackPreviewColor()
    {
        inPreviewRange = false;
        _materialHandler.StatusBulletAoEOfAttackForMortar(false);
        _materialHandler.DiseableAndEnableStatus(false);
        EndMouseOverColor();
    }
    public void MortarActivationRange()
    {
        _materialHandler.DiseableAndEnableActivationNodeForMortar(true);
        _materialHandler.StatusActivationRageForMortar();
    }

    // //CAMBIAR CUANDO ESTE EL SHADER
    public void EndActivationRangeColor()
    {
        //Debug.Log("END ACTIVATION RANGE");
        _materialHandler.DiseableAndEnableActivationNodeForMortar(false);
    }

    //Pinta el ultimo tile del path
    //CAMBIAR
    public void LastInPathColor()
    {
        //Debug.Log("ultimo on");
        _materialHandler.DiseableAndEnableStatus(true);
        _materialHandler.StatusTileToMoveToLastTileSelected();
    }
    
    //Despinta el ultimo tile del path
    //CAMBIAR
    public void EndLastInPathColor()
    {
        //Debug.Log("ultimo off");
    }
    #endregion

    public bool IsUnitFree()
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

    public Character GetCharacterAbove()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.up, out hit, 3, characterMask);
        if (hit.transform)
            return hit.transform.GetComponent<Character>();
        else return null;
    }

    
}
