using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif
namespace FormatGames.VirtualJoystick.Lite
{
    public class HierarchyManager
    {

#if UNITY_EDITOR


        public void CreateHierarrchy(Transform parent, Joystick joystick)
        {

            //------------------------------------- INSTANTIATE -----------------------------------------//

            GameObject Object_ = new GameObject();

            GameObject BodyInstance = GameObject.Instantiate(Object_, parent);
            GameObject BackGroundInstance = GameObject.Instantiate(Object_, BodyInstance.transform);
            GameObject HandleShadowInstance = GameObject.Instantiate(Object_, BodyInstance.transform);
            GameObject ComplementsInstance = GameObject.Instantiate(Object_, BodyInstance.transform);
            GameObject HandleInstance = GameObject.Instantiate(Object_, BodyInstance.transform);

            RectTransform BodyRect = BodyInstance.AddComponent<RectTransform>();
            RectTransform ComplementsRect = ComplementsInstance.AddComponent<RectTransform>();
            RectTransform BackGroundRect = BackGroundInstance.AddComponent<RectTransform>();
            RectTransform HandleShadowRect = HandleShadowInstance.AddComponent<RectTransform>();
            RectTransform HandleRect = HandleInstance.AddComponent<RectTransform>();

            Image BackGroundIMG = BackGroundInstance.AddComponent<Image>();
            Image HandleShadowIMG = HandleShadowInstance.AddComponent<Image>();
            Image HandleIMG = HandleInstance.AddComponent<Image>();

            BodyInstance.AddComponent<CanvasGroup>();


            BackGroundIMG.color = Color.white;
            BackGroundIMG.raycastTarget = false;

            HandleShadowIMG.color = Color.black;
            HandleShadowIMG.raycastTarget = false;

            HandleIMG.color = Color.white;
            HandleIMG.raycastTarget = false;


            BodyRect.gameObject.name = "Body";
            ComplementsRect.gameObject.name = "Complements";
            BackGroundRect.gameObject.name = "BackGround";
            HandleShadowRect.gameObject.name = "HandleShadow";
            HandleRect.gameObject.name = "Handle";

            joystick.JoystickCanvas = BodyInstance.GetComponent<CanvasGroup>();

            if (!parent.GetComponent<RectTransform>()) { parent.gameObject.AddComponent<RectTransform>(); }

            joystick.MainParent = parent.GetComponent<RectTransform>();
            joystick.Body = BodyRect;
            joystick.BackGround = BackGroundIMG;
            joystick.HandleShadow = HandleShadowIMG;
            joystick.Handle = HandleIMG;


            GameObject.DestroyImmediate(Object_);


            //------------------------------------- RESIZE -----------------------------------------//

            float JoystickSize = 400f;
            float HandleSize = 150f;
            float HandleShadowSize = 150f;

            /*Joystick*/

            BodyRect.sizeDelta = new Vector2(JoystickSize, JoystickSize);

            /*Complements*/

            ComplementsRect.offsetMin = new Vector2(0, 0);
            ComplementsRect.offsetMax = new Vector2(0, 0);
            ComplementsRect.anchorMin = new Vector2(0, 0);
            ComplementsRect.anchorMax = new Vector2(1, 1);

            /*BackGround*/

            BackGroundRect.offsetMin = new Vector2(0, 0);
            BackGroundRect.offsetMax = new Vector2(0, 0);
            BackGroundRect.anchorMin = new Vector2(0, 0);
            BackGroundRect.anchorMax = new Vector2(1, 1);

            /*Handle*/

            HandleShadowRect.sizeDelta = new Vector2(HandleShadowSize, HandleShadowSize);
            HandleRect.sizeDelta = new Vector2(HandleSize, HandleSize);



            
   
              // Textures


            // Define paths for the background and handle sprites

            string backgroundPath = JoystickTools.GetSpritePath<Sprite>("DefaultBackground");
            string handlePath = JoystickTools.GetSpritePath<Sprite>("DefaultHandle");



            // Function to create a sprite from a file path
            Sprite CreateSpriteFromPath(string imagePath)
            {
                byte[] imageBytes = File.ReadAllBytes(imagePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageBytes);
                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }

            // Set sprites for the background and handle
            BackGroundIMG.sprite = CreateSpriteFromPath(backgroundPath);
            Sprite handleSprite = CreateSpriteFromPath(handlePath);
            HandleShadowIMG.sprite = handleSprite;
            HandleIMG.sprite = handleSprite;
             
             




            BodyInstance.hideFlags = HideFlags.HideInHierarchy;
        }

#endif

        public float LocalOffsetToWorldPosition(RectTransform target, string Axis, string factor,float offset)
        {
            float Handle_Offset_x;
            Vector3 Handle_Offset = new Vector3(0,0,0);

            switch (Axis)
            {
                case "x":
                

                    if (factor == "-")
                    {
                        Handle_Offset_x = target.localPosition.x - offset;
                    }
                    else
                    {
                        Handle_Offset_x = target.localPosition.x + offset;
                    }

                    Handle_Offset = new Vector3(Handle_Offset_x, 0, 0);

                    break;

                case "y":

                    if (factor == "-")
                    {
                        Handle_Offset_x = target.localPosition.y - offset;
                    }
                    else
                    {
                        Handle_Offset_x = target.localPosition.y + offset;
                    }

                    Handle_Offset = new Vector3(0, Handle_Offset_x, 0);

                    break;

                case "z":

                    if (factor == "-")
                    {
                        Handle_Offset_x = target.localPosition.z - offset;
                    }
                    else
                    {
                        Handle_Offset_x = target.localPosition.z + offset;
                    }

                    Handle_Offset = new Vector3(0, 0, Handle_Offset_x);

                    break;
            }

            Vector3 Handle_Offset_World = target.transform.TransformPoint(Handle_Offset);


            return Vector3.Distance(target.transform.position, Handle_Offset_World);

            
        }
    }

}
