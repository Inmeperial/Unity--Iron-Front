using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WorldUI : MonoBehaviour
{
    #region Fields
    [Header("Status")]
    [SerializeField] private GameObject _statusContainer;
    [SerializeField] private float _showDuration;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private GameObject _overweightIcon;
    
    [Header("Body")]
    [SerializeField] private Slider _bodyHpSlider;
    [SerializeField] private Slider _bodyDamageSlider;

    [Header("Left Arm")]
    [SerializeField] private Slider _leftArmHpSlider;
    [SerializeField] private Slider _leftArmDamageSlider;

    [Header("Right Arm")]
    [SerializeField] private Slider _rightArmHpSlider;
    [SerializeField] private Slider _rightArmDamageSlider;

    [Header("Legs")]
    [SerializeField] private Slider _legsHpSlider;
    [SerializeField] private Slider _legsDamageSlider;

    [Header("Actions")]
    [SerializeField] private GameObject _moveActionIcon;
    [SerializeField] private GameObject _attackActionIcon;

    [Header("Buttons")] 
    [SerializeField] private float _forwardMultiplier;
    [SerializeField] private GameObject _buttonsContainer;

    [SerializeField] private MechaPartButton _bodyButton;
    [SerializeField] private MechaPartButton _leftArmButton;
    [SerializeField] private MechaPartButton _rightArmButton;
    [SerializeField] private MechaPartButton _legsButton;
    #endregion

    private bool _isToggledOn;
    public bool IsToggledOn => _isToggledOn;

    private bool _isActive;
    public bool IsActive => _isActive;


    // Start is called before the first frame update
    private void Start()
    {
        Hide();
        ButtonsContainerSetActive(false);
    }

    public void SetWorldUIValues(float bodyCurr, float rArmCurr, float lArmCurr, float legsCurr, bool moveStatus, bool attackStatus, bool overweightStatus)
    {
        SetBodySlider(bodyCurr);
        SetLeftArmSlider(lArmCurr);
        SetRightArmSlider(rArmCurr);
        SetLegsSlider(legsCurr);
        MoveActionIcon(moveStatus);
        AttackActionIcon(attackStatus);
        OverweightIcon(overweightStatus);
    }
    
    public void Hide()
    {
        _isActive = false;
        _statusContainer.SetActive(false);
    }

    public void HideWithTimer() => StartCoroutine(DeactivateUI(_showDuration));

    IEnumerator DeactivateUI(float timer)
    {
        yield return new WaitForSeconds(timer);
        _statusContainer.SetActive(false);
    }
    
    public void Show()
    {
        _isActive = true;
        _statusContainer.SetActive(true);
    }
    
    public void SetLimits(float bodyMax, float rArmMax, float lArmMax, float legsMax)
    {
        
        _bodyHpSlider.maxValue = bodyMax;
        _bodyHpSlider.minValue = 0;
        _bodyDamageSlider.maxValue = bodyMax;
        _bodyDamageSlider.minValue = 0;
        //_bodyButton.SetSlider(0, bodyMax);

        _rightArmHpSlider.maxValue = rArmMax;
        _rightArmHpSlider.minValue = 0;
        _rightArmDamageSlider.maxValue = rArmMax;
        _rightArmDamageSlider.minValue = 0;
        //_rightArmButton.SetSlider(0, rArmMax);

        _leftArmHpSlider.maxValue = lArmMax;
        _leftArmHpSlider.minValue = 0;
        _leftArmDamageSlider.maxValue = lArmMax;
        _leftArmDamageSlider.minValue = 0;
        //_leftArmButton.SetSlider(0, lArmMax);

        _legsHpSlider.maxValue = legsMax;
        _legsHpSlider.minValue = 0;
        _legsDamageSlider.maxValue = legsMax;
        _legsDamageSlider.minValue = 0;
        //_legsButton.SetSlider(0, legsMax);
    }

    public void Toggle(bool state) => _isToggledOn = state;

    //TODO: Revisar Sliders
    #region WorldCanvas
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
   
   public void UpdateBodySlider(float damage, float currentHp)
   {
       _bodyHpSlider.value = currentHp;
       StartCoroutine(UpdateBody(damage));
   }

   IEnumerator UpdateBody(float damage)
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
   
    public void UpdateLeftArmSlider(float damage, float currentHp)
    {
        _leftArmHpSlider.value = currentHp;
        StartCoroutine(UpdateLeftArm(damage));
    }

    IEnumerator UpdateLeftArm(float damage)
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
    
    public void UpdateRightArmSlider(float damage, float currentHp)
    {
        _rightArmHpSlider.value = currentHp;
        StartCoroutine(UpdateRightArm(damage));
    }

    IEnumerator UpdateRightArm(float damage)
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
    
    public void UpdateLegsSlider(float damage, float currentHp)
    {
        _legsHpSlider.value = currentHp;
        StartCoroutine(UpdateLegs(damage));
    }

    IEnumerator UpdateLegs(float damage)
    {
        for (int i = 1; i <= damage; i++)
        {
            _legsDamageSlider.value -= 1;
            yield return new WaitForEndOfFrame();
        }
       
        if (_legsDamageSlider.value < 0)
            _legsDamageSlider.value = 0;
    }

    private void MoveActionIcon(bool status) => _moveActionIcon.SetActive(status);

    private void AttackActionIcon(bool status) => _attackActionIcon.SetActive(status);

    public void OverweightIcon(bool status) => _overweightIcon.SetActive(status);

    public void SetName(string name) => _nameText.text = name;
    #endregion

    //Activa el contenedor de los botones
    public void ButtonsContainerSetActive(bool status) => _buttonsContainer.SetActive(status);

}
