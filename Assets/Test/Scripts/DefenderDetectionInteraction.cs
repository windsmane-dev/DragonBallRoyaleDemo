using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderDetectionInteraction : MonoBehaviour, IInteractable
{
    Defender defender;
    public void Interact(Unit interactingUnit)
    {
        if (interactingUnit is Attacker attacker && attacker.HasBall())
        {
            defender.SendMessage("OnAttackerDetected", attacker);
        }
    }

    void SetDefenderObject( Defender d)
    {
        defender = d;
    }
}
