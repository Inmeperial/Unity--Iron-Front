using UnityEngine;
using System.Collections;

public class RandomEnvironmentSoundPlayer : RandomSoundsPlayer
{
    [SerializeField] private float _updateSoundTime;
    protected override IEnumerator SoundsUpdater()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_updateSoundTime);

        while (true)
        {
            SoundData sound = GetRandomSound();

            AudioManager.Instance.PlaySound(sound, gameObject);

            yield return waitForSeconds;
        }
    }
}
