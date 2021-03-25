using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    List<Tile> _tilesList;
    AStarAgent _agent;
    int _tilesIndex;
    bool _moveVertical = false;
    private void Start()
    {
        _agent = FindObjectOfType<AStarAgent>();
    }
    public void StartMovement(Tile start, Transform target, float speed)
    {
        _agent.init = start;
        _agent.finit = target.gameObject.GetComponent<Tile>();
        _tilesList = _agent.PathFindingAstar();
        _tilesIndex = 1;
        StartCoroutine(Move(_tilesIndex, speed));
    }

    IEnumerator Move(int tilesIndex, float speed)
    {
        if(transform.position != _tilesList[_tilesList.Count-1].transform.position)
        {
            var newPos = _tilesList[tilesIndex].transform.position;
            Debug.Log("nombre: " + _tilesList[tilesIndex].gameObject.name);
            if (transform.position.y == newPos.y)
            {
                Debug.Log("Y IGUAL");
                StartCoroutine(MoveVertical(transform.position.y + 1, speed));
                yield return new WaitUntil(() => _moveVertical);
            }
            else if (transform.position.y < newPos.y)
            {
                Debug.Log("Y MENOR");
                var dif = (newPos.y - transform.position.y) + 1;
                StartCoroutine(MoveVertical(transform.position.y + dif, speed));
                yield return new WaitUntil(() => _moveVertical);
            }
            else
            {
                Debug.Log("Y MAYOR");
                if (Mathf.Abs(transform.position.y - newPos.y) > 1)
                {
                    Debug.Log("Y MAYOR 2");
                    var dif = (transform.position.y - newPos.y) - 1;
                    StartCoroutine(MoveVertical(transform.position.y - dif, speed));
                    yield return new WaitUntil(() => _moveVertical);
                }
                
            }
            

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
        }
    }

    IEnumerator MoveVertical(float finalYPosition, float speed)
    {
        if (transform.position.y < finalYPosition)
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
