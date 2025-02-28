using System.Collections;
using UnityEngine;

public class Defender : Unit
{
    private IMovable movementHandler;
    private DefenderData defenderData;
    private DefenderInteraction interactionHandler;
    private DefenderStateBehaviour stateBehaviour;
    [SerializeField] private GameObject detectionRangeObject;
    private Attacker target;
    private Transform parentLand;
    public Attacker Target => target;

    public override void Initialize(UnitData data)
    {
        defenderData = data as DefenderData;
        if (defenderData == null)
        {
            Debug.LogError("Invalid DefenderData assigned!");
            return;
        }

        base.Initialize(data);
        EventHolder.TriggerRequestParentLand(LandType.Defender, parentTransfom => { parentLand = parentTransfom; });
        movementHandler = new MovementHandler(transform, data.normalSpeed, Vector3.forward, parentLand);
        InteractionWrapper wrapper = gameObject.AddComponent<InteractionWrapper>();
        interactionHandler = new DefenderInteraction(this);
        wrapper.Initialize(interactionHandler);
      
        SetDetectionRadius();
    }

    public override void Activate()
    {
        base.Activate();
        stateBehaviour = new DefenderStateBehaviour(this, defenderData, detectionRangeObject);
        StartCoroutine(Tick());
    }
    private void SetDetectionRadius()
    {
        float fieldWidth = GetFieldWidth();
        float detectionRadius = fieldWidth * (defenderData.detectionRange / 100f);
        detectionRangeObject.transform.localScale = Vector3.one * detectionRadius;
        detectionRangeObject.SendMessage("SetDefenderObject", this);
    }

    private float GetFieldWidth()
    {
        Land field = FindObjectOfType<Land>();
        return field.GetComponent<Collider>().bounds.size.x;
    }

    IEnumerator Tick()
    {
        while (isActive)
        {
            stateBehaviour.UpdateState();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator MovementTick()
    {
        while (isActive)
        {
            movementHandler.Tick();
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            Debug.Log(other.gameObject.name + "check");
            interactable.Interact(this);
        }
    }

    void OnAttackerDetected(Attacker attacker)
    {
        stateBehaviour.OnAttackerDetected(attacker);
        StartCoroutine(MovementTick());
    }
    public void Move(Vector3 direction, float speed)
    {
        movementHandler.ChangeDirection(direction);
        movementHandler.ChangeSpeed(speed);
    }

    public void SetTarget(Attacker newTarget)
    {
        target = newTarget;
    }

    public void OnAttackerCaught()
    {
        Debug.Log("Defender Script");
        stateBehaviour.OnAttackerCaught();
    }

    public void IgnoreCollisions(bool ignore)
    {
        GetComponent<Collider>().enabled = !ignore;
    }

    public void Stationary()
    {
        StopCoroutine(MovementTick());
    }

    public float GetReturnSpeed()
    {
        return defenderData.returnSpeed;
    }

    public float GetDetectionRange()
    {
        return defenderData.detectionRange;
    }
}
