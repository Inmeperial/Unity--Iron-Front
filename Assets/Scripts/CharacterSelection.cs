using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public LayerMask charMask;
    public Character _selection;
    TileHighlight _highlight;
    private void Start()
    {
        _highlight = GetComponent<TileHighlight>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SelectCharacter(charMask);
    }

    void SelectCharacter(LayerMask charMask)
    {
        var character = MouseRay.GetTarget(charMask);
        if (character != null && character.CompareTag("Character"))
        {
            Debug.Log("SELECCION");
            if (_selection != null)
            {
                _selection.DeselectThisUnit();
            }
            _selection = character.GetComponent<Character>();
            _selection.SelectThisUnit();
            _highlight.ChangeActiveCharacter(_selection);
        }
    }

    public Character GetActualChar()
    {
        return _selection;
    }
}
