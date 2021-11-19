using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private float _movementDuration;

    [SerializeField] private Transform _limit;

    [SerializeField] private List<Tile> _bridgeTiles;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        foreach (var tile in _bridgeTiles)
        {
            tile.gameObject.SetActive(true);
            tile.RemoveFromNeighbour();
            tile.gameObject.SetActive(false);
        }
    }

    public void StartMovement()
    {
        Debug.Log("start movement");
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float time = 0;
        
        Debug.Log("move");
        while (time <= _movementDuration)
        {
            Debug.Log("moving");
            time += Time.deltaTime;
            var normalizedTime = time / _movementDuration;
            
            transform.position = Vector3.Lerp(_startPosition, _limit.position, normalizedTime);

            yield return new WaitForEndOfFrame();
        }

        foreach (var tile in _bridgeTiles)
        {
            tile.gameObject.SetActive(true);
            tile.AddToNeighbour();
        }
    }
}
