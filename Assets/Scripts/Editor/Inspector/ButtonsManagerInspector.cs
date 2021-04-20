using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

[CustomEditor(typeof(ButtonsUIManager))]
public class ButtonsManagerInspector : Editor
{
    ButtonsUIManager _selection;
    GUIStyle _importantStyle = new GUIStyle();
    bool showButtons;
    bool showPlayerHud;
    bool showEnemyHud;
    private void OnEnable()
    {
        _selection = Selection.activeGameObject.GetComponent<ButtonsUIManager>();
        _importantStyle.fontStyle = FontStyle.Bold;
        _importantStyle.fontSize = 15;
    }

    public override void OnInspectorGUI()
    {
        _selection.moveContainer = (GameObject)EditorGUILayout.ObjectField("Move Container Container", _selection.moveContainer, typeof(GameObject), true);
        _selection.actionMenu = (GameObject)EditorGUILayout.ObjectField("Action Menu Container", _selection.actionMenu, typeof(GameObject), true);

        _selection.buttonMove = (Button)EditorGUILayout.ObjectField("Button Move", _selection.buttonMove, typeof(Button), true);
        _selection.buttonUndo = (Button)EditorGUILayout.ObjectField("Button Undo", _selection.buttonUndo, typeof(Button), true);
        _selection.buttonSelectEnemy = (Button)EditorGUILayout.ObjectField("Button Attack", _selection.buttonSelectEnemy, typeof(Button), true);
        _selection.buttonExecuteAttack = (Button)EditorGUILayout.ObjectField("Button Execute Attack", _selection.buttonExecuteAttack, typeof(Button), true);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Buttons", _importantStyle);
        showButtons = EditorGUILayout.Toggle(showButtons);
        EditorGUILayout.EndHorizontal();
        if (showButtons)
        {
            _selection.bodyPartsButtonsContainer = (GameObject)EditorGUILayout.ObjectField("Body Parts Button Container", _selection.bodyPartsButtonsContainer, typeof(GameObject), true);

            _selection.buttonBody = (Button)EditorGUILayout.ObjectField("Button Body", _selection.buttonBody, typeof(Button), true);
            _selection.buttonBodyMinus = (Button)EditorGUILayout.ObjectField("Button Minus", _selection.buttonBodyMinus, typeof(Button), true);
            _selection.buttonBodyX = (Button)EditorGUILayout.ObjectField("Button Body X", _selection.buttonBodyX, typeof(Button), true);

            _selection.buttonLArm = (Button)EditorGUILayout.ObjectField("Button LArm", _selection.buttonLArm, typeof(Button), true);
            _selection.buttonLArmMinus = (Button)EditorGUILayout.ObjectField("Button LArm Minus", _selection.buttonLArmMinus, typeof(Button), true);
            _selection.buttonLArmX = (Button)EditorGUILayout.ObjectField("Button LArm X", _selection.buttonLArmX, typeof(Button), true);

            _selection.buttonRArm = (Button)EditorGUILayout.ObjectField("Button RArm", _selection.buttonRArm, typeof(Button), true);
            _selection.buttonRArmMinus = (Button)EditorGUILayout.ObjectField("Button RArm Minus", _selection.buttonRArmMinus, typeof(Button), true);
            _selection.buttonRArmX = (Button)EditorGUILayout.ObjectField("Button RArm X", _selection.buttonRArmX, typeof(Button), true);

            _selection.buttonLegs = (Button)EditorGUILayout.ObjectField("Button Legs", _selection.buttonLegs, typeof(Button), true);
            _selection.buttonLegsMinus = (Button)EditorGUILayout.ObjectField("Button Legs Minus", _selection.buttonLegsMinus, typeof(Button), true);
            _selection.buttonLegsX = (Button)EditorGUILayout.ObjectField("Button Legs X", _selection.buttonLegsX, typeof(Button), true);
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Player HUD", _importantStyle);
        showPlayerHud = EditorGUILayout.Toggle(showPlayerHud);
        EditorGUILayout.EndHorizontal();

        if (showPlayerHud)
        {
            _selection.playerHudContainer = (GameObject)EditorGUILayout.ObjectField("Player HUD Container", _selection.playerHudContainer, typeof(GameObject), true);

            _selection.damageText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Damage Text", _selection.damageText, typeof(TextMeshProUGUI), true);
            _selection.rangeText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Range Text", _selection.rangeText, typeof(TextMeshProUGUI), true);


            _selection.playerBodySlider = (Slider)EditorGUILayout.ObjectField("Body Slider", _selection.playerBodySlider, typeof(Slider), true);
            _selection.playerBodyCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("Body HP Text", _selection.playerBodyCurrHP, typeof(TextMeshProUGUI), true);

            _selection.playerLeftArmSlider = (Slider)EditorGUILayout.ObjectField("LArm Slider", _selection.playerLeftArmSlider, typeof(Slider), true);
            _selection.playerLeftArmCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("LArm HP Text", _selection.playerLeftArmCurrHP, typeof(TextMeshProUGUI), true);

            _selection.playerRightArmSlider = (Slider)EditorGUILayout.ObjectField("RArm Slider", _selection.playerRightArmSlider, typeof(Slider), true);
            _selection.playerRightArmCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("RArm HP Text", _selection.playerRightArmCurrHP, typeof(TextMeshProUGUI), true);

            _selection.playerLegsSlider = (Slider)EditorGUILayout.ObjectField("Legs Slider", _selection.playerLegsSlider, typeof(Slider), true);
            _selection.playerLegsCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("Legs HP Text", _selection.playerLegsCurrHP, typeof(TextMeshProUGUI), true);
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Enemy HUD", _importantStyle);
        showEnemyHud = EditorGUILayout.Toggle(showEnemyHud);
        EditorGUILayout.EndHorizontal();

        if (showEnemyHud)
        {
            _selection.enemyHudContainer = (GameObject)EditorGUILayout.ObjectField("Player HUD Container", _selection.enemyHudContainer, typeof(GameObject), true);

            _selection.enemyBodySlider = (Slider)EditorGUILayout.ObjectField("Body Slider", _selection.enemyBodySlider, typeof(Slider), true);
            _selection.enemyBodyCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("Body HP Text", _selection.enemyBodyCurrHP, typeof(TextMeshProUGUI), true);

            _selection.enemyLeftArmSlider = (Slider)EditorGUILayout.ObjectField("LArm Slider", _selection.enemyLeftArmSlider, typeof(Slider), true);
            _selection.enemyLeftArmCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("LArm HP Text", _selection.enemyLeftArmCurrHP, typeof(TextMeshProUGUI), true);

            _selection.enemyRightArmSlider = (Slider)EditorGUILayout.ObjectField("RArm Slider", _selection.enemyRightArmSlider, typeof(Slider), true);
            _selection.enemyRightArmCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("RArm HP Text", _selection.enemyRightArmCurrHP, typeof(TextMeshProUGUI), true);

            _selection.enemyLegsSlider = (Slider)EditorGUILayout.ObjectField("Legs Slider", _selection.enemyLegsSlider, typeof(Slider), true);
            _selection.enemyLegsCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("Legs HP Text", _selection.enemyLegsCurrHP, typeof(TextMeshProUGUI), true);
        }

    }
}
