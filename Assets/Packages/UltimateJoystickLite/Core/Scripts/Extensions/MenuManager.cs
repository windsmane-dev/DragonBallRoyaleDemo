using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using FormatGames.VirtualJoystick.Lite;


public class MenuManager : MonoBehaviour
{

    static void CreateDefaultJoystick(int type, string fileName)
    {
        GameObject currentSelection = Selection.activeGameObject;
        Canvas canvas = currentSelection != null ? currentSelection.GetComponent<Canvas>() : FindObjectOfType<Canvas>();

        if (type == 0)
        {
            // If there is no canvas in the scene, create one
            if (canvas == null && currentSelection == null)
            {
                canvas = CreateCanvas();
            }

            GameObject parent = currentSelection != null ? currentSelection : canvas.gameObject;

            // Create the joystick and set its parent
            GameObject joystickGameObject = new GameObject("Ultimate Joystick Lite");
            joystickGameObject.AddComponent<RectTransform>();
            joystickGameObject.transform.SetParent(parent.transform, false);
            joystickGameObject.AddComponent<Joystick>();

            Selection.activeGameObject = joystickGameObject;
        }

        // prefab

        else
        {


            // Find and adjust Prefab Path

            string path = JoystickTools.GetPrefabPath<GameObject>(fileName);
           

            // Load Prefab

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);


            // Check Canvas



            // If there is no canvas in the scene, create one
            if (canvas == null && currentSelection == null)
            {
                canvas = CreateCanvas();
            }

            GameObject parent = currentSelection != null ? currentSelection : canvas.gameObject;


            if (prefab != null)
            {
                // Instantiate the prefab at the target position and rotation
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                instance.transform.SetParent(parent.transform, false);
                Selection.activeGameObject = instance;
            }
        }
    }


    [MenuItem("GameObject/Joystick (FG)/Joystick", false, 10)]

    static void CreateJoystick()
    {
        CreateDefaultJoystick(0,"");
    }
    
    [MenuItem("GameObject/Joystick (FG)/Default Joystick (Prefab)", false, 10)]
    static void CreateDefaultJoystick()
    {

        CreateDefaultJoystick(1, "DefaultJoystick");
    }

    // Add a menu item named "Create Joystick" to the GameObject menu in Unity
    [MenuItem("GameObject/Joystick (FG)/Touch Area Joystic (Prefab)", false, 10)]

    static void CreateTouchAreaJoystick()
    {
        CreateDefaultJoystick(1, "TouchAreaJoystick");
    }

    static Canvas CreateCanvas()
    {
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();
        return canvas;
    }
}