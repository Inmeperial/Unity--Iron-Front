using SceneReference;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission Data", menuName = "Scriptable Objects/Mission Data")]
public class MissionDataSO : ScriptableObject
{
    public ReferenceToScene mission;
    public Sprite image;
}