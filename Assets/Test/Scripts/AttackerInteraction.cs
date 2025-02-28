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

        // Find the closest attacker
        Attacker nearestAttacker = null;
        float minDistance = float.MaxValue;

        foreach (GameObject attackerObj in activeAttackers)
        {
            if (attackerObj.TryGetComponent<Attacker>(out Attacker otherAttacker) && !otherAttacker.HasBall())
            {
                float distance = Vector3.Distance(attacker.transform.position, otherAttacker.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestAttacker = otherAttacker;
                }
            }
        }

        if (nearestAttacker != null)
        {
            BallController ball = attacker.GetComponentInChildren<BallController>();
            if (ball != null)
            {
                ball.PassToAttacker(nearestAttacker.transform, attacker.GetPassSpeed());
                attacker.LoseBall();
                attacker.gameObject.layer = LayerMask.NameToLayer("Player");
            }
        }
        else
        {
            Debug.Log("No valid attackers found. Ending turn.");
            EventHolder.TriggerEndTurn();
        }
    }

}
