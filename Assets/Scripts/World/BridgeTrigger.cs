﻿using UnityEngine;

public class BridgeTrigger : MonoBehaviour
{
    [SerializeField] private Bridge _bridge;

    private void OnTriggerEnter(Collider other)
    {
        _bridge.StartMovement();
        gameObject.SetActive(false);
    }
}
