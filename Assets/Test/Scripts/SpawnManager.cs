using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private EnergySystem player1Energy;
    private EnergySystem player2Energy;

    public void Initialize(EnergySystem p1Energy, EnergySystem p2Energy)
    {
        player1Energy = p1Energy;
        player2Energy = p2Energy;
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
        int playerID = (landType == LandType.Attacker) ? 1 : 2;
        EnergySystem energy = (playerID == 1) ? player1Energy : player2Energy;
        UnitType unitType = (landType == LandType.Attacker) ? UnitType.Attacker : UnitType.Defender;

        EventHolder.TriggerUnitDataRequest(unitType, (unitData) =>
        {
            if (unitData == null)
            {
                Debug.LogError($"No UnitData found for {unitType}!");
                return;
            }

            
            EventHolder.TriggerEnergyRequest(playerID, unitData.energyCost, (success) =>
            {
                if (success)
                {
                    GameObject unitObject = GameManager.Instance.UnitFactory.CreateUnit(unitType, spawnPosition, Quaternion.identity);

                    if (unitObject.TryGetComponent<IUnit>(out IUnit unit))
                    {
                        unit.Initialize(unitData); 
                        unit.Activate();
                    }
                    else
                    {
                        Debug.LogError($"Spawned unit of type {unitType} does not implement IUnit!");
                    }
                }
            });
        });
    }
}
