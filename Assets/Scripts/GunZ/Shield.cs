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
                var p = t[i].transform.position;
                p.y = prefabHeight;

                var go = Instantiate(interactionPrefabs, p, Quaternion.identity);
                var button = go.GetComponentInChildren<CustomButton>();
                button.OnLeftClick.AddListener(() =>Rotate(go.transform));
                _instantiated.Add(go);
            }

            _selected = true;
        }
        else
        {
            foreach (var prefab in _instantiated)
            {
                Destroy(prefab);
            }
            _instantiated.Clear();
            _selected = false;
        }
        
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
        _character.transform.LookAt(t.position);
        _character.DeactivateAttack();
    }
}