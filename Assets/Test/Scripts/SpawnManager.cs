using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private EnergySystem player1Energy;
    private EnergySystem player2Energy;
    private TurnManager turnManager;

    public void Initialize(EnergySystem p1Energy, EnergySystem p2Energy, TurnManager turnMgr)
    {
        player1Energy = p1Energy;
        player2Energy = p2Energy;
        turnManager = turnMgr;
    }

    private void OnEnable()
    {
        EventHolder.OnInputReceived += HandleInput;
    }

    private void OnDisable()
    {
        EventHolder.OnInputReceived -= HandleInput;
    }

    private void HandleInput(Vector3 spawnPosition, LandType landType)
    {
        int currentPlayerID = turnManager.GetCurrentTurn(); // Get current player's turn
        EnergySystem energySystem = (currentPlayerID == 1) ? player1Energy : player2Energy; // Select correct energy system

        UnitType unitType = (landType == LandType.Attacker) ? UnitType.Attacker : UnitType.Defender;

        // Request energy deduction from the correct player
        EventHolder.TriggerEnergyRequest(currentPlayerID, 2, (success) =>
        {
            if (success)
            {
                IUnit unit = GameManager.Instance.UnitFactory.CreateUnit(unitType, spawnPosition, Quaternion.identity);
                unit.Activate();
            }
        });
    }
}
