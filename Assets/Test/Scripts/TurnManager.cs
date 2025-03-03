using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{

    private int currentTurn = 1;

    private int TotalTurns = 1; // as we always start the turn from 1, not zero. 
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
        EventHolder.TriggerTotalTurnCountUpdate(TotalTurns);
        if (TotalTurns <= 5)
            EventHolder.TriggerTurnReset();
        else
            EventHolder.TriggerGameEndCheck();
    }

    public int GetCurrentTurn()
    {
        return currentTurn;
    }
}
