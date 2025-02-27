using System.Collections;
using UnityEngine;

public class Attacker : Unit
{
    private IMovable movementHandler;
    private AttackerData attackerData;

    public override void Initialize(UnitData data)
    {
        attackerData = data as AttackerData;
        if (attackerData == null)
        {
            Debug.LogError("Invalid AttackerData assigned!");
            return;
        }

        base.Initialize(data);
        movementHandler = new MovementHandler(transform, data.normalSpeed, Vector3.forward);
    }

    public override void Activate()
    {
        base.Activate();
        StartCoroutine(Tick());
        EventHolder.TriggerRequestBallStatus(SetChaseBall);

    }

    IEnumerator Tick()
    {
        while(isActive)
        {
            movementHandler.Tick();
            yield return new WaitForSeconds(Time.deltaTime);
        }    
    }

    public void SetChaseBall(Vector3 ballPosition, bool isPickedUp)
    {
        if(!isPickedUp)
            movementHandler.ChangeDirection((ballPosition - transform.position).normalized);
    }

    public void MoveStraight()
    {
        movementHandler.ChangeDirection(Vector3.forward);
    }
}
