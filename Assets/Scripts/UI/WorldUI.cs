using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WorldUI : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] private GameObject _statusContainer;
    [SerializeField] private float _showDuration;
    
    [Header("Body")]
    [SerializeField] private TextMeshProUGUI _bodyHpText;
    [SerializeField] private Slider _bodyHpSlider;
    [SerializeField] private Slider _bodyDamageSlider;
    [SerializeField] private TextMeshProUGUI _bodyCount;
    
    [Header("Left Arm")]
    [SerializeField] private TextMeshProUGUI _leftArmHpText;
    [SerializeField] private Slider _leftArmHpSlider;
    [SerializeField] private Slider _leftArmDamageSlider;
    [SerializeField] private TextMeshProUGUI _leftArmCount;
    
    [Header("Right Arm")]
    [SerializeField] private TextMeshProUGUI _rightArmHpText;
    [SerializeField] private Slider _rightArmHpSlider;
    [SerializeField] private Slider _rightArmDamageSlider;
    [SerializeField] private TextMeshProUGUI _rightArmCount;
    
    [Header("Legs")]
    [SerializeField] private TextMeshProUGUI _legsHpText;
    [SerializeField] private Slider _legsHpSlider;
    [SerializeField] private Slider _legsDamageSlider;
    [SerializeField] private TextMeshProUGUI _legsCount;
    
    [Header("Actions")]
    [SerializeField] private GameObject _moveActionIcon;
    [SerializeField] private GameObject _attackActionIcon;

    [Header("Buttons")] 
    [SerializeField] private float _forwardMultiplier;
    [SerializeField] private GameObject _buttonsContainer;

    [SerializeField] private CustomButton _bodyButton;
    [SerializeField] private Slider _bodyButtonSlider;
    [SerializeField] private CustomButton _leftArmButton;
    [SerializeField] private Slider _leftArmButtonSlider;
    [SerializeField] private CustomButton _rightArmButton;
    [SerializeField] private Slider _rightArmButtonSlider;
    [SerializeField] private CustomButton _legsButton;
    [SerializeField] private Slider _legsButtonSlider;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _statusContainer.SetActive(false);
        _camera = Camera.main;
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

    //Desactiva el contenedor de estado
    public void DeactivateWorldUI()
    {
        _statusContainer.SetActive(false);
    }
    
    //Desactiva el contenedor de estado luego de cierto tiempo
    public void DeactivateWorldUIWithTimer()
    {
        StartCoroutine(DeactivateUI(_showDuration));
    }
    
    IEnumerator DeactivateUI(float timer)
    {
        yield return new WaitForSeconds(timer);
        _statusContainer.SetActive(false);
    }
    
    //Activa el contenedor de estado
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
        _bodyButtonSlider.maxValue = bodyMax;
        _bodyButtonSlider.minValue = 0;

        _rightArmHpSlider.maxValue = rArmMax;
        _rightArmHpSlider.minValue = 0;
        _rightArmDamageSlider.maxValue = rArmMax;
        _rightArmDamageSlider.minValue = 0;
        _rightArmButtonSlider.maxValue = rArmMax;
        _rightArmButtonSlider.minValue = 0;

        _leftArmHpSlider.maxValue = lArmMax;
        _leftArmHpSlider.minValue = 0;
        _leftArmDamageSlider.maxValue = lArmMax;
        _leftArmDamageSlider.minValue = 0;
        _leftArmButtonSlider.maxValue = lArmMax;
        _leftArmButtonSlider.minValue = 0;

        _legsHpSlider.maxValue = legsMax;
        _legsHpSlider.minValue = 0;
        _legsDamageSlider.maxValue = legsMax;
        _legsDamageSlider.minValue = 0;
        _legsButtonSlider.maxValue = legsMax;
        _legsButtonSlider.minValue = 0;
    }
    
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

   
   //Actualiza la barra de vida en cada frame en base al daño recibido
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
    
   //Actualiza la barra de vida en cada frame en base al daño recibido
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
    
    //Actualiza la barra de vida en cada frame en base al daño recibido
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
    
    //Actualiza la barra de vida en cada frame en base al daño recibido
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

    //Estado del icono de mover
    void MoveActionIcon(bool status)
    {
        _moveActionIcon.SetActive(status);
    }
    
    //Estado del icono de atacar
    void AttackActionIcon(bool status)
    {
        _attackActionIcon.SetActive(status);
    }
#endregion

#region WorldButtons

    //Activa el contenedor de los botones
    public void ButtonsContainerSetActive(bool status)
    {
        _buttonsContainer.SetActive(status);
    }

    //Activa el objeto de los botones y su interaccion
    public void ButtonsEnabling(bool body, bool leftArm, bool rightArm, bool legs)
    {
        BodyEnabling(body);
        LeftArmEnabling(leftArm);
        RightArmEnabling(rightArm);
        LegsEnabling(legs);
    }

    
    public void BodyEnabling(bool status)
    {
        //_bodyButton.transform.position += -_bodyButton.transform.forward * _forwardMultiplier; 
        _bodyButton.gameObject.SetActive(status);
        _bodyButton.interactable = status;
        _bodyCount.text = "0";
    }
    
    public void LeftArmEnabling(bool status)
    {
        //_leftArmButton.transform.position += -_leftArmButton.transform.forward * _forwardMultiplier;
        _leftArmButton.gameObject.SetActive(status);
        _leftArmButton.interactable = status;
        _leftArmCount.text = "0";
    }
    
    public void RightArmEnabling(bool status)
    {
        //_rightArmButton.transform.position += -_rightArmButton.transform.forward * _forwardMultiplier;
        _rightArmButton.gameObject.SetActive(status);
        _rightArmButton.interactable = status;
        _rightArmCount.text = "0";
    }
    public void LegsEnabling(bool status)
    {
        //_legsButton.transform.position += -_legsButton.transform.forward * _forwardMultiplier;
        _legsButton.gameObject.SetActive(status);
        _legsButton.interactable = status;
        _legsCount.text = "0";
    }

    public void SetBodyHpText(float hp)
    {
        var h = Mathf.FloorToInt(hp > 0 ? hp : 0);
        _bodyHpText.text = h.ToString();
        _bodyButtonSlider.value = h;
    }
    
    public void SetLeftArmHpText(float hp)
    {
        var h = Mathf.FloorToInt(hp > 0 ? hp : 0);
        _leftArmHpText.text = hp.ToString();
        _leftArmButtonSlider.value = h;
    }
    
    public void SetRightArmHpText(float hp)
    {
        var h = Mathf.FloorToInt(hp > 0 ? hp : 0);
        _rightArmHpText.text = hp.ToString();
        _rightArmButtonSlider.value = h;
    }
    
    public void SetLegsHpText(float hp)
    {
        var h = Mathf.FloorToInt(hp > 0 ? hp : 0);
        _legsHpText.text = hp.ToString();
        _legsButtonSlider.value = h;
    }

    public void SetBodyCount(int count)
    {
        _bodyCount.text = count.ToString();
    }
    
    public void SetLeftArmCount(int count)
    {
        _leftArmCount.text = count.ToString();
    }
    
    public void SetRightArmCount(int count)
    {
        _rightArmCount.text = count.ToString();
    }
    
    public void SetLegsCount(int count)
    {
        _legsCount.text = count.ToString();
    }
    
    #endregion
}
