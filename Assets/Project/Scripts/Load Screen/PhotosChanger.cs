using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotosChanger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _image;
    [SerializeField] private Sprite[] _photos;

    [Header("Configs")]
    [SerializeField] private float _timeBetweenPhoto;
    [SerializeField] private float _transitionTime;

    // Start is called before the first frame update
    void Start()
    {
        SetRandomPhoto();

        StartCoroutine(UpdatePhotos());
    }

    private void SetRandomPhoto()
    {
        int random = Random.Range(0, _photos.Length);
        
        _image.sprite = _photos[random];
    }

    private IEnumerator UpdatePhotos()
    {
        WaitForSeconds waitBetweenPhoto = new WaitForSeconds(_timeBetweenPhoto);

        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        while (true)
        {
            yield return waitBetweenPhoto;

            float time = _transitionTime;

            Color color = _image.color;

            while (time > 0)
            {
                time -= Time.deltaTime;

                float lerpValue = Mathf.Lerp(0, 1, time / _transitionTime);

                color.r = lerpValue;
                color.g = lerpValue;
                color.b = lerpValue;

                _image.color = color;

                yield return waitForEndOfFrame;
            }

            SetRandomPhoto();

            while (time < _transitionTime)
            {
                time += Time.deltaTime;

                float lerpValue = Mathf.Lerp(0, 1, time / _transitionTime);

                color.r = lerpValue;
                color.g = lerpValue;
                color.b = lerpValue;

                _image.color = color;

                yield return waitForEndOfFrame;
            }
        }
    }
}
