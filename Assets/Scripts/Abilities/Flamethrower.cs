using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Ability
{
    [SerializeField]private int damage;
	[SerializeField]private float range;
	[SerializeField]private float angle;
    [SerializeField] private LayerMask gridMask;
    [SerializeField] private LayerMask characterMask;
    private TileHighlight _highlight;
    private Vector3 _position;
    private Vector3 _debbugDir;
    private Vector3 _mouseDir;
    private Vector3 _facingDir;
    private Camera _mainCam;
    private HashSet<Tile> _tilesInRange = new HashSet<Tile>();

    public override void Select()
	{
        if (InCooldown() || !_character.CanAttack()) return;
        if (!_highlight)
        {
            _highlight = FindObjectOfType<TileHighlight>();
        }
        _mainCam = Camera.main;
        _position = _character.transform.position;
        _character.EquipableSelectionState(true, this);
        _character.DeselectThisUnit();
    }

	public override void Deselect()
	{
        _highlight.MortarClearTilesInAttackRange(_tilesInRange);
        _tilesInRange.Clear();
        _character.EquipableSelectionState(false, null);
        _character.SelectThisUnit();
    }

	public override void Use(Action callback = null)
	{
        //Consigo las direcciones
        Ray ray = _mainCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
		{
            _mouseDir = raycastHit.point;
		}
        _facingDir = (_mouseDir - _position);
        
        //Pinto los tiles que esten en el area de ataque
        var tiles = Physics.OverlapSphere(_position, range, gridMask);
        foreach (var item in tiles)
        {
            var tempTile = item.GetComponent<Tile>();
            if (!tempTile) continue;
            if (Vector3.Angle(_facingDir, (item.transform.position - _position)) > angle / 2)
            {
                PaintAndClearTile(tempTile);
                continue;
            }
            Debug.Log("Painting Tile");
            PaintAndClearTile(tempTile);
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X))
		{
            //Ataco a todas las unidades que esten en el area de ataque
            List<Character> charactersHitted = new List<Character>();
            var collisions = Physics.OverlapSphere(_position, range, characterMask);
            foreach (var item in collisions)
            {
                if (Vector3.Angle(_facingDir, (item.transform.position - _position)) > angle / 2) continue;
                var tempChar = item.GetComponentInParent<Character>();
                if (!tempChar || charactersHitted.Contains(tempChar)) continue;
                charactersHitted.Add(tempChar);
            }
            DamageCharacters(charactersHitted);
            if (callback != null)
                callback();
            Deselect();
        }

        if (Input.GetMouseButtonDown(1))
		{
            if (callback != null)
                callback();
            Deselect();
		}
	}

    void DamageCharacters(List<Character> charactersToDamage)
    {
        Debug.Log("Flamethrower damage");
        foreach (var item in charactersToDamage)
        {
            if (item == _character) continue;
            item.GetBody().TakeDamage(damage);
            item.SetHurtAnimation();
        }
        _character.DeactivateAttack();
    }

    void PaintAndClearTile(Tile tileToChange)
	{
        if (!_tilesInRange.Contains(tileToChange))
        {
            _tilesInRange.Add(tileToChange);
            _highlight.MortarPaintTilesInAttackRange(tileToChange);
        }
		else
		{
            _tilesInRange.Remove(tileToChange);
            tileToChange.MortarEndCanBeAttackedColor();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_position, _facingDir.normalized * range);
        Gizmos.DrawWireSphere(_position, range);
        Gizmos.DrawRay(_position, Quaternion.Euler(0, angle / 2, 0) * _facingDir.normalized * range);
        Gizmos.DrawRay(_position, Quaternion.Euler(0, -angle / 2, 0) * _facingDir.normalized * range);
    }
}
