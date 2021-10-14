using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializationTest : MonoBehaviour
{
    public MechaEquipmentSO equipment;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("llamo add body");
            equipment.AddBody(0);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("llamo add l arm");
            equipment.AddArm(0, true);
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("llamo add r arm");
            equipment.AddArm(0, false);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("llamo add legs");
            equipment.AddLegs(0);
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("llamo add l gun");
            equipment.AddGun(0, true);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("llamo add r arm");
            equipment.AddGun(0, false);
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("llamo save");
            equipment.Save();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("llamo load");
            equipment.Load();
        }
    }
}
