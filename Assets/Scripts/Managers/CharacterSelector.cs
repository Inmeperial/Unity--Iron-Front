using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField] private InputsReader _inputReader;
    public LayerMask charMask;
    public LayerMask gridBlockMask;
    
    private TileHighlight _highlight;
    private bool _canSelectUnit;
    
    private bool _selectingWithDelay;

    public static CharacterSelector Instance;

    public Action<Character> OnTurnMechaSelected;
    public Action OnPlayerMechaDeselected;
    public Action<Character> OnEnemyMechaSelected;
    public Action OnEnemyMechaDeselected;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        EnableCharacterSelection();
        _highlight = GetComponent<TileHighlight>();

        _inputReader.OnDeselectKeyPressed += DeselectCurrentEnemy;
        _inputReader.OnDeselectKeyPressed += EnableCharacterSelection;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_canSelectUnit)
            return;

        if (GameManager.Instance.ActiveTeam == EnumsClass.Team.Red) 
            return;

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (Input.GetMouseButtonDown(0) && _canSelectUnit && MouseRay.CheckIfType(charMask))
            {
                SelectCharacterFromObject(charMask);

            }
            if (Input.GetMouseButtonDown(0) && _canSelectUnit && MouseRay.CheckIfType(gridBlockMask))
            {
                SelectCharacterFromTile(gridBlockMask);
            }
        }
    }

    private void SelectCharacterFromTile(LayerMask layerMask)
    {
        GameObject tile = MouseRay.GetTargetGameObject(layerMask);

        if (!tile || !tile.CompareTag("GridBlock"))
            return;

        Character characterAboveTile = tile.GetComponent<Tile>().GetUnitAbove();

        if (characterAboveTile) 
            Selection(characterAboveTile);
    }
    
    private void SelectCharacterFromObject(LayerMask mask)
    {
        Transform characterTransform = MouseRay.GetTargetTransform(mask);
        
        if (!characterTransform || !characterTransform.CompareTag("Character"))
            return;
        Character character = characterTransform.GetComponent<Character>();
        Selection(character);
    }

    public void Selection(Character character)
    {
        if (!character.CanBeSelected())
        {
            Debug.Log(character.GetCharacterName() + " cant be selected");
            return;
        }
        if (character.IsMyTurn())
        {
            //ButtonsUIManager.Instance.DeselectActions();

            DeselectCurrentCharacter();

            DeselectCurrentEnemy();

            character.SelectThisUnit();

            _highlight.ChangeActiveCharacter(character);

            OnTurnMechaSelected?.Invoke(character);

            //ButtonsUIManager.Instance.SetPlayerCharacter(character);
            //ButtonsUIManager.Instance.SetPlayerUI();
        }
        else if (character.GetUnitTeam() != GameManager.Instance.ActiveTeam && character.CanBeAttacked())
        {
            Character selectedCharacter = GameManager.Instance.CurrentTurnMecha;

            if (selectedCharacter)
            {
                if (!selectedCharacter.GetLeftGun() && !selectedCharacter.GetRightGun())
                    return;
            }

            if (selectedCharacter.CanAttack())
            {
                selectedCharacter.SaveRotation();

                selectedCharacter.RotateTowardsEnemy(character.transform);

                bool enemyInSight = selectedCharacter.IsEnemyInSight(character);

                if (enemyInSight)
                {
                    DeselectCurrentEnemy();

                    OnEnemyMechaSelected?.Invoke(character);
                    //character.SelectingEnemy();

                    //character.SelectedAsEnemy();


                    //ButtonsUIManager.Instance.SetEnemy(character);
                    //ButtonsUIManager.Instance.SetEnemyUI();

                    //DeactivateCharacterSelection();
                }
                else
                {
                    selectedCharacter.RaysOffDelay();
                    selectedCharacter.ResetRotationAndRays(); 
                }
            }
        }
    }

    public void EnableCharacterSelection() => _canSelectUnit = true;
    public void DisableCharacterSelection() => _canSelectUnit = false;

    //public void ResetSelector()
    //{
    //    DeselectCurrentCharacter();

    //    DeselectCurrentEnemy();
    //    //Cambiarlo despues
    //    //ButtonsUIManager.Instance.DeactivateExecuteAttackButton();
    //}

    public void SelectionWithDelay(Character character)
    {
        if (!_selectingWithDelay)
            StartCoroutine(SelectionDelay(character));
    }

    private IEnumerator SelectionDelay(Character character)
    {
        _selectingWithDelay = true;

        yield return new WaitForEndOfFrame();

        if (character.IsMyTurn())
            Selection(character);

        _selectingWithDelay = false;
    }

    public void DeselectCurrentCharacter() => OnPlayerMechaDeselected?.Invoke();

    public void DeselectCurrentEnemy() => OnEnemyMechaDeselected?.Invoke();

    private void OnDestroy()
    {
        _inputReader.OnDeselectKeyPressed -= DeselectCurrentEnemy;
        _inputReader.OnDeselectKeyPressed -= EnableCharacterSelection;

        OnTurnMechaSelected = null;
        OnPlayerMechaDeselected = null;
        OnEnemyMechaSelected = null;
        OnEnemyMechaDeselected = null;
    }
}