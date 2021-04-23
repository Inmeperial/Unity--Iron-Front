using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    List<Tile> _tilesList;
    int _tilesIndex;
    bool _moveVertical = false;
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
            _moveVertical = false;
            newPos.y += _tilesList[tilesIndex].transform.localScale.y;
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
            else 
            {
                _character.ReachedEnd();
            }
        }
    }

    //IEnumerator MoveVertical(float finalYPosition, float speed)
    //{
    //    if (transform.position.y <= finalYPosition)
    //    {
    //        transform.position += Vector3.up * speed * Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //        MoveVertical(finalYPosition, speed);
    //    }
    //    else
    //    {
    //        transform.position += Vector3.down * speed * Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //        MoveVertical(finalYPosition, speed);
    //    }
    //    _moveVertical = true;
    //}
}
