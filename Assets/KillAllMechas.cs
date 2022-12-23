using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllMechas : MonoBehaviour
{
    public EnumsClass.Team team;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            var mechas = FindObjectsOfType<Character>();

            foreach (var item in mechas)
            {
                if (item.GetUnitTeam() == team)
                    item.Dead();
            }
        }
    }
}
