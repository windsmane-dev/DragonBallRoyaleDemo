using UnityEngine;
using System.Collections.Generic;

public class UnitFactory
{
    private Dictionary<UnitType, PoolManager<GameObject>> unitPools;

    public UnitFactory(Dictionary<UnitType, GameObject> prefabs, int poolSize)
    {
        unitPools = new Dictionary<UnitType, PoolManager<GameObject>>();

        foreach (var unitType in prefabs.Keys)
        {
            unitPools[unitType] = new PoolManager<GameObject>(prefabs[unitType], poolSize);
        }
    }

    public GameObject CreateUnit(UnitType type, Vector3 position, Quaternion rotation)
    {
        if (!unitPools.ContainsKey(type))
        {
            Debug.LogError($"Unit type {type} not registered in UnitFactory.");
            return null;
        }

        GameObject unitObject = unitPools[type].GetFromPool(position, rotation);

        if (unitObject != null)
        {
            unitObject.SetActive(true);
            return unitObject;
        }
        else
        {
            Debug.LogError($"Failed to spawn unit of type {type}.");
            return null;
        }
    }

    public void ReturnUnit(GameObject unit)
    {
        foreach (var pool in unitPools.Values)
        {
            if (pool.Contains(unit))
            {
                pool.ReturnToPool(unit);
                return;
            }
        }
    }
}
