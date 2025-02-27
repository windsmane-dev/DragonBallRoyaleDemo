using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour, IUnit
{
    protected UnitData unitData;
    protected UnitVisualHandler visualHandler;
    protected bool isActive = false;

    public virtual void Initialize(UnitData data)
    {
        unitData = data;
        visualHandler = new UnitVisualHandler(gameObject);
        StartSpawnTimer();
    }

    public virtual void Activate()
    {
        isActive = true;
        visualHandler.SetUnitColor(Color.white);
        Debug.Log($"{gameObject.name} is now active!");
    }

    public virtual void Deactivate()
    {
        isActive = false;
        visualHandler.SetUnitColor(Color.gray);
        Debug.Log($"{gameObject.name} is now inactive!");
    }

    public void StartSpawnTimer()
    {
        Deactivate();
        StartCoroutine(SpawnDelayRoutine());
    }

    private IEnumerator SpawnDelayRoutine()
    {
        yield return new WaitForSeconds(unitData.spawnTime);
        Activate();
    }
}
