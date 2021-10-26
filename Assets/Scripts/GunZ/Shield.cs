using UnityEngine;
using System.Collections.Generic;
public class Shield : Gun
{
    public GameObject interactionPrefabs;
    public float prefabHeight;
    private bool _selected = false;
    private List<GameObject> _instantiated = new List<GameObject>();
    public override void SetGun(GunSO data, Character character)
    {
        _gunType = GunsType.Shield;
        _gun = "Shield";
        base.SetGun(data, character);
    }

    public override void Ability()
    {
        if (_selected) return;
        
        List<Tile> t = _myChar.GetTileBelow().allNeighbours;

        for (int i = 0; i < t.Count; i++)
        {
            Transform tr = t[i].transform;
            Vector3 pos = tr.position;
            pos.y += prefabHeight;

            GameObject go = Instantiate(interactionPrefabs, pos, Quaternion.identity);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.Rotate(rt.right, 90f);
            rt.Rotate(rt.up, 90f * -i);
            CustomButton button = go.GetComponentInChildren<CustomButton>();
            button.OnLeftClick.AddListener(() => Rotate(go.transform));
            _instantiated.Add(go);
        }

        _selected = true;
    }

    public override void Deselect()
    {
        _selected = false;
        foreach (GameObject prefab in _instantiated)
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

    private void Rotate(Transform t)
    {
        Vector3 position = _myChar.transform.position;
        Vector3 shieldCharVector = (transform.position - position).normalized;
        Vector3 buttonCharVector = (t.position - position).normalized;
        
        float angle = Vector3.SignedAngle(shieldCharVector, buttonCharVector, Vector3.up);

        if (angle >= 90 && angle < 180)
            angle = 90;
        else if (angle >= 180 && angle < 270)
            angle = 180;
        else if (angle >= -90 && angle < 0)
            angle = -90;
        else if (angle >= -180 && angle < -90)
            angle = -180;
        else return;
        
        _myChar.transform.RotateAround(_myChar.transform.position, _myChar.transform.up, angle);
        foreach (GameObject prefab in _instantiated)
        {
            Destroy(prefab);
        }
        
        _instantiated.Clear();
        _selected = false;
        _myChar.DeactivateAttack();
    }
}