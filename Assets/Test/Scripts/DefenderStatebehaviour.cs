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

     void SetStandbyState()
    {
        currentState = DefenderState.Standby;
        detectionRangeObject.SetActive(true);
        defender.IgnoreCollisions(false);
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
        
        Vector3 direction = (originalPosition - defender.transform.position).normalized;
        direction.y = 0f;
        defender.Move(direction, defenderData.returnSpeed);

        defender.StartSpawnTimer(defenderData.reactivateTime);
        //defender.StartCoroutine(ReactivateAfterTime());
            
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
        if (Vector3.Distance(defender.transform.position, originalPosition) < 0.1f)
        {
            defender.Stationary();
        }
    }

    private IEnumerator ReactivateAfterTime()
    {
        
        if(defender.TryGetComponent<IUnit>(out var unit))
        {
            Debug.Log("Deactivating LULU");
            unit.Deactivate();
        }
        yield return new WaitForSeconds(defenderData.reactivateTime);
        defender.IgnoreCollisions(false);
        if(unit != null)
        {
            unit.Activate();
            Debug.Log("Activating LULU");
        }
        SetStandbyState();

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

    public void OnActivated()
    {
        SetStandbyState();
    }
}
