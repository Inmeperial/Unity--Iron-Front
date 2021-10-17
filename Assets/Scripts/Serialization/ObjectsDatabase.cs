using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "ObjectsDatabase")]
public class ObjectsDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public List<BodySO> bodies;
    public Dictionary<BodySO, int> bodiesID = new Dictionary<BodySO, int>();
    public Dictionary<int, BodySO> bodiesSO = new Dictionary<int, BodySO>();
    
    public List<ArmSO> arms;
    public Dictionary<ArmSO, int> armsID = new Dictionary<ArmSO, int>();
    public Dictionary<int, ArmSO> armsSO = new Dictionary<int, ArmSO>();
    
    public List<LegsSO> legs;
    public Dictionary<LegsSO, int> legsID = new Dictionary<LegsSO, int>();
    public Dictionary<int, LegsSO> legsSO = new Dictionary<int, LegsSO>();

    public List<GunSO> guns;
    public Dictionary<GunSO, int> gunID = new Dictionary<GunSO, int>();
    public Dictionary<int, GunSO> gunSO = new Dictionary<int, GunSO>();

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        bodiesID = new Dictionary<BodySO, int>();
        bodiesSO = new Dictionary<int, BodySO>();

        for (int i = 0; i < bodies.Count; i++)
        {
            bodiesID.Add(bodies[i], i);
            bodiesSO.Add(i, bodies[i]);
        }
        
        armsID = new Dictionary<ArmSO, int>();
        armsSO = new Dictionary<int, ArmSO>();
        
        for (int i = 0; i < arms.Count; i++)
        {
            armsID.Add(arms[i], i);
            armsSO.Add(i, arms[i]);
        }
        
        legsID = new Dictionary<LegsSO, int>();
        legsSO = new Dictionary<int, LegsSO>();
        
        for (int i = 0; i < legs.Count; i++)
        {
            legsID.Add(legs[i], i);
            legsSO.Add(i, legs[i]);
        }

        gunID = new Dictionary<GunSO, int>();
        
        for (int i = 0; i < guns.Count; i++)
        {
            gunID.Add(guns[i], i);
            gunSO.Add(i, guns[i]);
        }
    }
}
