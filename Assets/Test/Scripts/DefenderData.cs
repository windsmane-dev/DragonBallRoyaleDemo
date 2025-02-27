using UnityEngine;

[CreateAssetMenu(fileName = "DefenderData", menuName = "Game Data/Defender Data")]
public class DefenderData : UnitData
{
    public float returnSpeed; // Speed when returning to position
    public float detectionRange; // Detection radius to chase attackers
}
