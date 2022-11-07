using System.Collections;

public class RandomMusicPlayer : RandomSoundsPlayer
{
    protected override IEnumerator SoundsUpdater()
    {
        while (true)
        {
            SoundData soundData = GetRandomSound();

            AudioManager.Instance.PlaySound(soundData, gameObject);

            yield return soundData.Clip.length;
        }
    }
}
