using System.Collections;
using UnityEngine;

public class RandomPositionEnvironmentalSoundsPlayer : RandomSoundsPlayer
{
    [SerializeField] private float _minWaitTimeBetweenSounds = 0f;
    [SerializeField] private float _maxWaitTimeBetweenSounds = 6f;

    protected override IEnumerator SoundsUpdater()
    {
        while (true)
        {
            SetRandomPosition();
            AudioManager.Instance.PlaySound(GetRandomSound(), gameObject);

            yield return new WaitForSeconds(Random.Range(_minWaitTimeBetweenSounds, _maxWaitTimeBetweenSounds));
        }
    }
    private void SetRandomPosition()
    {
        transform.position = new Vector3(Random.Range(0f, 133), 0, Random.Range(0f, 133));
    }
}