using UnityEngine;

public class InteractionHandler : IInteractable
{
    protected Unit unit;

    public InteractionHandler(Unit unit)
    {
        this.unit = unit;
    }

    public virtual void Interact(Unit interactingUnit)
    {
        // Default interaction behavior (can be overridden in subclasses)
    }
}
