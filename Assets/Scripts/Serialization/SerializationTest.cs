using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SerializationTest : MonoBehaviour
{
    public MechaEquipmentSO equipment;

    public TextMeshProUGUI text;
    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     Debug.Log("llamo add body");
        //     equipment.AddBody(0);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     Debug.Log("llamo add l arm");
        //     equipment.AddArm(0, true);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     Debug.Log("llamo add r arm");
        //     equipment.AddArm(0, false);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     Debug.Log("llamo add legs");
        //     equipment.AddLegs(0);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     Debug.Log("llamo add l gun");
        //     equipment.AddGun(0, true);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     Debug.Log("llamo add r arm");
        //     equipment.AddGun(0, false);
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     Debug.Log("llamo save");
        //     equipment.Save();
        // }
        //
        // if (Input.GetKeyDown(KeyCode.X))
        // {
        //     Debug.Log("llamo load");
        //     equipment.Load();
        // }
        //
        // if (Input.GetKeyDown(KeyCode.Space))
        //     CheckEquipment();
    }

    public void CheckEquipment()
    {
        text.text = "";

        if (equipment.body)
        {
            text.text += "body ok \n";
        }
        else text.text += "body null \n";
        
        if (equipment.legs)
        {
            text.text += "legs ok \n";
        }
        else text.text += "legs null \n";
        
        if (equipment.leftArm)
        {
            text.text += "leftArm ok \n";
        }
        else text.text += "leftArm null \n";
        
        if (equipment.rightArm)
        {
            text.text += "rightArm ok \n";
        }
        else text.text += "rightArm null \n";
        
        if (equipment.leftGun)
        {
            text.text += "leftGun ok \n";
        }
        else text.text += "leftGun null \n";
        
        if (equipment.rightGun)
        {
            text.text += "rightGun ok \n";
        }
        else text.text += "rightGun null \n";

        text.text += "name is: " + equipment.name;
    }
}
