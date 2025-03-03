using UnityEngine;

public class YAxisRotationController : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float sensitivity = 0.1f;
    public Space rotationSpace = Space.World;
    public bool invertHorizontal = false;

    private Vector2 initialInputPosition;
    private bool isInteracting;
    private int activeFingerId = -1;

    void Update()
    {
        if (IsMobilePlatform())
        {
            HandleTouchInput();
        }
        else
        {
            HandleMouseInput();
        }
    }

    bool IsMobilePlatform()
    {
        return Application.isMobilePlatform;
    }

    void HandleTouchInput()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && !isInteracting)
            {
                StartInteraction(touch.position, touch.fingerId);
            }
            else if (touch.fingerId == activeFingerId)
            {
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    UpdateDelta(touch.position);
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    EndInteraction();
                }
            }
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && !isInteracting)
        {
            StartInteraction(Input.mousePosition, -1);
        }

        if (isInteracting)
        {
            if (Input.GetMouseButton(0))
            {
                UpdateDelta(Input.mousePosition);
            }
            else
            {
                EndInteraction();
            }
        }
    }

    void StartInteraction(Vector2 position, int fingerId)
    {
        initialInputPosition = position;
        isInteracting = true;
        activeFingerId = fingerId;
    }

    void UpdateDelta(Vector2 currentPosition)
    {
        Vector2 delta = currentPosition - initialInputPosition;
        ApplyRotation(CalculateRotation(delta));
    }

    float CalculateRotation(Vector2 delta)
    {
        float horizontalMultiplier = invertHorizontal ? -1 : 1;
        return delta.x * sensitivity * Time.deltaTime * horizontalMultiplier;
    }

    void ApplyRotation(float yRotation)
    {
        transform.Rotate(0, yRotation, 0, rotationSpace);
        transform.position += transform.forward * 1f * Time.deltaTime;
    }

    void EndInteraction()
    {
        isInteracting = false;
        activeFingerId = -1;
    }
}