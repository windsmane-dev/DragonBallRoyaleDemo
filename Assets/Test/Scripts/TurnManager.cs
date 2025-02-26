using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{
    public static event Action OnTurnSwitch;
    private int currentTurn = 1;

    public void SwitchTurn()
    {
        currentTurn = (currentTurn == 1) ? 2 : 1;
        OnTurnSwitch?.Invoke();
        Debug.Log($"Turn switched! Now it's Player {currentTurn}'s turn.");
    }

    public int GetCurrentTurn()
    {
        return currentTurn;
    }
}
