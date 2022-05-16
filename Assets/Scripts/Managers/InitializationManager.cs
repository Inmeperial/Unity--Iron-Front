using UnityEngine;

public class InitializationManager : MonoBehaviour
{
    private void Awake()
    {
        //var characters = FindObjectsOfType<Character>();

        //foreach (var c in characters)
        //{
        //    c.ManualAwake();
        //}
        
        FindObjectOfType<EquipmentManager>().ManualAwake();

        TurnManager turnManager = FindObjectOfType<TurnManager>();
        turnManager.ManualAwake();

    }

    private void Start()
    {
        Character[] characters = FindObjectsOfType<Character>();
        
        foreach (Character character in characters)
        {
            character.ManualStart();
        }

        //GetPartsOfMecha[] getParts = FindObjectsOfType<GetPartsOfMecha>();

        //foreach (var g in getParts)
        //{
        //    g.ManualStart();
        //}

        //TurnManager turnManager = FindObjectOfType<TurnManager>();

        //turnManager.ManualStart();
        TurnManager.Instance.ManualStart();
    }
}
