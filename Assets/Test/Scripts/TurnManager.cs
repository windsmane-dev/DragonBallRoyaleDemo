using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{

    private int currentTurn = 1;

    private void OnEnable()
    {
        EventHolder.OnTurnSwitch += SwitchTurn;
    }
    private void OnDisable()
    {
        EventHolder.OnTurnSwitch -= SwitchTurn;
    }
    public void SwitchTurn()
    {
        currentTurn = (currentTurn == 1) ? 2 : 1;
        Debug.Log($"Turn switched! Now it's Player {currentTurn}'s turn.");

        EventHolder.TriggerTurnReset();
    }

    public int GetCurrentTurn()
    {
        return currentTurn;
    }
}
