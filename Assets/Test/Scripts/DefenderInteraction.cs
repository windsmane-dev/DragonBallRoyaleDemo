using UnityEngine;

public class DefenderInteraction :InteractionHandler
{
    private Defender defender;
    public DefenderInteraction(Defender unit) : base(unit) { defender = unit; }

    public override void Interact(Unit interactingUnit)
    {
        if (interactingUnit is Attacker attacker && attacker.HasBall())
        {
            Debug.Log("Caught Enemy");
            defender.SendMessage("OnAttackerCaught", interactingUnit as Attacker);
        }
    }


}
