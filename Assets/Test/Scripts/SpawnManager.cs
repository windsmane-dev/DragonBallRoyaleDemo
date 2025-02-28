using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private EnergySystem player1Energy;
    private EnergySystem player2Energy;
    private GameObject ballPrefab;
    private GameObject ballObject;

    public void Initialize(EnergySystem p1Energy, EnergySystem p2Energy, GameObject ball)
    {
        player1Energy = p1Energy;
        player2Energy = p2Energy;
        ballPrefab = ball;
    }

    private void OnEnable()
    {
        EventHolder.OnInputReceived += HandleInput;
        EventHolder.OnGameStart += RequestBallSpawnPosition;
        EventHolder.OnTurnReset += ResetField;
    }

    private void OnDisable()
    {
        EventHolder.OnInputReceived -= HandleInput;
        EventHolder.OnGameStart -= RequestBallSpawnPosition;
        EventHolder.OnTurnReset -= ResetField;
    }

    private void HandleInput(Vector3 spawnPosition, LandType landType, int playerID)
    {
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
                    }
                    else
                    {
                        Debug.LogError($"Spawned unit of type {unitType} does not implement IUnit!");
                    }
                }
            });
        });
    }

    private void RequestBallSpawnPosition()
    {
        EventHolder.TriggerRequestLandPosition(SpawnBall);
    }

    private void SpawnBall(Vector3 spawnPosition)
    {
        if (ballPrefab == null)
        {
            Debug.LogError("Ball prefab is missing in SpawnManager!");
            return;
        }

        if(ballObject == null)
        {
            ballObject = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            ballObject.SetActive(true);
            ballObject.SendMessage("Init", spawnPosition);
        }
        
        Debug.Log($"Ball spawned at {spawnPosition}");
    }

    private void ResetField()
    {
        Debug.Log("Resetting field: Returning all units to pool and removing ball.");
        ballObject.SendMessage("ResetBall");
        ballObject.SetActive(false);
        GameManager.Instance.UnitFactory.ReturnAllUnits();
        Invoke("RequestBallSpawnPosition", 0.5f);
    }
}
