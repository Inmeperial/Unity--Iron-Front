using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionProjector : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _transitionDuration;
    [SerializeField] private SoundData _cardOutSound;
    [SerializeField] private SoundData _cardInSound;

    private Vector2 _origin;
    private void Awake()
    {

        _origin = _image.rectTransform.anchoredPosition;
    }
    public void ChangeMissionImage(MissionDataSO missionData)
    {
        StopAllCoroutines();

        StartCoroutine(Transition(missionData.image));
    }

    private IEnumerator Transition(Sprite image)
    {
        float time = 0f;

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        Vector2 pos = new Vector2(0, _image.rectTransform.anchoredPosition.y);

        float finalX = _origin.x - image.rect.width * 2;

        AudioManager.Instance.PlaySound(_cardOutSound, gameObject);

        while (time < _transitionDuration)
        {
            pos.x = Mathf.Lerp(_image.rectTransform.anchoredPosition.x, finalX, time / _transitionDuration);

            _image.rectTransform.anchoredPosition = pos;

            time += Time.deltaTime;

            yield return waitForEndOfFrame;
        }

        _image.sprite = image;

        time = 0f;

        AudioManager.Instance.PlaySound(_cardInSound, gameObject);

        while (time < _transitionDuration)
        {
            pos.x = Mathf.Lerp(_image.rectTransform.anchoredPosition.x, _origin.x, time / _transitionDuration);

            _image.rectTransform.anchoredPosition = pos;

            time += Time.deltaTime;

            yield return waitForEndOfFrame;
        }
    }
}
