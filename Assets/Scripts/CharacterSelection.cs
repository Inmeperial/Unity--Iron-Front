using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public LayerMask charMask;
    public Character _selection;
    TileHighlight _highlight;
    public bool _canSelect;
    private void Start()
    {
        _canSelect = true;
        _highlight = GetComponent<TileHighlight>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canSelect)
            SelectCharacter(charMask);
    }

    //Selection of the character that will move.
    void SelectCharacter(LayerMask charMask)
    {
        var character = MouseRay.GetTargetTransform(charMask);
        if (character != null && character.CompareTag("Character"))
        {
            //Check if i have a previous unit and deselect it.
            if (_selection != null)
            {
                _selection.DeselectThisUnit();
            }

            _selection = character.GetComponent<Character>();
            _selection.SelectThisUnit();
            _highlight.ChangeActiveCharacter(_selection);
        }
    }

    //Returns the character that is currently selected.
    public Character GetActualChar()
    {
        return _selection;
    }

    public void ActivateCharacterSelection(bool state)
    {
        _canSelect = state;
    }
}
