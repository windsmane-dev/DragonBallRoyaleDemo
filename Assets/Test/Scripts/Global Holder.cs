using System;
using UnityEngine;

public static class EventHolder
{
    public static event Action<Vector3, LandType> OnInputReceived;
    public static event Action<int, int, Action<bool>> OnEnergyRequest;
    public static event Action<int, EnergySystem> OnEnergySystemInitialized;
    public static event Action<int, float, float> OnEnergyUpdated;

    public static void TriggerInputReceived(Vector3 position, LandType landType)
    {
        OnInputReceived?.Invoke(position, landType);
    }

    public static void TriggerEnergyRequest(int playerID, int amount, Action<bool> callback)
    {
        OnEnergyRequest?.Invoke(playerID, amount, callback);
    }

    public static void TriggerEnergySystemInitialized(int playerID, EnergySystem energySystem)
    {
        OnEnergySystemInitialized?.Invoke(playerID, energySystem);
    }

    public static void TriggerEnergyUpdated(int playerID, float currentEnergy, float maxEnergy)
    {
        OnEnergyUpdated?.Invoke(playerID, currentEnergy, maxEnergy);
    }
}
public interface IUnit
{
    void Activate();
    void Deactivate();
}

public interface ISelectable
{
    void OnSelect(Vector3 position);
}

public enum LandType
{
    Attacker,
    Defender
}

public enum UnitType
{
    Attacker,
    Defender
}

public interface IInputReader
{
    void ProcessInput();
}


