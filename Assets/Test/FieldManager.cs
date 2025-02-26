using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventHolder.onClicked.AddListener(CheckInputLocation);
    }

    private void OnDestroy()
    {
        EventHolder.onClicked.RemoveListener(CheckInputLocation);
    }

    void CheckInputLocation(Vector3 pos, TouchType touchType)
    {

    }
}
