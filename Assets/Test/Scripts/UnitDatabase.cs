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
        public GameObject prefab; // Changed from MonoBehaviour to GameObject
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
}
