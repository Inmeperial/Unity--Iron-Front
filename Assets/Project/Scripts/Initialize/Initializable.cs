using UnityEngine;

public abstract class Initializable : MonoBehaviour
{
    [SerializeField] protected float _initializeDelaySeconds;
    public float InitializeDelaySeconds => _initializeDelaySeconds;
    public abstract void Initialize();
}
