using UnityEngine;
using System;
using System.Collections.Generic;

public class UnitFactory
{
    private Dictionary<UnitType, PoolManager<GameObject>> unitPools;
    private Dictionary<UnitType, UnitData> unitDataDictionary;

    public UnitFactory(Dictionary<UnitType, GameObject> prefabs, Dictionary<UnitType, UnitData> unitData, int poolSize)
    {
        unitPools = new Dictionary<UnitType, PoolManager<GameObject>>();
        unitDataDictionary = unitData;

        foreach (var unitType in prefabs.Keys)
        {
            unitPools[unitType] = new PoolManager<GameObject>(prefabs[unitType], poolSize);
        }

        EventHolder.OnUnitDataRequest += HandleUnitDataRequest; 
    }

    public void Destroyed()
    {
        EventHolder.OnUnitDataRequest -= HandleUnitDataRequest; 
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

    private void HandleUnitDataRequest(UnitType unitType, Action<UnitData> callback)
    {
        if (unitDataDictionary.TryGetValue(unitType, out UnitData data))
        {
            callback(data);
        }
        else
        {
            Debug.LogError($"UnitData not found for {unitType}!");
            callback(null);
        }
    }
}
