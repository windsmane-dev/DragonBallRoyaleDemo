using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{

    private int currentTurn = 1;

    private int TotalTurns;
    private void OnEnable()
    {
        EventHolder.OnTurnSwitch += SwitchTurn;
        EventHolder.OnRequestTurnInfo += ProvideTurnInfo; 
    }

    private void ProvideTurnInfo(Action<int> callback)
    {
        callback(GetCurrentTurn());

    }

    private void OnDisable()
    {
        EventHolder.OnTurnSwitch -= SwitchTurn;
        EventHolder.OnRequestTurnInfo -= ProvideTurnInfo;
    }
    public void SwitchTurn()
    {
        currentTurn = (currentTurn == 1) ? 2 : 1;
        Debug.Log($"Turn switched! Now it's Player {currentTurn}'s turn.");
        TotalTurns++;

        if (TotalTurns < 5)
            EventHolder.TriggerTurnReset();
        else
            EventHolder.TriggerGameEndCheck();
    }

    public int GetCurrentTurn()
    {
        return currentTurn;
    }
}
