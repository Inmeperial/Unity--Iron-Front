using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TileHighlight : MonoBehaviour
{
    Transform previousTile;
    public LayerMask mask;
    // Update is called once per frame
    void Update()
    {
        if (previousTile != null)
        {
            MouseExitsTile();
            previousTile = null;
        }

        CheckTile();
    }

    void CheckTile()
    {
        var obj = MouseRay.GetTarget(mask);
        if (obj != null && obj.CompareTag("GridBlock"))
        {
            var tile = obj.gameObject.GetComponent<Tile>();
            if (tile.isWalkable)
            {
                MouseOverTile(tile);
            }
        }
    }

    public void MouseOverTile(Tile tile)
    {
        Material mat = new Material(tile.render.sharedMaterial);
        mat.color = Color.yellow;

        tile.render.sharedMaterial = mat;
        previousTile = tile.transform;
    }

    public void MouseExitsTile()
    {
        Tile tile = previousTile.gameObject.GetComponent<Tile>();
        Material mat = new Material(tile.render.sharedMaterial);
        mat.color = Color.green;

        tile.render.sharedMaterial = mat;
    }
}
