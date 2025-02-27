using UnityEngine;

public class Land : MonoBehaviour, ISelectable
{
    [SerializeField] private LandType landType;

    private void OnEnable()
    {
        TurnManager.OnTurnSwitch += SwitchRole;
    }

    private void OnDisable()
    {
        TurnManager.OnTurnSwitch -= SwitchRole;
    }

    public void OnSelect(Vector3 position)
    {
        EventHolder.TriggerInputReceived(position, landType);
    }

    private void SwitchRole()
    {
        landType = (landType == LandType.Attacker) ? LandType.Defender : LandType.Attacker;
        Debug.Log($"Land role switched to: {landType}");
    }
}
