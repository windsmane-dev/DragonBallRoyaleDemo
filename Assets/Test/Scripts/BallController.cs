using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour, IInteractable
{
    private bool isPickedUp = false;
    private Transform targetAttacker;
    private float passSpeed = 0f;

    private void OnEnable()
    {
        EventHolder.OnRequestBallStatus += ProvideBallStatus;
    }

    private void OnDisable()
    {
        EventHolder.OnRequestBallStatus -= ProvideBallStatus;
    }

    private void ProvideBallStatus(System.Action<Vector3, bool> callback)
    {
        callback(transform.position, isPickedUp);
    }

    public void PickUp(Transform newParent)
    {
        if (!isPickedUp)
        {
            StopAllCoroutines();

            isPickedUp = true;
            transform.SetParent(newParent);
            transform.localPosition = Vector3.zero;
            EventHolder.TriggerBallPickedUp();
        }
    }

    public void PassToAttacker(Transform attacker, float speed)
    {
        if (attacker == null)
        {
            Debug.LogError("No valid attacker to pass to.");
            return;
        }

        isPickedUp = false;
        targetAttacker = attacker;
        passSpeed = speed;
        transform.SetParent(null);
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        while(targetAttacker != null)
        {
            MoveTowardsTarget();
            yield return new WaitForSeconds(Time.deltaTime);
        }
       
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetAttacker.position, passSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetAttacker.position) < 0.1f)
        {
            PickUp(targetAttacker);
            targetAttacker = null;
            StopAllCoroutines();
        }
    }

    public void Interact(Unit interactingUnit)
    {
        if (interactingUnit is Attacker attacker)
        {
            attacker.SendMessage("PickUpBall");
            PickUp(attacker.GetBallHoldPosition());
        }
    }

    private void ResetBall()
    {
        StopAllCoroutines();

        isPickedUp = false;
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
        transform.SetParent(null);
        StopAllCoroutines();
    }

    void Init(Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        transform.rotation = Quaternion.identity;
    }
}
