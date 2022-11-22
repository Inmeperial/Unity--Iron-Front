using UnityEngine;


public class SoundDataPlayer : MonoBehaviour
{
    [SerializeField] private SoundData _sound;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlaySound(_sound, gameObject);
    }
}
