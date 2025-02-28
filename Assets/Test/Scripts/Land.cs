using System;
using UnityEngine;
public class Land : MonoBehaviour, ISelectable
{
    [SerializeField] private LandType landType;
    [SerializeField] private Transform goalTransform;

    public int playerID;
    private void OnEnable()
    {
        EventHolder.OnTurnSwitch += SwitchRole;
        EventHolder.OnRequestLandPosition += ProvideSpawnPosition;
        EventHolder.OnRequestGoalPosition += ProvideGoalPosition;
        EventHolder.OnRequestParentLand += ProvideParentLand;
    }

    private void OnDisable()
    {
        EventHolder.OnTurnSwitch -= SwitchRole;
        EventHolder.OnRequestLandPosition -= ProvideSpawnPosition;
        EventHolder.OnRequestGoalPosition -= ProvideGoalPosition;
        EventHolder.OnRequestParentLand -= ProvideParentLand;
    }

    void ProvideParentLand(LandType inType, Action<Transform> callback)
    {
        if(inType == landType)
        {
            callback(this.transform);
        } 
    }

    public void OnSelect(Vector3 position)
    {
        EventHolder.TriggerInputReceived(position, landType, playerID);
    }

    private void SwitchRole()
    {
        landType = (landType == LandType.Attacker) ? LandType.Defender : LandType.Attacker;
        Debug.Log($"Land role switched to: {landType}");
    }

    private void ProvideSpawnPosition(Action<Vector3> callback)
    {
        if (landType == LandType.Attacker)
        {
            Vector3 spawnPosition = GetRandomPointInLand();
            callback(spawnPosition);
        }
    }

    private void ProvideGoalPosition(Action<Vector3> callback)
    {
        if (landType == LandType.Defender)
            callback(goalTransform.position);
    }

    private Vector3 GetRandomPointInLand()
    {
        Collider landCollider = GetComponent<Collider>();
        if (landCollider == null) return transform.position;

        Bounds bounds = landCollider.bounds;
        float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(randomX, transform.position.y, randomZ);
    }
}
