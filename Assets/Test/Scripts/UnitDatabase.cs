using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UnitDatabase", menuName = "Game Data/Unit Database")]
public class UnitDatabase : ScriptableObject
{
    public List<UnitEntry> units;

    [System.Serializable]
    public class UnitEntry
    {
        public UnitType unitType;
        public MonoBehaviour prefab;
    }

    public Dictionary<UnitType, MonoBehaviour> GetUnitDictionary()
    {
        Dictionary<UnitType, MonoBehaviour> unitDict = new Dictionary<UnitType, MonoBehaviour>();
        foreach (var entry in units)
        {
            if (entry.prefab != null)
                unitDict[entry.unitType] = entry.prefab;
        }
        return unitDict;
    }
}
