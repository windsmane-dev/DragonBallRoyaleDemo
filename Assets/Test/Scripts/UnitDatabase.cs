using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UnitDatabase", menuName = "Game Data/Unit Database")]
public class UnitDatabase : ScriptableObject
{
    public List<UnitEntry> units;
    public GameObject ballPrefab;

    [System.Serializable]
    public class UnitEntry
    {
        public UnitType unitType;
        public GameObject prefab;
        public UnitData unitData;
    }

    public Dictionary<UnitType, GameObject> GetUnitDictionary()
    {
        Dictionary<UnitType, GameObject> unitDict = new Dictionary<UnitType, GameObject>();
        foreach (var entry in units)
        {
            if (entry.prefab != null)
                unitDict[entry.unitType] = entry.prefab;
        }
        return unitDict;
    }

    public Dictionary<UnitType, UnitData> GetUnitDataDictionary()
    {
        Dictionary<UnitType, UnitData> unitDataDict = new Dictionary<UnitType, UnitData>();
        foreach (var entry in units)
        {
            if (entry.unitData != null)
                unitDataDict[entry.unitType] = entry.unitData;
        }
        return unitDataDict;
    }
}
