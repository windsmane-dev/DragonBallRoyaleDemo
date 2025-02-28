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
        StartSpawnTimer(unitData.spawnTime);
    }

    public virtual void Activate()
    {
        isActive = true;
        visualHandler.SetUnitColor(Color.white);
    }

    public virtual void Deactivate()
    {
        isActive = false;
        visualHandler.SetUnitColor(Color.gray);
        Debug.Log($"{gameObject.name} is now inactive!");
    }

    public void StartSpawnTimer(float time)
    {
        Deactivate();
        StartCoroutine(SpawnDelayRoutine(time));
    }

    private IEnumerator SpawnDelayRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        Activate();
    }
}
