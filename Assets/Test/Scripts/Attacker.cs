using UnityEngine;
public class Attacker : MonoBehaviour, IUnit
{
    public void Activate()
    {
        gameObject.SetActive(true);
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
