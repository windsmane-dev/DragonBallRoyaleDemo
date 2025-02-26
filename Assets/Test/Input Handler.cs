using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // Get mouse position in screen space
            Vector3 mousePosition = Input.mousePosition;



            // Convert to world space
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;



            // Perform raycast
            if (Physics.Raycast(ray, out hit))
            {

                // "hit.point" now contains the exact point on the GameObject where the mouse is hovering
                EventHolder.onClicked.Invoke(hit.point);

            }
        }
    }
}
