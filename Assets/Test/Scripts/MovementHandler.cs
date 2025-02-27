using UnityEngine;

public class MovementHandler : IMovable
{
    private Transform unitTransform;
    private float speed;
    private Vector3 direction;

    public MovementHandler(Transform transform, float initialSpeed, Vector3 initialDirection)
    {
        unitTransform = transform;
        speed = initialSpeed;
        direction = initialDirection;
    }

    public void Tick()
    {
        unitTransform.position += direction * speed * Time.deltaTime;
    }

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ChangeDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }
}
