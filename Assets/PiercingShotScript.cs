using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingShotScript : MonoBehaviour
{
    public GameObject startPos, endPos, effectObj;
    public float movementSpeed = 0;
    private bool startMovement, effectStarted, effectEnded = false;

    void Start()
    {
        ReStartEffect();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReStartEffect();
        }

        if (!startMovement)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startMovement = true;
            }
        }

        if (startMovement)
        {
            if (!effectStarted)
            {
                StartEffect();
            }
            effectObj.transform.position += Vector3.forward * Time.deltaTime * movementSpeed;
        }

        if (effectObj.transform.position.z >= endPos.transform.position.z)
        {
            if (!effectEnded)
            {
                EndEffect();
            }            
        }
    }

    private void StartEffect()
    {
        effectStarted = true;
        foreach (Transform child in effectObj.transform)
        {
            child.gameObject.GetComponent<ParticleSystem>().Play();
        }

    }

    private void EndEffect()
    {
        effectObj.transform.position = endPos.transform.position;
        effectEnded = true;
        startMovement = false;
        foreach (Transform child in effectObj.transform)
        {
            child.gameObject.GetComponent<ParticleSystem>().Stop();
        }
    }

    private void ReStartEffect()
    {
        EndEffect();
        startMovement = false;
        effectStarted = false;
        effectEnded = false;
        effectObj.transform.LookAt(endPos.transform.position);
        effectObj.transform.position = startPos.transform.position;
    }
}
