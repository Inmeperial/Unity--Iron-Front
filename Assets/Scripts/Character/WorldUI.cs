using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CanEditMultipleObjects]
public class WorldUI : MonoBehaviour
{
    [SerializeField] private GameObject _statusContainer;
    [SerializeField] private GameObject _buttonsContainer;

    [SerializeField] private float _showDuration;
    
    [SerializeField] private Slider _bodyHpSlider;
    [SerializeField] private Slider _bodyDamageSlider;
    [SerializeField] private Slider _leftArmHpSlider;
    [SerializeField] private Slider _leftArmDamageSlider;
    [SerializeField] private Slider _rightArmHpSlider;
    [SerializeField] private Slider _rightArmDamageSlider;
    [SerializeField] private Slider _legsHpSlider;
    [SerializeField] private Slider _legsDamageSlider;
    [SerializeField] private GameObject _moveActionIcon;
    [SerializeField] private GameObject _attackActionIcon;
    
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _statusContainer.SetActive(false);
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_statusContainer.activeInHierarchy)
        {
            _statusContainer.transform.LookAt(transform.position + _camera.transform.forward);
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
        _statusContainer.SetActive(false);
    }
    
    public void DeactivateWorldUIWithTimer()
    {
        StartCoroutine(DeactivateUI(_showDuration));
    }
    
    IEnumerator DeactivateUI(float timer)
    {
        yield return new WaitForSeconds(timer);
        _statusContainer.SetActive(false);
    }
    
    public void ContainerActivation(bool status)
    {
        _statusContainer.SetActive(status);
    }

   
    
    public void SetLimits(float bodyMax, float rArmMax, float lArmMax, float legsMax)
    {
        _bodyHpSlider.maxValue = bodyMax;
        _bodyHpSlider.minValue = 0;
        _bodyDamageSlider.maxValue = bodyMax;
        _bodyDamageSlider.minValue = 0;

        _rightArmHpSlider.maxValue = rArmMax;
        _rightArmHpSlider.minValue = 0;
        _rightArmDamageSlider.maxValue = rArmMax;
        _rightArmDamageSlider.minValue = 0;

        _leftArmHpSlider.maxValue = lArmMax;
        _leftArmHpSlider.minValue = 0;
        _leftArmDamageSlider.maxValue = lArmMax;
        _leftArmDamageSlider.minValue = 0;

        _legsHpSlider.maxValue = legsMax;
        _legsHpSlider.minValue = 0;
        _legsDamageSlider.maxValue = legsMax;
        _legsDamageSlider.minValue = 0;
    }
    
   public void SetBodySlider(float quantity)
    {
        if (quantity < 0)
        {
            _bodyHpSlider.value = 0;
            _bodyDamageSlider.value = 0;
        }

        else
        {
            _bodyHpSlider.value = quantity;
            _bodyDamageSlider.value = quantity;
        }
    }

   public void UpdateBodySlider(int damage, int currentHp)
   {
       _bodyHpSlider.value = currentHp;
       StartCoroutine(UpdateBody(damage));
   }

   IEnumerator UpdateBody(int damage)
   {
       for (int i = 1; i <= damage; i++)
       {
           _bodyDamageSlider.value -= 1;
           yield return new WaitForEndOfFrame();
       }
       
       if (_bodyDamageSlider.value < 0)
           _bodyDamageSlider.value = 0;
   }
   

   public void SetLeftArmSlider(float quantity)
    {
        if (quantity < 0)
        {
            _leftArmHpSlider.value = 0;
            _leftArmDamageSlider.value = 0;
        }

        else
        {
            _leftArmHpSlider.value = quantity;
            _leftArmDamageSlider.value = quantity;
        }
    }
    
    public void UpdateLeftArmSlider(int damage, int currentHp)
    {
        _leftArmHpSlider.value = currentHp;
        StartCoroutine(UpdateLeftArm(damage));
    }

    IEnumerator UpdateLeftArm(int damage)
    {
        for (int i = 1; i <= damage; i++)
        {
            _leftArmDamageSlider.value -= 1;
            yield return new WaitForEndOfFrame();
        }
       
        if (_leftArmDamageSlider.value < 0)
            _leftArmDamageSlider.value = 0;
    }

    public void SetRightArmSlider(float quantity)
    {
        if (quantity < 0)
        {
            _rightArmHpSlider.value = 0;
            _rightArmDamageSlider.value = 0;
        }

        else
        {
            _rightArmHpSlider.value = quantity;
            _rightArmDamageSlider.value = quantity;
        }
    }
    
    public void UpdateRightArmSlider(int damage, int currentHp)
    {
        _rightArmHpSlider.value = currentHp;
        StartCoroutine(UpdateRightArm(damage));
    }

    IEnumerator UpdateRightArm(int damage)
    {
        for (int i = 1; i <= damage; i++)
        {
            _rightArmDamageSlider.value -= 1;
            yield return new WaitForEndOfFrame();
        }
       
        if (_rightArmDamageSlider.value < 0)
            _rightArmDamageSlider.value = 0;
    }

    public void SetLegsSlider(float quantity)
    {
        if (quantity < 0)
        {
            _legsHpSlider.value = 0;
            _legsDamageSlider.value = 0;
        }

        else
        {
            _legsHpSlider.value = quantity;
            _legsDamageSlider.value = quantity;
        }
    }
    
    public void UpdateLegsSlider(int damage, int currentHp)
    {
        _legsHpSlider.value = currentHp;
        StartCoroutine(UpdateLegs(damage));
    }

    IEnumerator UpdateLegs(int damage)
    {
        for (int i = 1; i <= damage; i++)
        {
            _legsDamageSlider.value -= 1;
            yield return new WaitForEndOfFrame();
        }
       
        if (_legsDamageSlider.value < 0)
            _legsDamageSlider.value = 0;
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
