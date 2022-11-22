using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] AudioClip sound;

    public void PlayTheSound()
	{
		//AudioManager.Instance.PlaySound(sound, gameObject);
	}
}
