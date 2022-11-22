using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GunsHUD : Initializable
{
    [Header("Inputs")]
    [SerializeField] private GunsSelector _gunsSelector;

    [Header("Others")]
    [SerializeField] private GameObject _container;

    [Header("Buttons")]
    [SerializeField] private Button _leftGunButton;
    [SerializeField] private Button _rightGunButton;

    [Header("Left Gun")]
    [SerializeField] private GameObject _leftGunContainer;
    [SerializeField] private GameObject _leftGunCircle;
    [SerializeField] private TextMeshProUGUI _leftGunNameText;
    [SerializeField] private TextMeshProUGUI _leftGunHitsText;
    [SerializeField] private TextMeshProUGUI _leftGunDamageText;
    [SerializeField] private TextMeshProUGUI _leftGunHitChanceText;

    [Header("Right Gun")]
    [SerializeField] private GameObject _rightGunContainer;
    [SerializeField] private GameObject _rightGunCircle;
    [SerializeField] private TextMeshProUGUI _rightGunNameText;
    [SerializeField] private TextMeshProUGUI _rightGunHitsText;
    [SerializeField] private TextMeshProUGUI _rightGunDamageText;
    [SerializeField] private TextMeshProUGUI _rightGunHitChanceText;

    private void Awake()
    {
        HideContainer();
    }

    public override void Initialize()
    {
        Character[] mechas = GameManager.Instance.GetMechas();

        foreach (Character mecha in mechas)
        {
            mecha.OnMechaSelected += OnMechaSelected;
            mecha.OnRightGunSelected += OnRightGunSelected;
            mecha.OnLeftGunSelected += OnLeftGunSelected;
        }
        HideContainer();

        _gunsSelector.OnLeftGunSelected += OnLeftGunSelected;
        _gunsSelector.OnRightGunSelected += OnRightGunSelected;

        GameManager.Instance.OnBeginTurn += ShowContainer;
        
        GameManager.Instance.OnEndTurn += HideContainer;
        GameManager.Instance.OnEnemyMechaSelected += HideContainer;
        GameManager.Instance.OnEnemyMechaDeselected += ShowContainer;
    }

    private void OnMechaSelected(Character mecha)
    {
        ConfigureGunsUI(mecha, mecha.GetLeftGun(), mecha.GetRightGun());

        ShowContainer();
    } 

    private void ConfigureGunsUI(Character mecha, Gun left, Gun right)
    {
        if (left)
        {
            _leftGunContainer.SetActive(true);
            _leftGunNameText.text = left.GetGunName();
            _leftGunHitsText.text = left.GetMaxBullets().ToString();
            _leftGunDamageText.text = left.GetBulletDamage().ToString();
            _leftGunHitChanceText.text = left.GetHitChance().ToString();

            if (mecha.GetUnitTeam() == EnumsClass.Team.Green)
                _leftGunButton.interactable = true;
            else
                _leftGunButton.interactable = false;
        }
        else
            _leftGunContainer.SetActive(false);

        if (right)
        {
            _rightGunContainer.SetActive(true);
            _rightGunNameText.text = right.GetGunName();
            _rightGunHitsText.text = right.GetMaxBullets().ToString();
            _rightGunDamageText.text = right.GetBulletDamage().ToString();
            _rightGunHitChanceText.text = right.GetHitChance().ToString();

            if (mecha.GetUnitTeam() == EnumsClass.Team.Green)
                _rightGunButton.interactable = true;
            else
                _rightGunButton.interactable = false;
        }
        else
            _rightGunContainer.SetActive(false);

    }
    

    private void ShowContainer()
    {
        _container.SetActive(true);
    }

    private void HideContainer()
    {
        _container.SetActive(false);
    }

    public void EnableButtonsInteraction()
    {
        _leftGunButton.interactable = true;
        _rightGunButton.interactable = true;
    }
    public void DisableButtonsInteraction()
    {
        _leftGunButton.interactable = false;
        _rightGunButton.interactable = false;
    }
    public void OnRightGunSelected()
    {
        _rightGunCircle.SetActive(true);
        _leftGunCircle.SetActive(false);
    }
    public void OnLeftGunSelected()
    {
        _leftGunCircle.SetActive(true);
        _rightGunCircle.SetActive(false);
    }

    private void OnDestroy()
    {
        Character[] mechas = GameManager.Instance.GetMechas();

        foreach (var mecha in mechas)
        {
            mecha.OnMechaSelected -= OnMechaSelected;
            mecha.OnRightGunSelected -= OnRightGunSelected;
            mecha.OnLeftGunSelected -= OnLeftGunSelected;
        }

        _gunsSelector.OnLeftGunSelected -= OnLeftGunSelected;
        _gunsSelector.OnRightGunSelected -= OnRightGunSelected;

        GameManager.Instance.OnBeginTurn -= ShowContainer;
        GameManager.Instance.OnEndTurn -= HideContainer;
        GameManager.Instance.OnEnemyMechaSelected -= HideContainer;
    }
}
