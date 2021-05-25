using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUI : MonoBehaviour
{
    [SerializeField] private GameObject _container;

    [SerializeField] private float _showDuration;
    
    [SerializeField] private Slider _bodySlider;
    [SerializeField] private Slider _leftArmSlider;
    [SerializeField] private Slider _rightArmSlider;
    [SerializeField] private Slider _legsSlider;
    [SerializeField] private GameObject _moveActionIcon;
    [SerializeField] private GameObject _attackActionIcon;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _container.SetActive(false);
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_container.activeInHierarchy)
        {
            _container.transform.LookAt(transform.position + _camera.transform.forward);
        }
    }

    public void SetWorldUIValues(float bodyCurr, float rArmCurr, float lArmCurr, float legsCurr, bool moveStatus, bool attackStatus)
    {
        SetBodySlider(bodyCurr);
        SetLeftArmSlider(lArmCurr);
        SetRightArmSlider(rArmCurr);
        SetLegsSlider(legsCurr);
        MoveActionIcon(moveStatus);
        AttackActionIcon(attackStatus);
    }

    public void DeactivateWorldUI()
    {
        _container.SetActive(false);
    }
    
    public void DeactivateWorldUIWithTimer()
    {
        StartCoroutine(DeactivateUI(_showDuration));
    }
    
    IEnumerator DeactivateUI(float timer)
    {
        yield return new WaitForSeconds(timer);
        _container.SetActive(false);
    }
    
    public void ContainerActivation(bool status)
    {
        _container.SetActive(status);
    }

   
    
    public void SetLimits(float bodyMax, float rArmMax, float lArmMax, float legsMax)
    {
        _bodySlider.maxValue = bodyMax;
        _bodySlider.minValue = 0;

        _rightArmSlider.maxValue = rArmMax;
        _rightArmSlider.minValue = 0;

        _leftArmSlider.maxValue = lArmMax;
        _leftArmSlider.minValue = 0;

        _legsSlider.maxValue = legsMax;
        _legsSlider.minValue = 0;
    }
    
   public void SetBodySlider(float quantity)
    {
        if (quantity < 0)
            _bodySlider.value = 0;
        else _bodySlider.value = quantity;
    }

   public void UpdateBodySlider(int damage)
   {
       StartCoroutine(UpdateBody(damage));
   }

   IEnumerator UpdateBody(int damage)
   {
       for (int i = 1; i <= damage; i++)
       {
           _bodySlider.value -= 1;
           yield return new WaitForEndOfFrame();
       }
       
       if (_bodySlider.value < 0)
           _bodySlider.value = 0;
   }
   

   public void SetLeftArmSlider(float quantity)
    {
        if (quantity < 0)
            _leftArmSlider.value = 0;
        else _leftArmSlider.value = quantity;
    }
    
    public void UpdateLeftArmSlider(int damage)
    {
        StartCoroutine(UpdateLeftArm(damage));
    }

    IEnumerator UpdateLeftArm(int damage)
    {
        for (int i = 1; i <= damage; i++)
        {
            _leftArmSlider.value -= 1;
            yield return new WaitForEndOfFrame();
        }
       
        if (_leftArmSlider.value < 0)
            _leftArmSlider.value = 0;
    }

    public void SetRightArmSlider(float quantity)
    {
        if (quantity < 0)
            _rightArmSlider.value = 0;
        else _rightArmSlider.value = quantity;
    }
    
    public void UpdateRightArmSlider(int damage)
    {
        StartCoroutine(UpdateRightArm(damage));
    }

    IEnumerator UpdateRightArm(int damage)
    {
        for (int i = 1; i <= damage; i++)
        {
            _rightArmSlider.value -= 1;
            yield return new WaitForEndOfFrame();
        }
       
        if (_rightArmSlider.value < 0)
            _rightArmSlider.value = 0;
    }

    public void SetLegsSlider(float quantity)
    {
        if (quantity < 0)
            _legsSlider.value = 0;
        else _legsSlider.value = quantity;
    }
    
    public void UpdateLegsSlider(int damage)
    {
        StartCoroutine(UpdateLegs(damage));
    }

    IEnumerator UpdateLegs(int damage)
    {
        for (int i = 1; i <= damage; i++)
        {
            _legsSlider.value -= 1;
            yield return new WaitForEndOfFrame();
        }
       
        if (_legsSlider.value < 0)
            _legsSlider.value = 0;
    }

    void MoveActionIcon(bool status)
    {
        _moveActionIcon.SetActive(status);
    }

    void AttackActionIcon(bool status)
    {
        _attackActionIcon.SetActive(status);
    }
}
