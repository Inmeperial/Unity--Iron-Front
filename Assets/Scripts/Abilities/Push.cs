using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Push : Ability
{
    private int _abilityUseRange;
    private int _pushDistance;
    private TileHighlight _highlight;
    //private Dictionary<Tile, int> _tilesInRangeChecked = new Dictionary<Tile, int>();
    private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
    private Tile _lastForwardTile, _lastBackTile, _lastRightTile, _lastLeftTile;
    bool collides;
    Character collidingUnit;
    public int collisionDamage = 50;
    public int pushDamage = 25;
    public override void Select()
    {
        Debug.Log("pinto tiles push");
        if (!_highlight)
        {
            _highlight = FindObjectOfType<TileHighlight>();
        }
        _abilityUseRange = _abilityData.pushUseRange;
        _pushDistance = _abilityData.pushDistance;
        PaintUseTiles(_character.GetMyPositionTile(), 0, Vector3.zero);
    }

    public override void Deselect()
    {
        _highlight.ClearTilesInActivationRange(_tilesInRange);
        _tilesInRange.Clear();
    }

    public override void Use(Action callback = null)
    {
		if (Input.GetMouseButtonDown(0))
		{
            Debug.Log("Pushing");
            var selection = MouseRay.GetTargetTransform(_character.block);
            var selectedTile = selection.GetComponent<Tile>();
            if (!selectedTile || !_tilesInRange.Contains(selectedTile)) return;
            var selectedUnit = selectedTile.GetUnitAbove();
            if (!selectedUnit) return;

            //Para que solo puedas empujar enemigos
            var unitTeam = selectedUnit.GetUnitTeam();
            if (unitTeam == EnumsClass.Team.Green) return;

            var tileToPushTo = GetTileToPushTo(selectedTile, (selectedUnit.transform.position - _character.transform.position).normalized, 0);
            PushAction(tileToPushTo, selectedUnit);
            if (callback != null)
                callback();
        }
    }

    /// <summary>
    /// Does the push movement and damage
    /// </summary>
    /// <param name="tileToPush"> The tile the unit is pushed to</param>
    /// <param name="enemy"> The unit that is pushed</param>
    void PushAction(Tile tileToPush, Character enemy)
	{
        //Hago un Lerp del enemy hacia el tileToPushTo
        enemy.transform.position = Vector3.Lerp(enemy.transform.position, tileToPush.transform.position, 1f);
        enemy.ChangeMyPosTile(tileToPush);
        enemy.body.TakeDamage(pushDamage);
		if (collides)
		{
            enemy.body.TakeDamage(collisionDamage);
            if (collidingUnit)
            {
                //Hacer daño a la otra unidad
                collidingUnit.body.TakeDamage(collisionDamage);
            }
        }
	}

    /// <summary>
    /// Returns the tile the unit is pushed to
    /// </summary>
    /// <param name="currentTile"></param>
    /// <param name="dir"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    Tile GetTileToPushTo(Tile currentTile, Vector3 dir, int count)
	{
        if (count >= _pushDistance) return currentTile;
        Debug.Log("Push To dir = " + dir);
        count++;
        if (dir == Vector3.forward)
        {
            Debug.Log("forward count: " + count);
            RaycastHit forwardHit;

            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.forward, out forwardHit);
            if (forwardHit.transform)
            {
                Tile t = forwardHit.transform.GetComponent<Tile>();

                if (t && t.IsWalkable())//capaz algo con este chequeo me puede decir que no agrega el casillero porque esta ocupado, osea choca con algo
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    _lastForwardTile = t;//Para saber a que tile empujar en esta direccion
                    return GetTileToPushTo(t, forwardHit.transform.forward, count);
                }
                else if (!t.IsWalkable())
				{
                    collides = true;
                    collidingUnit = t.GetUnitAbove();
                    return currentTile;

				}
            }
        }

        if(dir == Vector3.back)
		{
            RaycastHit backHit;

            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.forward * -1, out backHit);

            if (backHit.transform)
            {
                Tile t = backHit.transform.GetComponent<Tile>();

                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    _lastBackTile = t;//Para saber a que tile empujar en esta direccion
                    return GetTileToPushTo(t, -backHit.transform.forward, count);
                }
                else if (!t.IsWalkable())
                {
                    collides = true;
                    collidingUnit = t.GetUnitAbove();
                    return currentTile;
                }
            }
        }

        if(dir == Vector3.right)
		{
            RaycastHit rightHit;

            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.right * -1, out rightHit);

            if (rightHit.transform)
            {
                Tile t = rightHit.transform.GetComponent<Tile>();

                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    _lastRightTile = t;//Para saber a que tile empujar en esta direccion
                    return GetTileToPushTo(t, rightHit.transform.right, count);
                }
                else if (!t.IsWalkable())
                {
                    collides = true;
                    collidingUnit = t.GetUnitAbove();
                    return currentTile;
                }
            }
        }

        if(dir == Vector3.left)
		{
            RaycastHit leftHit;

            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.right * -1, out leftHit);

            if (leftHit.transform)
            {
                Tile t = leftHit.transform.GetComponent<Tile>();

                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    _lastLeftTile = t; //Para saber a que tile empujar en esta direccion
                    return GetTileToPushTo(t, -leftHit.transform.right, count);
                }
                else if (!t.IsWalkable())
                {
                    collides = true;
                    collidingUnit = t.GetUnitAbove();
                    return currentTile;
                }
            }
        }
        return currentTile;
    }

    private void PaintUseTiles(Tile currentTile, int count, Vector3 dir)
    {
        if (count >= _abilityUseRange) //|| (_tilesInRangeChecked.ContainsKey(currentTile) && _tilesInRangeChecked[currentTile] <= count))
            return;

        _tilesInRange.Add(currentTile);
        count++;
        if (dir == Vector3.zero)
        {
            Debug.Log("comienzo");
            RaycastHit forwardHit;
            RaycastHit leftHit;
            RaycastHit rightHit;
            RaycastHit backHit;
    
            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.forward, out forwardHit);
            Physics.Raycast(position, currentTile.transform.right * -1, out leftHit);
            Physics.Raycast(position, currentTile.transform.right, out rightHit);
            Physics.Raycast(position, currentTile.transform.forward * -1, out backHit);
    
            if (forwardHit.transform)
            {
                Tile t = forwardHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, forwardHit.transform.forward);                    
                }
            }
            
            if (leftHit.transform)
            {
                Tile t = leftHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, leftHit.transform.right * -1);                    
                }
            }
            
            if (rightHit.transform)
            {
                Tile t = rightHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, rightHit.transform.right);                    
                }
            }
            
            if (backHit.transform)
            {
                Tile t = backHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    PaintUseTiles(t, count, backHit.transform.forward * -1);                    
                }
            }
        }

        if (dir == Vector3.forward)
        {
            Debug.Log("forward count: " + count);
            RaycastHit forwardHit;
            
            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.forward, out forwardHit);
            if (forwardHit.transform)
            {
                Tile t = forwardHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    _lastForwardTile = t;//Para saber a que tile empujar en esta direccion
                    PaintUseTiles(t, count, forwardHit.transform.forward);                    
                }
            }
            else return;
        }

        if (dir == Vector3.left)
        {
            RaycastHit leftHit;
            
            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.right * -1, out leftHit);
            
            if (leftHit.transform)
            {
                Tile t = leftHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    _lastLeftTile = t; //Para saber a que tile empujar en esta direccion
                    PaintUseTiles(t, count, leftHit.transform.right * -1);                    
                }
            }
            else return;
        }
        
        if (dir == Vector3.right)
        {
            RaycastHit rightHit;
            
            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.right * -1, out rightHit);
            
            if (rightHit.transform)
            {
                Tile t = rightHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    _lastRightTile = t;//Para saber a que tile empujar en esta direccion
                    PaintUseTiles(t, count, rightHit.transform.right);                    
                }
            }
            else return;
        }
        
        if (dir == Vector3.back)
        {
            RaycastHit backHit;
            
            var position = currentTile.transform.position;
            Physics.Raycast(position, currentTile.transform.forward * -1, out backHit);
            
            if (backHit.transform)
            {
                Tile t = backHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInActivationRange(t);
                    _lastBackTile = t;//Para saber a que tile empujar en esta direccion
                    PaintUseTiles(t, count, backHit.transform.forward * -1);                    
                }
            }
            else return;
        }
        


            // _tilesInRangeChecked[currentTile] = count;
        //
        // foreach (Tile tile in currentTile.allNeighbours)
        // {
        //     if (!_tilesInRange.Contains(tile))
        //     {
        //         if (!tile.HasTileAbove() && tile.IsWalkable())
        //         {
        //             _tilesInRange.Add(tile);
        //             tile.inAttackRange = true;
        //             _highlight.PaintTilesInPreviewRange(tile);
        //         }
        //
        //     }
        //     PaintUseTiles(tile, count + 1);
        // }
    }
}
