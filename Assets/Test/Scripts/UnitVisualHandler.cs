using UnityEngine;

public class UnitVisualHandler
{
    private Renderer unitRenderer;
    private Animator anim;
    GameObject parentObject;
    
    public UnitVisualHandler(GameObject unitObject, Animator _anim)
    {
        unitRenderer = unitObject.GetComponent<Renderer>();
        anim = _anim;
        parentObject = unitObject;
        if (unitRenderer == null)
        {
            Debug.LogError("No Renderer found on unit!");
        }
    }

    public void SetUnitMaterial(Material mat)
    {
        if (unitRenderer != null)
        {
            unitRenderer.material = mat;
        }
    }

    public void OnActivate(GameObject particle)
    {
        anim.SetBool("Inactive", false);
        anim.SetBool("Active", true);

        GameObject.Instantiate(particle, parentObject.transform.parent.position, Quaternion.identity);
    }

    public void OnDeactivate(GameObject particle)
    {
        anim.SetBool("Inactive", true);
        anim.SetBool("Active", false);
        GameObject.Instantiate(particle, parentObject.transform.parent.position, Quaternion.identity);
    }

    public void OnSpawn(GameObject particle)
    {
        GameObject.Instantiate(particle, parentObject.transform.parent.position, Quaternion.identity);
    }

    public void OnDespawn(GameObject particle)
    {
        GameObject.Instantiate(particle, parentObject.transform.parent.position, Quaternion.identity);
    }
}
