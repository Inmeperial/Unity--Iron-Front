using System.Collections;
using UnityEngine;

public class RandomSoundsPlayer : MonoBehaviour
{
    [SerializeField] protected SoundData[] _sounds;

    protected virtual void Start()
    {
        if (_sounds.Length <= 0)
            return;

        StartCoroutine(SoundsUpdater());
    }

    protected virtual IEnumerator SoundsUpdater()
    {
        while (true)
        {
            SoundData sound = GetRandomSound();
            AudioManager.Instance.PlaySound(sound, gameObject);

            yield return new WaitForSeconds(sound.Clip.length);
        }
    }

    protected SoundData GetRandomSound()
    {
        int random = Random.Range(0, _sounds.Length);

        return _sounds[random];
    }
}
