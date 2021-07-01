using UnityEngine;
using System.Collections.Generic;
public class Shield : Gun
{
    public GameObject interactionPrefabs;
    public float prefabHeight;
    private Character _character;
    private bool _selected = false;
    private List<GameObject> _instantiated = new List<GameObject>();
    public override void SetGun()
    {
        base.SetGun();
        _character = GetCharacter(transform);
    }

    public override void Ability()
    {
        if (!_selected)
        {
            var t = _character.GetTileBelow().allNeighbours;

            for (int i = 0; i < t.Count; i++)
            {
                var tr = t[i].transform;
                var pos = tr.position;
                pos.y += prefabHeight;

                var go = Instantiate(interactionPrefabs, pos, Quaternion.identity);
                var rt = go.GetComponent<RectTransform>();
                rt.Rotate(rt.right, 90f);
                rt.Rotate(rt.up, 90f * -i);
                var button = go.GetComponentInChildren<CustomButton>();
                button.OnLeftClick.AddListener(() => Rotate(go.transform));
                _instantiated.Add(go);
            }

            _selected = true;
        }
    }

    public override void Deselect()
    {
        _selected = false;
        foreach (var prefab in _instantiated)
        {
            Destroy(prefab);
        }
        _instantiated.Clear();
    }

    private Character GetCharacter(Transform parent)
    {
        while (true)
        {
            if (parent == null) return null;

            var character = parent.GetComponent<Character>();

            if (character) return character;

            parent = parent.parent;
        }
    }

    public void Rotate(Transform t)
    {
        var position = _character.transform.position;
        var shieldCharVector = (transform.position - position).normalized;
        var buttonCharVector = (t.position - position).normalized;
        
        var angle = Vector3.SignedAngle(shieldCharVector, buttonCharVector, Vector3.up);

        if (angle >= 90 && angle < 180)
            angle = 90;
        else if (angle >= 180 && angle < 270)
            angle = 180;
        else if (angle >= -90 && angle < 0)
            angle = -90;
        else if (angle >= -180 && angle < -90)
            angle = -180;
        else return;
        
        _character.transform.RotateAround(_character.transform.position, _character.transform.up, angle);
        foreach (var prefab in _instantiated)
        {
            Destroy(prefab);
        }
        
        _instantiated.Clear();
        _selected = false;
        _character.DeactivateAttack();
    }
}