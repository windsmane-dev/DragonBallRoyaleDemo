using UnityEngine;
using System.Collections;

public abstract class Unit : MonoBehaviour, IUnit
{
    public UnitData unitData;
    protected UnitVisualHandler visualHandler;

    public GameObject renderObject;
    public Animator anim;
    [SerializeField]protected bool isActive = false;

    public virtual void Initialize(UnitData data)
    {
        unitData = data;

        if (visualHandler == null)
        {

            visualHandler = new UnitVisualHandler(renderObject, anim);
        }



        visualHandler.OnSpawn(unitData.spawnVFX);

        isActive = false;
        visualHandler.SetUnitMaterial(unitData.deactivatedMat);
        StartCoroutine(SpawnDelayRoutine(unitData.spawnTime));
    }

    public virtual void Activate()
    {
        isActive = true;
           
        visualHandler.SetUnitMaterial(unitData.activatedMat);
        visualHandler.OnActivate(unitData.activateVFX);
       
    }

    public virtual void Deactivate()
    {
        isActive = false;
        visualHandler.SetUnitMaterial(unitData.deactivatedMat);
        visualHandler.OnDeactivate(unitData.deactivateVFX);
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
