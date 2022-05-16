using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : WeaponAbility
{
    private int _abilityUseRange;
    private int _pushDistance;
    private TileHighlight _highlight;
    //private Dictionary<Tile, int> _tilesInRangeChecked = new Dictionary<Tile, int>();
    private HashSet<Tile> _tilesInRange = new HashSet<Tile>();
    bool collides;
    Character collidingUnit;
    
    private PushSO _abilityData;

    public override void Initialize(Character character, EquipableSO data, Location location)
    {
        base.Initialize(character, data, location);
	    
        _abilityData = data as PushSO;
    }
    
    public override void Select()
    {
        if (_inCooldown || !_character.CanAttack()) return;
        Debug.Log("pinto tiles push");
        if (!_highlight)
        {
            _highlight = FindObjectOfType<TileHighlight>();
        }
        _abilityUseRange = _abilityData.pushUseRange;
        _pushDistance = _abilityData.pushDistance;
        PaintUseTiles(_character.GetMyPositionTile(), 0, Vector3.zero);
        _character.EquipableSelectionState(true, this);
        _character.DeselectThisUnit();
    }

    public override void Deselect()
    {
        if(_tilesInRange.Count != 0)
            _highlight.MortarClearTilesInAttackRange(_tilesInRange);
        _tilesInRange.Clear();
        _character.EquipableSelectionState(false, null);
        _character.SelectThisUnit();
    }

    public override void Use(Action callback = null)
    {
        Debug.Log("Using Push");
		if (Input.GetMouseButtonDown(0))
		{
            Debug.Log("Pushing");
            var selectedTile = MouseRay.GetTargetTransform(_character.GetBlockLayerMask());
		
            if (!selectedTile) return;
		
            var tile = selectedTile.GetComponent<Tile>();

            if (!tile) return;
		
            if (!_tilesInRange.Contains(tile)) return;
            
            var selectedUnit = tile.GetUnitAbove();
            if (!selectedUnit) return;

            //Para que solo puedas empujar enemigos
            var unitTeam = selectedUnit.GetUnitTeam();
            if (unitTeam == EnumsClass.Team.Green) return;

            var tileToPushTo = GetTileToPushTo(tile, (selectedUnit.transform.position - _character.transform.position).normalized, 0);
            PushAction(tileToPushTo, selectedUnit);
            
            if (callback != null)
                callback();
            
            AbilityUsed(_abilityData);
            UpdateButtonText(_availableUses.ToString(), _abilityData);
            _button.interactable = false;
            Deselect();
        }

        if (Input.GetMouseButtonDown(1))
            Deselect();
    }

    /// <summary>
    /// Calls the push movement and does the damage
    /// </summary>
    /// <param name="tileToPush"> The tile the unit is pushed to</param>
    /// <param name="enemy"> The unit that is pushed</param>
    void PushAction(Tile tileBeignPushedTo, Character enemy)
	{
        //Hago un Lerp del enemy hacia el tileBeignPushedTo
        StartCoroutine(LerpPush(enemy.transform, tileBeignPushedTo.transform.position, _abilityData.pushLerpDuration));
        enemy.ChangeMyPosTile(tileBeignPushedTo);
        enemy.GetBody().TakeDamage(_abilityData.pushDamage);
        enemy.SetHurtAnimation();
        EffectsController.Instance.PlayParticlesEffect(enemy.GetBurningSpawner(), EnumsClass.ParticleActionType.Damage);
        if (collides)
		{
            enemy.SetHurtAnimation();
            enemy.GetBody().TakeDamage(_abilityData.collisionDamage);
            EffectsController.Instance.PlayParticlesEffect(enemy.GetBurningSpawner(), EnumsClass.ParticleActionType.Damage);

            if (collidingUnit)
            {
                //Hacer daño a la otra unidad
                collidingUnit.SetHurtAnimation();
                collidingUnit.GetBody().TakeDamage(_abilityData.collisionDamage);
                EffectsController.Instance.PlayParticlesEffect(collidingUnit.GetBurningSpawner(), EnumsClass.ParticleActionType.Damage);
            }
        }
        _character.DeactivateAttack();
	}

    IEnumerator LerpPush(Transform movingTarget, Vector3 targetPos, float duration)
	{
        float lerpTime = 0;
        Vector3 startPos = movingTarget.position;
        targetPos = targetPos + (Vector3.up * 4);
        while(lerpTime < duration)
		{
            movingTarget.position = Vector3.Lerp(startPos, targetPos, lerpTime / duration);
            lerpTime += Time.deltaTime;
            yield return null;
		}
        movingTarget.position = targetPos;
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

        RaycastHit nextHit;

        var position = currentTile.transform.position;
        Physics.Raycast(position, dir, out nextHit);
        if (nextHit.transform)
        {
            Tile t = nextHit.transform.GetComponent<Tile>();
            if (!t) return currentTile;

            if (t && t.IsWalkable() && t.IsFree())
            {
                return GetTileToPushTo(t, dir, count);
            }
            else if (!t.IsWalkable() || !t.IsFree())
            {
                collides = true;
                collidingUnit = t.GetUnitAbove();
                return currentTile;
            }
        }
        return currentTile;
    }

    private void PaintUseTiles(Tile currentTile, int count, Vector3 dir)
    {
        _tilesInRange.Add(currentTile);
        if (count >= _abilityUseRange) //|| (_tilesInRangeChecked.ContainsKey(currentTile) && _tilesInRangeChecked[currentTile] <= count))
            return;

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
                    _highlight.MortarPaintTilesInAttackRange(t);
                    PaintUseTiles(t, count, forwardHit.transform.forward);                    
                }
            }
            
            if (leftHit.transform)
            {
                Tile t = leftHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInAttackRange(t);
                    PaintUseTiles(t, count, leftHit.transform.right * -1);                    
                }
            }
            
            if (rightHit.transform)
            {
                Tile t = rightHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInAttackRange(t);
                    PaintUseTiles(t, count, rightHit.transform.right);                    
                }
            }
            
            if (backHit.transform)
            {
                Tile t = backHit.transform.GetComponent<Tile>();
    
                if (t && t.IsWalkable())
                {
                    _highlight.MortarPaintTilesInAttackRange(t);
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
                    _highlight.MortarPaintTilesInAttackRange(t);
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
                    _highlight.MortarPaintTilesInAttackRange(t);
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
                    _highlight.MortarPaintTilesInAttackRange(t);
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
                    _highlight.MortarPaintTilesInAttackRange(t);
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
