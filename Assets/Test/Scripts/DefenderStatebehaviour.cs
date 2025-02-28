using UnityEngine;
using System.Collections;

public class DefenderStateBehaviour
{
    private Defender defender;
    private Vector3 originalPosition;
    private DefenderData defenderData;
    private GameObject detectionRangeObject;
    private DefenderState currentState = DefenderState.Standby;

    public DefenderStateBehaviour(Defender defender, DefenderData data, GameObject detectionRangeObj)
    {
        this.defender = defender;
        this.defenderData = data;
        this.detectionRangeObject = detectionRangeObj;
        originalPosition = defender.transform.position;
        SetStandbyState();
    }

    public void UpdateState()
    {
        switch (currentState)
        {
            case DefenderState.Standby:
                break;
            case DefenderState.Chasing:
                ChaseTarget();
                break;
            case DefenderState.Inactive:
                MoveBackToOriginalPosition();
                break;
        }
    }

    private void SetStandbyState()
    {
        currentState = DefenderState.Standby;
        detectionRangeObject.SetActive(true);
        defender.Stationary();
    }

    private void SetChasingState(Attacker target)
    {
        currentState = DefenderState.Chasing;
        detectionRangeObject.SetActive(false);
        defender.SetTarget(target);
    }

    private void SetInactiveState()
    {
        Debug.Log("inactive state reached");
        currentState = DefenderState.Inactive;
        detectionRangeObject.SetActive(false);
        defender.IgnoreCollisions(true);
        defender.StartCoroutine(ReactivateAfterTime());
    }

    private void ChaseTarget()
    {
        if (defender.Target == null)
        {
            SetStandbyState();
            return;
        }

        Vector3 direction = (defender.Target.transform.position - defender.transform.position).normalized;
        direction.y = 0f;
        defender.Move(direction, defenderData.normalSpeed);
    }

    private void MoveBackToOriginalPosition()
    {
        Vector3 direction = (originalPosition - defender.transform.position).normalized;
        direction.y = 0f;
        defender.Move(direction, defenderData.returnSpeed);

        if (Vector3.Distance(defender.transform.position, originalPosition) < 0.1f)
        {
            SetStandbyState();
            defender.IgnoreCollisions(false);
        }
    }

    private IEnumerator ReactivateAfterTime()
    {
        yield return new WaitForSeconds(defenderData.reactivateTime);
        SetStandbyState();
        defender.IgnoreCollisions(false);
    }

    public void OnAttackerDetected(Attacker attacker)
    {
        if (currentState == DefenderState.Standby)
        {
            SetChasingState(attacker);
        }
    }

    public void OnAttackerCaught()
    {
        Debug.Log("setting Inactive State");
        SetInactiveState();
    }
}
