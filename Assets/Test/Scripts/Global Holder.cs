using System;
using UnityEngine;

/// <summary>
/// Holds global events used throughout the game.
/// </summary>
public static class EventHolder
{
    //Game Flow Events
    public static event Action OnGameStart; 
    public static event Action<Action<Vector3>> OnRequestLandPosition;
    public static event Action<Action<Vector3>> OnRequestGoalPosition;
    public static event Action<LandType, Action<Transform>> OnRequestParentLand;
    //Ball Tracking Events
    public static event Action<Action<Vector3, bool>> OnRequestBallStatus; 
    public static event Action OnBallPickedUp; 
    //Unit & Energy Events
    public static event Action<Vector3, LandType, int> OnInputReceived;
    public static event Action<int, int, Action<bool>> OnEnergyRequest;
    public static event Action<int, EnergySystem> OnEnergySystemInitialized;
    public static event Action<int, float, float> OnEnergyUpdated;
    public static event Action<UnitType, Action<UnitData>> OnUnitDataRequest;
    //Turn Related Events
    public static event Action OnTurnSwitch;
    public static event Action<int> OnScropeUpdated;
    public static event Action<Action<int>> OnRequestTurnInfo;
    public static event Action OnTurnReset;

    //Game Flow Event Triggers
    public static void TriggerTurnReset()
    {
        OnTurnReset?.Invoke();
    }
    
    public static void TriggerGameStart()
    {
        OnGameStart?.Invoke();
    }

    public static void TriggerRequestLandPosition(Action<Vector3> callback)
    {
        OnRequestLandPosition?.Invoke(callback);
    }

    public static void TriggerRequestGoalPosition(Action<Vector3> callback)
    {
        OnRequestGoalPosition?.Invoke(callback);
    }

    public static void TriggerRequestParentLand(LandType land,  Action<Transform> callback)
    {
        OnRequestParentLand?.Invoke(land, callback);
    }

    //Ball Tracking Event Triggers
    public static void TriggerRequestBallStatus(Action<Vector3, bool> callback)
    {
        OnRequestBallStatus?.Invoke(callback);
    }

    public static void TriggerBallPickedUp()
    {
        OnBallPickedUp?.Invoke();
    }

    //Unit & Energy Event Triggers
    public static void TriggerInputReceived(Vector3 position, LandType landType, int playerID)
    {
        OnInputReceived?.Invoke(position, landType, playerID);
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

    public static void TriggerEndTurn()
    {
        OnTurnSwitch?.Invoke();
    }

    public static void TriggerScoreUpdate(int playerID)
    {
        OnScropeUpdated?.Invoke(playerID);
    }

    public static void TriggerRequestTurnInfo(Action<int> callback)
    {
        OnRequestTurnInfo?.Invoke(callback);
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
    void ResetRotation();
}

public interface IInteractable
{
    void Interact(Unit interactingUnit);
}

public enum DefenderState
{
    Standby,
    Chasing,
    Inactive
}

public class InteractionWrapper : MonoBehaviour, IInteractable
{
    private InteractionHandler interactionHandler;

    public void Initialize(InteractionHandler handler)
    {
        interactionHandler = handler;
    }

    public void Interact(Unit interactingUnit)
    {
        interactionHandler?.Interact(interactingUnit);
    }
}



