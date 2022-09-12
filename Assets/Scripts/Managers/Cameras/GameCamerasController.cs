using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamerasController : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CloseUpCamera _closeUpCamera;
    [SerializeField] private CameraMovement _cameraMovement;

    public CloseUpCamera CloseUpCamera => _closeUpCamera;

    public CameraMovement CameraMovement => _cameraMovement;
}
