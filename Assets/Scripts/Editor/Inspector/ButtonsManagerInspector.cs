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
        _selection.showWorldUIKey = (KeyCode)EditorGUILayout.EnumPopup("Show World UI Key", _selection.showWorldUIKey);
        _selection.toggleWorldUIKey = (KeyCode)EditorGUILayout.EnumPopup("Toggle World UI Key", _selection.toggleWorldUIKey);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Buttons", _importantStyle);
        showButtons = EditorGUILayout.Toggle(showButtons);
        EditorGUILayout.EndHorizontal();
        if (showButtons)
        {
            _selection.bodyPartsButtonsContainer = (GameObject)EditorGUILayout.ObjectField("Body Parts Button Container", _selection.bodyPartsButtonsContainer, typeof(GameObject), true);

            _selection.buttonBody = (CustomButton)EditorGUILayout.ObjectField("Button Body", _selection.buttonBody, typeof(CustomButton), true);

            _selection.buttonLArm = (CustomButton)EditorGUILayout.ObjectField("Button LArm", _selection.buttonLArm, typeof(CustomButton), true);

            _selection.buttonRArm = (CustomButton)EditorGUILayout.ObjectField("Button RArm", _selection.buttonRArm, typeof(CustomButton), true);

            _selection.buttonLegs = (CustomButton)EditorGUILayout.ObjectField("Button Legs", _selection.buttonLegs, typeof(CustomButton), true);
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

            _selection.playerBodyCurrHp = (TextMeshProUGUI)EditorGUILayout.ObjectField("Body HP Text", _selection.playerBodyCurrHp, typeof(TextMeshProUGUI), true);

            _selection.playerLeftArmCurrHp = (TextMeshProUGUI)EditorGUILayout.ObjectField("LArm HP Text", _selection.playerLeftArmCurrHp, typeof(TextMeshProUGUI), true);

            _selection.playerRightArmCurrHp = (TextMeshProUGUI)EditorGUILayout.ObjectField("RArm HP Text", _selection.playerRightArmCurrHp, typeof(TextMeshProUGUI), true);

            _selection.playerLegsCurrHp = (TextMeshProUGUI)EditorGUILayout.ObjectField("Legs HP Text", _selection.playerLegsCurrHp, typeof(TextMeshProUGUI), true);
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Enemy HUD", _importantStyle);
        showEnemyHud = EditorGUILayout.Toggle(showEnemyHud);
        EditorGUILayout.EndHorizontal();

        if (showEnemyHud)
        {
            _selection.enemyHudContainer = (GameObject)EditorGUILayout.ObjectField("Player HUD Container", _selection.enemyHudContainer, typeof(GameObject), true);

            _selection.enemyBodyCurrHp = (TextMeshProUGUI)EditorGUILayout.ObjectField("Body HP Text", _selection.enemyBodyCurrHp, typeof(TextMeshProUGUI), true);
            _selection.bulletsForBodyText = (TextMeshProUGUI) EditorGUILayout.ObjectField("Body Bullets Text", _selection.bulletsForBodyText, typeof(TextMeshProUGUI), true);
            
            _selection.enemyLeftArmCurrHp = (TextMeshProUGUI)EditorGUILayout.ObjectField("LArm HP Text", _selection.enemyLeftArmCurrHp, typeof(TextMeshProUGUI), true);
            _selection.bulletsForLArmText = (TextMeshProUGUI) EditorGUILayout.ObjectField("LArm Bullets Text", _selection.bulletsForLArmText, typeof(TextMeshProUGUI), true);
            
            _selection.enemyRightArmCurrHp = (TextMeshProUGUI)EditorGUILayout.ObjectField("RArm HP Text", _selection.enemyRightArmCurrHp, typeof(TextMeshProUGUI), true);
            _selection.bulletsForRArmText = (TextMeshProUGUI) EditorGUILayout.ObjectField("RArm Bullets Text", _selection.bulletsForRArmText, typeof(TextMeshProUGUI), true);
            
            _selection.enemyLegsCurrHp = (TextMeshProUGUI)EditorGUILayout.ObjectField("Legs HP Text", _selection.enemyLegsCurrHp, typeof(TextMeshProUGUI), true);
            _selection.bulletsForLegsText = (TextMeshProUGUI) EditorGUILayout.ObjectField("Legs Bullets Text", _selection.bulletsForLegsText, typeof(TextMeshProUGUI), true);
        }

    }
}
