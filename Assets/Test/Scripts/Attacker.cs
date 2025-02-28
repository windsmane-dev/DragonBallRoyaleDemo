using System.Collections;
using UnityEngine;

public class Attacker : Unit
{
    private IMovable movementHandler;
    private AttackerData attackerData;
    private AttackerInteraction interactionHandler;
    private bool hasBall = false;
    [SerializeField] private Transform ballPosition;

    private Transform parentLand;

    bool isSubscribedToEvent;
    public override void Initialize(UnitData data)
    {
        attackerData = data as AttackerData;
        if (attackerData == null)
        {
            Debug.LogError("Invalid AttackerData assigned!");
            return;
        }

        base.Initialize(data);
        EventHolder.TriggerRequestParentLand(LandType.Attacker, parentTransfom => { parentLand = parentTransfom; });
        movementHandler = new MovementHandler(transform, data.normalSpeed, Vector3.forward, parentLand);
        InteractionWrapper wrapper = gameObject.AddComponent<InteractionWrapper>();
        interactionHandler = new AttackerInteraction(this);
        wrapper.Initialize(interactionHandler);

        
    }

    public override void Activate()
    {
        base.Activate();
        StartCoroutine(Tick());
        EventHolder.TriggerRequestBallStatus(SetChaseBall);
        EventHolder.OnBallPickedUp += OnBallPickedUp;
        isSubscribedToEvent = true;
    }

    private void OnDisable()
    {
        if(isSubscribedToEvent)
        EventHolder.OnBallPickedUp -= OnBallPickedUp;
    }

    IEnumerator Tick()
    {
        while (isActive)
        {
            movementHandler.Tick();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactable.Interact(this);
        }
    }

    public void SetChaseBall(Vector3 ballPosition, bool isPickedUp)
    {
        if (!isPickedUp)
        {
            movementHandler.ChangeDirection((ballPosition - transform.position).normalized);
        }
    }

    private void OnBallPickedUp()
    {
        if (!hasBall)
        {
            ResetMovement();
        }
    }

    public void PickUpBall()
    {
        hasBall = true;
        gameObject.layer = LayerMask.NameToLayer("PlayerWithBall");
        movementHandler.ChangeSpeed(attackerData.carryingSpeed);
        EventHolder.TriggerRequestGoalPosition(MoveTowardsGoal);
    }

    private void MoveTowardsGoal(Vector3 position)
    {
        movementHandler.ChangeDirection(position - transform.position);
    }

    private void ResetMovement()
    {
        movementHandler.ResetRotation();
    }


    public bool HasBall()
    {
        return hasBall;
    }

    public void LoseBall()
    {
        hasBall = false;
        movementHandler.ChangeSpeed(0);
        gameObject.layer = LayerMask.NameToLayer("Player");
        EventHolder.OnBallPickedUp -= OnBallPickedUp;
        isSubscribedToEvent = false;

        StartSpawnTimer(attackerData.reactivateTime);
    }
    public float GetPassSpeed()
    {
        return attackerData.passSpeed;
    }
    public Transform GetBallHoldPosition()
    {
        return ballPosition;
    }

    public bool IsActive()
    {
        return isActive;
    }

}
