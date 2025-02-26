using UnityEngine;

public class Land : MonoBehaviour, ISelectable
{
    [SerializeField] private LandType landType;

    public void OnSelect(Vector3 position)
    {
        EventHolder.TriggerInputReceived(position, landType);
    }

    public void SwitchRole()
    {
        landType = (landType == LandType.Attacker) ? LandType.Defender : LandType.Attacker;
    }
}
