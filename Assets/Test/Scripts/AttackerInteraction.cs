using UnityEngine;
using System.Collections.Generic;

public class AttackerInteraction : InteractionHandler
{
    private Attacker attacker;

    public AttackerInteraction(Attacker unit) : base(unit)
    {
        attacker = unit;
    }

    public override void Interact(Unit interactingUnit)
    {
        if (interactingUnit is Defender defender && attacker.HasBall())
        {
            PassBallToNearestAttacker();
        }
    }

    private void PassBallToNearestAttacker()
    {
        List<GameObject> activeAttackers = GameManager.Instance.UnitFactory.GetActiveUnits(UnitType.Attacker);
        activeAttackers.Remove(attacker.gameObject); 

        if (activeAttackers.Count == 0)
        {
            Debug.Log("No attackers left to receive the ball. Ending turn.");
            EventHolder.TriggerEndTurn(); 
            return;
        }

        
        List<Attacker> validAttackers = new List<Attacker>();
        foreach (GameObject attackerObj in activeAttackers)
        {
            if (attackerObj.TryGetComponent<Attacker>(out Attacker otherAttacker) && otherAttacker.IsActive())
            {
                validAttackers.Add(otherAttacker);
            }
        }

        if (validAttackers.Count == 0)
        {
            Debug.Log("No active attackers left. Ending turn.");
            EventHolder.TriggerEndTurn();
            return;
        }

        
        Attacker nearestAttacker = null;
        float minDistance = float.MaxValue;

        foreach (Attacker validAttacker in validAttackers)
        {
            float distance = Vector3.Distance(attacker.transform.position, validAttacker.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestAttacker = validAttacker;
            }
        }

        if (nearestAttacker != null)
        {
            BallController ball = attacker.GetComponentInChildren<BallController>();
            if (ball != null)
            {
                ball.PassToAttacker(nearestAttacker.transform, attacker.GetPassSpeed());
                attacker.LoseBall();
            }
        }
        else
        {
            Debug.Log("No valid attackers found. Ending turn.");
            EventHolder.TriggerEndTurn();
        }
    }


}
