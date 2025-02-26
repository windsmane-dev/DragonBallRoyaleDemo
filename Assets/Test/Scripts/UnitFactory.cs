using UnityEngine;
using System.Collections.Generic;

public class UnitFactory
{
    private Dictionary<UnitType, PoolManager<MonoBehaviour>> unitPools;

    public UnitFactory(Dictionary<UnitType, MonoBehaviour> prefabs, int poolSize)
    {
        unitPools = new Dictionary<UnitType, PoolManager<MonoBehaviour>>();

        foreach (var unitType in prefabs.Keys)
        {
            unitPools[unitType] = new PoolManager<MonoBehaviour>(prefabs[unitType], poolSize);
        }
    }

    public IUnit CreateUnit(UnitType type, Vector3 position, Quaternion rotation)
    {
        if (!unitPools.ContainsKey(type))
        {
            Debug.LogError($"Unit type {type} not registered in UnitFactory.");
            return null;
        }

        MonoBehaviour unitObject = unitPools[type].GetFromPool(position, rotation);
        IUnit unit = unitObject as IUnit;

        if (unit != null)
        {
            unit.Activate();
            return unit;
        }
        else
        {
            Debug.LogError($"Unit of type {type} does not implement IUnit.");
            return null;
        }
    }

    public void ReturnUnit(IUnit unit)
    {
        MonoBehaviour monoUnit = unit as MonoBehaviour;
        if (monoUnit != null)
        {
            unit.Deactivate();
            foreach (var pool in unitPools.Values)
            {
                if (pool.Contains(monoUnit))
                {
                    pool.ReturnToPool(monoUnit);
                    return;
                }
            }
        }
    }
}
