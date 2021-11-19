using UnityEngine;


public class SoundsEnvironmentalFX : MonoBehaviour
{
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.audioManagerInstance.PlaySoundWithParameters(clip, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
