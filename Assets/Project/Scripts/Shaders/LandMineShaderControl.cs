using UnityEngine;

public class LandMineShaderControl : MonoBehaviour
{
    private float _timer = 0f;
    private float _totalTimer = 0f;
    private GameObject _childObj;
    private Light _lightChild;

    // Start is called before the first frame update
    void Start()
    {
        _childObj = this.transform.GetChild(0).gameObject;
        
        _lightChild = _childObj.gameObject.GetComponent<Light>();
        _lightChild.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AnimationLight();
    }

    /// <summary>
    /// Animation for the light in the mine.
    /// </summary>
    public void AnimationLight()
    {
        if (_timer >= 360)
        {
            _timer = 0;
        }
        _timer += Time.deltaTime;
        _totalTimer = 7 + Mathf.Sin(Time.time) * 3;
        _lightChild.intensity = _totalTimer;
    }
}
