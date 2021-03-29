using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TileHighlight : MonoBehaviour
{
    Character _character;
    Transform previousTile;
    public AStarAgent agent;
    public LayerMask mask;
    List<Tile> _previewPath = new List<Tile>();
    public bool characterMoving;
    CharacterSelection _charSelector;

    private void Start()
    {
        _charSelector = GetComponent<CharacterSelection>();
    }

    // Update is called once per frame
    void Update()
    {
        if (previousTile != null)
        {
            MouseExitsTile();
            if (!characterMoving)
                EndPreview();
        }

        if (!characterMoving)
            RayToTile();
    }

    //Check if mouse is over a tile.
    void RayToTile()
    {
        var obj = MouseRay.GetTargetTransform(mask);
        if (obj != null && obj.CompareTag("GridBlock"))
        {
            var tile = obj.gameObject.GetComponent<Tile>();
            if (tile.isWalkable)
            {
                MouseOverTile(tile);
            }
        }
    }

    //Change tile color.
    public void MouseOverTile(Tile tile)
    {
        Material mat = new Material(tile.render.sharedMaterial);
        mat.color = Color.yellow;

        tile.render.sharedMaterial = mat;
        previousTile = tile.transform;
        if (_character != null && _character.IsSelected() && !characterMoving)
        {
            PathPreview(_charSelector.GetActualChar());
        }
    }

    public void MouseExitsTile()
    {
        Tile tile = previousTile.gameObject.GetComponent<Tile>();
        Material mat = new Material(tile.render.sharedMaterial);
        mat.color = Color.green;

        tile.render.sharedMaterial = mat;
    }

    public void EndPreview()
    {
        previousTile = null;
        if (_previewPath.Count > 0)
        {
            foreach (var item in _previewPath)
            {
                item.EndPathfindingPreview();
            }
            _previewPath.Clear();
        }
    }
    public void PathPreview(Character character)
    {
        var start = character.ActualPosition();
        if (start)
        {
            agent.init = start;
            agent.finit = previousTile.GetComponent<Tile>();
            _previewPath = agent.PathFindingAstar();
            if (_previewPath.Count > 0)
            {
                foreach (var tile in _previewPath)
                {
                    if (tile.isWalkable)
                        tile.PathFindingPreviewColor();
                }
            }
        }
        
    }

    public void ChangeActiveCharacter(Character character)
    {
        this._character = character;
    }
}
