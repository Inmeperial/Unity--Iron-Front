using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

// [CustomEditor(typeof(ButtonsUIManager))]
// public class ButtonsManagerInspector : Editor
// {
//     ButtonsUIManager _selection;
//     GUIStyle _importantStyle = new GUIStyle();
//     bool showButtons;
//     bool showPlayerHud;
//     bool showAttackHud;
//     bool showEnemyHud;
//     private void OnEnable()
//     {
//         _selection = Selection.activeGameObject.GetComponent<ButtonsUIManager>();
//         _importantStyle.fontStyle = FontStyle.Bold;
//         _importantStyle.fontSize = 15;
//     }
//
//     public override void OnInspectorGUI()
//     {
//         _selection.itemButton =
//             (ItemButton) EditorGUILayout.ObjectField("ItemButton", _selection.itemButton, typeof(ItemButton), true);
//         _selection.buttonExecuteAttack = (Button)EditorGUILayout.ObjectField("Button Execute Attack", _selection.buttonExecuteAttack, typeof(Button), true);
//         _selection.attackDelay = EditorGUILayout.FloatField("Attack Delay", _selection.attackDelay);
//         _selection.buttonEndTurn = (Button)EditorGUILayout.ObjectField("Button End Turn", _selection.buttonEndTurn, typeof(Button), true);
//         _selection.deselectKey = (KeyCode)EditorGUILayout.EnumPopup("Deselect Key", _selection.deselectKey);
//         _selection.selectRGunKey = (KeyCode)EditorGUILayout.EnumPopup("Select RGun Key", _selection.selectRGunKey);
//         _selection.selectLGunKey = (KeyCode)EditorGUILayout.EnumPopup("Select LGun Key", _selection.selectLGunKey);
//         _selection.showWorldUIKey = (KeyCode)EditorGUILayout.EnumPopup("Show World UI Key", _selection.showWorldUIKey);
//         _selection.toggleWorldUIKey = (KeyCode)EditorGUILayout.EnumPopup("Toggle World UI Key", _selection.toggleWorldUIKey);
//         
//         EditorGUILayout.BeginHorizontal();
//         EditorGUILayout.LabelField("Player HUD", _importantStyle);
//         showPlayerHud = EditorGUILayout.Toggle(showPlayerHud);
//         EditorGUILayout.EndHorizontal();
//
//         if (showPlayerHud)
//         {
//             _selection.playerHudContainer = (GameObject)EditorGUILayout.ObjectField("Player HUD Container", _selection.playerHudContainer, typeof(GameObject), true);
//
//             _selection.leftWeaponCircle = (GameObject)EditorGUILayout.ObjectField("Left Weapon Circle", _selection.leftWeaponCircle, typeof(GameObject), true);
//             _selection.rightWeaponCircle = (GameObject)EditorGUILayout.ObjectField("Right Weapon Circle", _selection.rightWeaponCircle, typeof(GameObject), true);
//
//             _selection.leftGunTypeText = (TextMeshProUGUI)EditorGUILayout.ObjectField("L Gun Type Text", _selection.leftGunTypeText, typeof(TextMeshProUGUI), true);
//             _selection.leftGunDamageText = (TextMeshProUGUI)EditorGUILayout.ObjectField("L GunDamage Text", _selection.leftGunDamageText, typeof(TextMeshProUGUI), true);
//             _selection.leftGunHitsText = (TextMeshProUGUI)EditorGUILayout.ObjectField("L Gun Hits Text", _selection.leftGunHitsText, typeof(TextMeshProUGUI), true);
//             _selection.leftGunHitChanceText = (TextMeshProUGUI)EditorGUILayout.ObjectField("L Gun HitChance Text", _selection.leftGunHitChanceText, typeof(TextMeshProUGUI), true);
//
//             _selection.rightGunTypeText = (TextMeshProUGUI)EditorGUILayout.ObjectField("R Gun Type Text", _selection.rightGunTypeText, typeof(TextMeshProUGUI), true);
//             _selection.rightGunDamageText = (TextMeshProUGUI)EditorGUILayout.ObjectField("R GunDamage Text", _selection.rightGunDamageText, typeof(TextMeshProUGUI), true);
//             _selection.rightGunHitsText = (TextMeshProUGUI)EditorGUILayout.ObjectField("R Gun Hits Text", _selection.rightGunHitsText, typeof(TextMeshProUGUI), true);
//             _selection.rightGunHitChanceText = (TextMeshProUGUI)EditorGUILayout.ObjectField("R Gun HitChance Text", _selection.rightGunHitChanceText, typeof(TextMeshProUGUI), true);
//
//             _selection.playerBodyCurrHp = (TextMeshProUGUI)EditorGUILayout.ObjectField("Body HP Text", _selection.playerBodyCurrHp, typeof(TextMeshProUGUI), true);
//             _selection.playerBodySlider = (Slider)EditorGUILayout.ObjectField("Body HP Slider", _selection.playerBodySlider, typeof(Slider), true);
//             
//             _selection.playerLeftArmCurrHp = (TextMeshProUGUI)EditorGUILayout.ObjectField("LArm HP Text", _selection.playerLeftArmCurrHp, typeof(TextMeshProUGUI), true);
//             _selection.playerLeftArmSlider = (Slider)EditorGUILayout.ObjectField("LArm HP Slider", _selection.playerLeftArmSlider, typeof(Slider), true);
//             
//             _selection.playerRightArmCurrHp = (TextMeshProUGUI)EditorGUILayout.ObjectField("RArm HP Text", _selection.playerRightArmCurrHp, typeof(TextMeshProUGUI), true);
//             _selection.playerRightArmSlider = (Slider)EditorGUILayout.ObjectField("RArm HP Slider", _selection.playerRightArmSlider, typeof(Slider), true);
//             
//             _selection.playerLegsCurrHp = (TextMeshProUGUI)EditorGUILayout.ObjectField("Legs HP Text", _selection.playerLegsCurrHp, typeof(TextMeshProUGUI), true);
//             _selection.playerLegsSlider = (Slider)EditorGUILayout.ObjectField("Legs HP Slider", _selection.playerLegsSlider, typeof(Slider), true);
//         }
//
//         EditorGUILayout.BeginHorizontal();
//         EditorGUILayout.LabelField("Attack HUD", _importantStyle);
//         showAttackHud = EditorGUILayout.Toggle(showAttackHud);
//         EditorGUILayout.EndHorizontal();
//         
//         if (showAttackHud)
//         {
//             _selection.attackHudContainer = (GameObject)EditorGUILayout.ObjectField("Attack HUD Container", _selection.attackHudContainer, typeof(GameObject), true);
//             _selection.attackWeaponNameText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Weapon Name Text", _selection.attackWeaponNameText, typeof(TextMeshProUGUI), true);
//             _selection.attackWeaponHitsText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Weapon Hits Text", _selection.attackWeaponHitsText, typeof(TextMeshProUGUI), true);
//             _selection.attackWeaponDamageText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Weapon Damage Text", _selection.attackWeaponDamageText, typeof(TextMeshProUGUI), true);
//             _selection.attackWeaponHitChanceText = (TextMeshProUGUI)EditorGUILayout.ObjectField("Weapon HitChance Text", _selection.attackWeaponHitChanceText, typeof(TextMeshProUGUI), true);
//             _selection.hitContainer = (GameObject)EditorGUILayout.ObjectField("Hit Container", _selection.hitContainer, typeof(GameObject), true);
//             _selection.hitImagePrefab = (GameObject)EditorGUILayout.ObjectField("Hit Image Prefab", _selection.hitImagePrefab, typeof(GameObject), true);
//         }
//     }
// }
