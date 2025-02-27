using System;
using UnityEngine;
public class Land : MonoBehaviour, ISelectable
{
    [SerializeField] private LandType landType;

    private void OnEnable()
    {
        TurnManager.OnTurnSwitch += SwitchRole;
        EventHolder.OnRequestLandPosition += ProvideSpawnPosition;
    }

    private void OnDisable()
    {
        TurnManager.OnTurnSwitch -= SwitchRole;
        EventHolder.OnRequestLandPosition -= ProvideSpawnPosition;
    }

    public void OnSelect(Vector3 position)
    {
        EventHolder.TriggerInputReceived(position, landType);
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
