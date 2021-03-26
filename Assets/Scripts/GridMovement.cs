using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    List<Tile> _tilesList;
    AStarAgent _agent;
    int _tilesIndex;
    bool _moveVertical = false;
    public TileHighlight highlight;
    private void Start()
    {
        _agent = FindObjectOfType<AStarAgent>();
    }
    public void StartMovement(Tile start, Transform target, float speed)
    {
        _agent.init = start;
        _agent.finit = target.gameObject.GetComponent<Tile>();
        _tilesList = _agent.PathFindingAstar();
        if (_tilesList.Count > 0)
        {
            _tilesIndex = 1;
            highlight.characterMoving = true;
            StartCoroutine(Move(_tilesIndex, speed));
        }
        else Debug.Log("Can't reach tile");
    }

    IEnumerator Move(int tilesIndex, float speed)
    {
        if(transform.position != _tilesList[_tilesList.Count-1].transform.position)
        {
            var newPos = _tilesList[tilesIndex].transform.position;
            _moveVertical = false;
            newPos.y += 1;
            Debug.Log("Tile: " + newPos);
            if ((newPos - transform.position).magnitude <= 0.1f)
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
            else highlight.characterMoving = false;
        }
    }

    IEnumerator MoveVertical(float finalYPosition, float speed)
    {
        if (transform.position.y <= finalYPosition)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            MoveVertical(finalYPosition, speed);
        }
        else
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            MoveVertical(finalYPosition, speed);
        }
        _moveVertical = true;
    }
}
