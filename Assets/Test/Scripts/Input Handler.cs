using UnityEngine;

public class InputHandler : MonoBehaviour, IInputReader
{
    public void ProcessInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<ISelectable>(out ISelectable selectable))
                {
                    selectable.OnSelect(hit.point);
                }
            }
        }
    }

    private void Update()
    {
        ProcessInput();
    }
}
