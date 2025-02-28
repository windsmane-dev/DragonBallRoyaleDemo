using UnityEngine;

public class MovementHandler : IMovable
{
    private Transform unitTransform;
    private float speed;
    private Transform parentLand;
    public MovementHandler(Transform transform, float initialSpeed, Vector3 initialDirection, Transform parentLandTransform)
    {
        unitTransform = transform;
        speed = initialSpeed;
        parentLand = parentLandTransform;
        //ChangeDirection(initialDirection); // Ensures the unit is facing the correct direction initially
        ResetRotation();
    }

    public void Tick()
    {
        unitTransform.position += unitTransform.forward * speed * Time.deltaTime;
    }

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ChangeDirection(Vector3 newDirection)
    {
        if (newDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(newDirection.x, 0f, newDirection.z).normalized);
            unitTransform.rotation = targetRotation;
        }
    }

    public void ResetRotation()
    {
        unitTransform.rotation = parentLand.transform.rotation;
    }
}
