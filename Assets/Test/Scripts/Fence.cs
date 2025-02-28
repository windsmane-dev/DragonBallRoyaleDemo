using UnityEngine;

public class Fence : MonoBehaviour, IInteractable
{
    public void Interact(Unit interactingUnit)
    {
        if (interactingUnit is Attacker attacker && !attacker.HasBall())
        {
            Debug.Log("Attacker reached the fence and is removed.");
            GameManager.Instance.UnitFactory.ReturnUnit(attacker.gameObject);
        }
    }
}
