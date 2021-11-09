using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public LayerMask block;
    [SerializeField] private float _height;
    private float _startingHeight;
    [SerializeField] private float _movementDuration;

    private bool _active;

    private Tile _tileBelow;
    // Start is called before the first frame update
    void Start()
    {
        _startingHeight = transform.position.y;

        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, block);

        var tile = hit.collider.GetComponent<Tile>();
        if (tile)
        {
            _tileBelow = tile;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && _active)
        {
            StartCoroutine(Move(false));
            return;
        }
            
        if (Input.GetKeyDown(KeyCode.M) && !_active)
        {
            StartCoroutine(Move(true));
            return;
        }
    }

    IEnumerator Move(bool goUp)
    {
        Vector3 start = transform.position;
        Vector3 end = start;
        float time = 0;

        var character = _tileBelow.GetUnitAbove();
        if (character)
        {
            character.transform.parent = transform;
            if (goUp)
            {
                end.y = _height;
                _active = true;
                _tileBelow.RemoveFromNeighbour();
            }
            else
            {
                end.y = _startingHeight;
                _active = false;
                _tileBelow.AddToNeighbour();
            }

            yield return new WaitForSeconds(2);
        
            while (time <= _movementDuration)
            {
                time += Time.deltaTime;
                var normalizedTime = time / _movementDuration;
                transform.position = Vector3.Lerp(start, end, normalizedTime);
                yield return new WaitForEndOfFrame();
            }
            if (!_active)
                character.transform.parent = null;
        }
    }
}
