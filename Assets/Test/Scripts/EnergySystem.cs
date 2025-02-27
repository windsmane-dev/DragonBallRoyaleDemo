using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    private int playerID;
    private float currentEnergy = 0f;
    private float energyRegenRate = 0.5f;

    [SerializeField] private float maxEnergy = 10f;

    public void Initialize(int id)
    {
        playerID = id;
        EventHolder.TriggerEnergySystemInitialized(playerID, this);
    }

    private void OnEnable()
    {
        EventHolder.OnEnergyRequest += HandleEnergyRequest;
    }

    private void OnDisable()
    {
        EventHolder.OnEnergyRequest -= HandleEnergyRequest;
    }

    private void Update()
    {
        RegenerateEnergy();
    }

    private void RegenerateEnergy()
    {
        currentEnergy = Mathf.Min(currentEnergy + energyRegenRate * Time.deltaTime, maxEnergy);
        EventHolder.TriggerEnergyUpdated(playerID, currentEnergy, maxEnergy);
    }

    private void HandleEnergyRequest(int requestedPlayerID, int amount, System.Action<bool> callback)
    {
        if (requestedPlayerID != playerID) return;

        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            EventHolder.TriggerEnergyUpdated(playerID, currentEnergy, maxEnergy);
            callback(true);
        }
        else
        {
            callback(false);
        }
    }

    public float GetCurrentEnergy() => currentEnergy;
    public float GetMaxEnergy() => maxEnergy;
}
