using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    private int playerID;
    private int currentEnergy = 10;

    public void Initialize(int id)
    {
        playerID = id;
    }

    private void OnEnable()
    {
        EventHolder.OnEnergyRequest += HandleEnergyRequest;
    }

    private void OnDisable()
    {
        EventHolder.OnEnergyRequest -= HandleEnergyRequest;
    }

    private void HandleEnergyRequest(int requestedPlayerID, int amount, System.Action<bool> callback)
    {
        if (requestedPlayerID != playerID) return; // Ignore requests for the other player

        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            callback(true);
        }
        else
        {
            callback(false);
        }
    }
}
