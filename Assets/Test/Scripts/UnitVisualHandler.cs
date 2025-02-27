using UnityEngine;

public class UnitVisualHandler
{
    private Renderer unitRenderer;

    public UnitVisualHandler(GameObject unitObject)
    {
        unitRenderer = unitObject.GetComponent<Renderer>();

        if (unitRenderer == null)
        {
            Debug.LogError("No Renderer found on unit!");
        }
    }

    public void SetUnitColor(Color color)
    {
        if (unitRenderer != null)
        {
            unitRenderer.material.color = color;
        }
    }
}
