using UnityEngine;

public class Defender : MonoBehaviour, IUnit
{
    public void Activate()
    {
        gameObject.SetActive(true);
        GetComponent<Renderer>().material.color = Color.blue;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
