using UnityEngine;

public abstract class UnitData : ScriptableObject // Made abstract to prevent direct instantiation
{
    public UnitType unitType; // Defines the type of unit (Attacker, Defender)
    public int energyCost; // Energy required to spawn
    public float spawnTime; // Time required for spawning
    public float reactivateTime; // Time before the unit can act again
    public float normalSpeed; // Movement speed
}
