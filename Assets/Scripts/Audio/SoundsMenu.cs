using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsMenu : MonoBehaviour
{
    [SerializeField]
    private AudioClip _clickSound;

    /*
     * TO-DO:
     *  Add sound for selection of broken weapon.
     */
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip GetClickSound()
    {
        return _clickSound;
    }
    public GameObject GetObjectToAddAudioSource()
    {
        return this.gameObject;
    }

}
