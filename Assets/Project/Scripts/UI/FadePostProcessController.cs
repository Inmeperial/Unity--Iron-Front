using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FadePostProcessController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _uiBlocker;
    private PostProcessVolume _transitionPPVolume;

    [Header("Configs")]
    [SerializeField] private float _transitionDuration;
    [SerializeField] private bool _curtainClosed;

    public Action OnTransitionFinished;

    // Start is called before the first frame update
    void Start()
    {
        PostProcessVolume[] pp = _camera.gameObject.GetComponents<PostProcessVolume>();

        foreach (PostProcessVolume item in pp)
        {
            if (!item.profile.GetSetting<TransitionPPSSettings>())
                continue;

            _transitionPPVolume = item;
            break;
        }

        _uiBlocker.SetActive(false);

        if (_curtainClosed)
        {
            _transitionPPVolume.weight = 1;
            StartFade();
        }
        else
            _transitionPPVolume.weight = 0;

        
    }

    public void StartFade()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        _uiBlocker.SetActive(true);
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        float time = 0;

        if (_curtainClosed)
            time = _transitionDuration;

        while (time <= _transitionDuration && time >= 0)
        {
            if (_curtainClosed)
                time -= Time.deltaTime;
            else
                time += Time.deltaTime;

            float lerpValue = Mathf.Lerp(0, 1, time / _transitionDuration);

            _transitionPPVolume.weight= lerpValue;

            yield return waitForEndOfFrame;
        }

        _curtainClosed = !_curtainClosed;

        _uiBlocker.SetActive(false);

        OnTransitionFinished?.Invoke();
    }
}
