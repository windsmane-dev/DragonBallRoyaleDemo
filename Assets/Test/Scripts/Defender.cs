using UnityEngine;

public class Defender : Unit
{
    private DefenderData defenderData;

    public override void Initialize(UnitData data)
    {
        defenderData = data as DefenderData;
        if (defenderData == null)
        {
            Debug.LogError("Invalid DefenderData assigned!");
            return;
        }

        base.Initialize(data);
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
