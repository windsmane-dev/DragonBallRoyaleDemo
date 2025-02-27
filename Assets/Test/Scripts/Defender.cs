using UnityEngine;

public class Defender : MonoBehaviour, IUnit
{
    private DefenderData defenderData; 

    public void Initialize(UnitData data)
    {
        defenderData = data as DefenderData; 

        if (defenderData == null)
        {
            Debug.LogError("Invalid DefenderData assigned!");
        }
    }

    public void Activate()
    {
        Debug.Log($"Defender activated with normal speed: {defenderData.normalSpeed}, " +
                  $"return speed: {defenderData.returnSpeed}, detection range: {defenderData.detectionRange}");
    }

    public void Deactivate()
    {
        Debug.Log("Defender deactivated.");
    }

    public float GetSpeed()
    {
        return defenderData.normalSpeed; 
    }

    public float GetReturnSpeed()
    {
        return defenderData.returnSpeed;
    }

    public float GetDetectionRange()
    {
        return defenderData.detectionRange;
    }
}
