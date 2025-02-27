using UnityEngine;

public class Attacker : MonoBehaviour, IUnit
{
    private AttackerData attackerData; 

    public void Initialize(UnitData data)
    {
        attackerData = data as AttackerData; 

        if (attackerData == null)
        {
            Debug.LogError("Invalid AttackerData assigned!");
        }
    }

    public void Activate()
    {
        Debug.Log($"Attacker activated with normal speed: {attackerData.normalSpeed}, " +
                  $"carrying speed: {attackerData.carryingSpeed}, pass speed: {attackerData.passSpeed}");
    }

    public void Deactivate()
    {
        Debug.Log("Attacker deactivated.");
    }

    public float GetSpeed()
    {
        return attackerData.normalSpeed;
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
