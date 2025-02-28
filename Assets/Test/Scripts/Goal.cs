using UnityEngine;

public class Goal : MonoBehaviour, IInteractable
{
    public void Interact(Unit interactingUnit)
    {
        if (interactingUnit is Attacker attacker && attacker.HasBall())
        {
            Debug.Log("Goal reached! Ending turn.");
            EventHolder.TriggerEndTurn();
        }
    }
}
