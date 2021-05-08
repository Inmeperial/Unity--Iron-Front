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

        _selection.buttonMove = (Button)EditorGUILayout.ObjectField("Button Move", _selection.buttonMove, typeof(Button), true);
        _selection.buttonUndo = (Button)EditorGUILayout.ObjectField("Button Undo", _selection.buttonUndo, typeof(Button), true);
        _selection.buttonExecuteAttack = (Button)EditorGUILayout.ObjectField("Button Execute Attack", _selection.buttonExecuteAttack, typeof(Button), true);
        _selection.buttonEndTurn = (Button)EditorGUILayout.ObjectField("Button End Turn", _selection.buttonEndTurn, typeof(Button), true);
        _selection.deselectKey = (KeyCode)EditorGUILayout.EnumPopup("Deselect Key", _selection.deselectKey);
        _selection.selectRGunKey = (KeyCode)EditorGUILayout.EnumPopup("Select RGun Key", _selection.selectRGunKey);
        _selection.selectLGunKey = (KeyCode)EditorGUILayout.EnumPopup("Select LGun Key", _selection.selectLGunKey);

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

            _selection.leftWeaponCircle = (GameObject)EditorGUILayout.ObjectField("Left Weapon Circle", _selection.leftWeaponCircle, typeof(GameObject), true);
            _selection.rightWeaponCircle = (GameObject)EditorGUILayout.ObjectField("Right Weapon Circle", _selection.rightWeaponCircle, typeof(GameObject), true);

            _selection.leftGunTypeText = (TextMeshProUGUI)EditorGUILayout.ObjectField("L Gun Type Text", _selection.leftGunTypeText, typeof(TextMeshProUGUI), true);
            _selection.leftGunDamageText = (TextMeshProUGUI)EditorGUILayout.ObjectField("L GunDamage Text", _selection.leftGunDamageText, typeof(TextMeshProUGUI), true);
            _selection.leftGunHitsText = (TextMeshProUGUI)EditorGUILayout.ObjectField("L Gun Hits Text", _selection.leftGunHitsText, typeof(TextMeshProUGUI), true);
            _selection.leftGunHitChanceText = (TextMeshProUGUI)EditorGUILayout.ObjectField("L Gun HitChance Text", _selection.leftGunHitChanceText, typeof(TextMeshProUGUI), true);

            _selection.rightGunTypeText = (TextMeshProUGUI)EditorGUILayout.ObjectField("R Gun Type Text", _selection.rightGunTypeText, typeof(TextMeshProUGUI), true);
            _selection.rightGunDamageText = (TextMeshProUGUI)EditorGUILayout.ObjectField("R GunDamage Text", _selection.rightGunDamageText, typeof(TextMeshProUGUI), true);
            _selection.rightGunHitsText = (TextMeshProUGUI)EditorGUILayout.ObjectField("R Gun Hits Text", _selection.rightGunHitsText, typeof(TextMeshProUGUI), true);
            _selection.rightGunHitChanceText = (TextMeshProUGUI)EditorGUILayout.ObjectField("R Gun HitChance Text", _selection.rightGunHitChanceText, typeof(TextMeshProUGUI), true);

            _selection.playerBodyCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("Body HP Text", _selection.playerBodyCurrHP, typeof(TextMeshProUGUI), true);

            _selection.playerLeftArmCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("LArm HP Text", _selection.playerLeftArmCurrHP, typeof(TextMeshProUGUI), true);

            _selection.playerRightArmCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("RArm HP Text", _selection.playerRightArmCurrHP, typeof(TextMeshProUGUI), true);

            _selection.playerLegsCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("Legs HP Text", _selection.playerLegsCurrHP, typeof(TextMeshProUGUI), true);
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Enemy HUD", _importantStyle);
        showEnemyHud = EditorGUILayout.Toggle(showEnemyHud);
        EditorGUILayout.EndHorizontal();

        if (showEnemyHud)
        {
            _selection.enemyHudContainer = (GameObject)EditorGUILayout.ObjectField("Player HUD Container", _selection.enemyHudContainer, typeof(GameObject), true);

            _selection.enemyBodyCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("Body HP Text", _selection.enemyBodyCurrHP, typeof(TextMeshProUGUI), true);

            _selection.enemyLeftArmCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("LArm HP Text", _selection.enemyLeftArmCurrHP, typeof(TextMeshProUGUI), true);

            _selection.enemyRightArmCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("RArm HP Text", _selection.enemyRightArmCurrHP, typeof(TextMeshProUGUI), true);

            _selection.enemyLegsCurrHP = (TextMeshProUGUI)EditorGUILayout.ObjectField("Legs HP Text", _selection.enemyLegsCurrHP, typeof(TextMeshProUGUI), true);
        }

    }
}
