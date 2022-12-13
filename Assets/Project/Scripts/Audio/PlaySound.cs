using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] SoundData _clickSound;

    public void PlayTheSound()
	{
		AudioManager.Instance.PlaySound(_clickSound, gameObject);
	}
}
