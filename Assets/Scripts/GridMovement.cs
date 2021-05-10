using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    List<Tile> _tilesList;
    int _tilesIndex;
    Character _character;
    public float moveSpeed;
    public float rotationSpeed;
    private void Start()
    {
        _character = GetComponent<Character>();
        
    }
    public void StartMovement(List<Tile> tilesList)
    {
        _tilesList = tilesList;
        _tilesIndex = 1;
        StartCoroutine(Move(_tilesIndex));
    }

    public void StartRotation()
    {
        //StartCoroutine(Rotate());
    }

    IEnumerator Rotate(Vector3 pos)
    {
        var targetDir = (pos - transform.position).normalized;
        Debug.Log(targetDir);
        float interpolation = 1;
        //Debug.Log("Angle: " + Vector3.SignedAngle(transform.forward, pos - transform.position, transform.forward));
        //while(transform.forward != targetDir)
        //{
        //    interpolation -= Time.deltaTime /2;
        //    if (interpolation <= 0)
        //    {
        //        transform.forward = targetDir;
        //        break;
        //    }
        //    Vector3 newRot = Vector3.Lerp(targetDir, transform.forward, interpolation);
        //    transform.Rotate(0, newRot.x * rotationSpeed, 0);
        //    yield return new WaitForEndOfFrame();
        //}

        yield return null;
    }

    IEnumerator Move(int tilesIndex)
    {
        if(transform.position != _tilesList[_tilesList.Count-1].transform.position)
        {
            var newPos = _tilesList[tilesIndex].transform.position;
            newPos.y += transform.position.y;

            yield return StartCoroutine(Rotate(newPos));
            if ((newPos - transform.position).magnitude <= 1.25f)
            {
                transform.position = newPos;
                tilesIndex++;
            }
            else
            {
                var dir = (newPos - transform.position).normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();

            if (tilesIndex < _tilesList.Count)
                StartCoroutine(Move(tilesIndex));
            else 
            {
                _character.ReachedEnd();
            }
        }
    }
}
