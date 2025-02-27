using System;
using UnityEngine;

/// <summary>
/// Holds global events used throughout the game.
/// </summary>
public static class EventHolder
{
    public static event Action OnGameStart;
    public static event Action<Action<Vector3>> OnRequestLandPosition;

    public static event Action<Vector3, LandType> OnInputReceived;
    public static event Action<int, int, Action<bool>> OnEnergyRequest;
    public static event Action<int, EnergySystem> OnEnergySystemInitialized;
    public static event Action<int, float, float> OnEnergyUpdated;
    public static event Action<UnitType, Action<UnitData>> OnUnitDataRequest;

    public static void TriggerGameStart()
    {
        OnGameStart?.Invoke();
    }

    public static void TriggerRequestLandPosition(Action<Vector3> callback)
    {
        OnRequestLandPosition?.Invoke(callback);
    }

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

    public static void TriggerUnitDataRequest(UnitType unitType, Action<UnitData> callback)
    {
        OnUnitDataRequest?.Invoke(unitType, callback);
    }
}


/// <summary>
/// Defines the selectable objects in the game.
/// </summary>
public interface ISelectable
{
    void OnSelect(Vector3 position);
}

/// <summary>
/// Defines the base interface for all unit types.
/// </summary>
public interface IUnit
{
    void Initialize(UnitData data);
    void Activate();
    void Deactivate();
}

/// <summary>
/// Defines how input is processed.
/// </summary>
public interface IInputReader
{
    void ProcessInput();
}

/// <summary>
/// Represents the type of land in the game.
/// </summary>
public enum LandType
{
    Attacker,
    Defender
}

/// <summary>
/// Represents the different types of units.
/// </summary>
public enum UnitType
{
    Attacker,
    Defender
}

/// <summary>
/// Interface to handle movement of units
/// </summary>
public interface IMovable
{
    void Tick(); 
    void ChangeSpeed(float newSpeed); 
    void ChangeDirection(Vector3 newDirection); 
}

