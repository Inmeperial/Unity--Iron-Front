﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    List<Tile> _tilesList;
    int _tilesIndex;
    Character _character;
    private void Start()
    {
        _character = GetComponent<Character>();
    }
    public void StartMovement(List<Tile> tilesList, float speed)
    {
        _tilesList = tilesList;
        _tilesIndex = 1;
        StartCoroutine(Move(_tilesIndex, speed));
    }

    IEnumerator Move(int tilesIndex, float speed)
    {
        if(transform.position != _tilesList[_tilesList.Count-1].transform.position)
        {
            var newPos = _tilesList[tilesIndex].transform.position;
            newPos.y += transform.position.y;
            if ((newPos - transform.position).magnitude <= 1.25f)
            {
                transform.position = newPos;
                tilesIndex++;
            }
            else
            {
                var dir = (newPos - transform.position).normalized;
                transform.position += dir * speed * Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();

            if (tilesIndex < _tilesList.Count)
                StartCoroutine(Move(tilesIndex, speed));
            else 
            {
                _character.ReachedEnd();
            }
        }
    }
}
