using UnityEngine;

public class Attacker : Unit
{
    private AttackerData attackerData;

    public override void Initialize(UnitData data)
    {
        attackerData = data as AttackerData;
        if (attackerData == null)
        {
            Debug.LogError("Invalid AttackerData assigned!");
            return;
        }

        base.Initialize(data);
    }

    public float GetCarryingSpeed()
    {
        return attackerData.carryingSpeed;
    }

    public float GetPassSpeed()
    {
        return attackerData.passSpeed;
    }
}
