using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] AudioClip sound;

    public void PlayTheSound()
	{
		AudioManager.audioManagerInstance.PlaySound(sound, gameObject);
	}
}
