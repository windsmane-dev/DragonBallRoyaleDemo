using System;
using UnityEngine;

public class BallController : MonoBehaviour, IPickup
{
    private bool isPickedUp = false;

    private void OnEnable()
    {
        EventHolder.OnRequestBallStatus += ProvideBallStatus;
    }

    private void OnDisable()
    {
        EventHolder.OnRequestBallStatus -= ProvideBallStatus;
    }

    private void ProvideBallStatus(Action<Vector3, bool> callback)
    {
        callback(transform.position, isPickedUp);
    }

    public void PickUp(Transform newParent)
    {
        if (!isPickedUp)
        {
            isPickedUp = true;
            transform.SetParent(newParent);
            transform.localPosition = Vector3.zero;
            EventHolder.TriggerBallPickedUp();
        }
    }

    public bool IsPickedUp()
    {
        return isPickedUp;
    }
}
