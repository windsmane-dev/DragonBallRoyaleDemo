using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static FormatGames.VirtualJoystick.Lite.Joystick;

namespace FormatGames.VirtualJoystick.Lite
{

    public class JoystickTools : MonoBehaviour
    {

        public void SetTransition(Joystick joystick, bool startTransition)
        {
            if (!joystick.enabled) { return; }

            if (joystick.transitionType == TransitionType.Default)
            {
                joystick.JoystickCanvas.enabled = false;
            }

            else
            {
                joystick.JoystickCanvas.enabled = true;

                if (startTransition)
                {
                    switch (joystick.transitionType)
                    {
                        case Joystick.TransitionType.ShowOnTouch:

                            joystick.JoystickCanvas.alpha = 1;

                            break;

                        case Joystick.TransitionType.HighlightOnTouch:

                            joystick.JoystickCanvas.alpha = joystick.MaximumHighlight * 0.01f;

                            break;
                    }

                    return;
                }


                switch (joystick.transitionType)
                {
                    case Joystick.TransitionType.ShowOnTouch:

                        joystick.JoystickCanvas.alpha = 0;

                        break;

                    case Joystick.TransitionType.HighlightOnTouch:

                        joystick.JoystickCanvas.alpha = joystick.MinimumHighlight * 0.01f;

                        break;
                }
            }


        }

        public Vector2 GetAxis(Joystick joystick)
        {
            Vector2 axis = new Vector2();

            switch (joystick.axisType)
            {
                case Joystick.AxisType.Angle:

                    axis = ((Vector2)joystick.Handle.rectTransform.position - joystick.TOUCH_POSITION).normalized;

                    break;

                case Joystick.AxisType.Cross:

                    switch (joystick.DIRECTION)
                    {
                        case Joystick.Direction.UP:

                            axis = new Vector2(0, 1);

                            break;

                        case Joystick.Direction.DOWN:

                            axis = new Vector2(0, -1);

                            break;

                        case Joystick.Direction.RIGHT:

                            axis = new Vector2(1, 0);

                            break;

                        case Joystick.Direction.LEFT:

                            axis = new Vector2(-1, 0);

                            break;
                    }

                    break;
            }

            switch (joystick.axisConstraints)
            {

                case Joystick.AxisConstraints.Horizontal:

                    Vector2 horizontal = new Vector2(axis.x, 0);

                    axis = horizontal;

                    break;

                case Joystick.AxisConstraints.Vertical:

                    Vector2 vertical = new Vector2(0, axis.y);

                    axis = vertical;

                    break;

            }

            return axis;
        }

        public void GetDirection(Joystick joystick)
        {
            //--------------------------- DIRECTION -----------------------------//

            Vector2 TOUCH_POSITION = joystick.TOUCH_POSITION;
            Vector2 Handle_position = joystick.Handle.rectTransform.position;

            Vector2 Axis = ((Vector2)joystick.Handle.rectTransform.position - TOUCH_POSITION).normalized;


            if (Axis != new Vector2(0, 0))
            {
                float Vertical = Mathf.Abs(Axis.y);
                float Horizontal = Mathf.Abs(Axis.x);

                if (Vertical > Horizontal)
                {

                    if (TOUCH_POSITION.y - Handle_position.y < 0)
                    {
                        joystick.DIRECTION = Joystick.Direction.UP;

                    }
                    else if (TOUCH_POSITION.y - Handle_position.y > 0)
                    {
                        joystick.DIRECTION = Joystick.Direction.DOWN;

                    }
                }
                else if (Horizontal > Vertical)
                {
                    if (TOUCH_POSITION.x - Handle_position.x < 0)
                    {
                        joystick. DIRECTION = Joystick.Direction.RIGHT;

                    }
                    else if (TOUCH_POSITION.x - Handle_position.x > 0)
                    {
                        joystick.DIRECTION = Joystick.Direction.LEFT;

                    }
                }
            }

            //--------------------------- ANGLE -----------------------------//

            GetAngle(joystick);

            //--------------------------- Handle Distance -----------------------------//


            //HandleDistance = Vector2.Distance(Handle.rectTransform.anchoredPosition, Vector2.zero);
        }
        public void GetAngle(Joystick joystick)
        {
            //ANGLE = Mathf.Atan2(Axis.y, Axis.x) * Mathf.Rad2Deg;

            Vector2 screenPointA = RectTransformUtility.WorldToScreenPoint(null, joystick.BackGround.transform.position);
            Vector2 screenPointB = RectTransformUtility.WorldToScreenPoint(null, joystick.Handle.transform.position);
            Vector2 direction = screenPointB - screenPointA;

            joystick.ANGLE = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        public bool inBounds(Vector3 touch, RectTransform rect)
        {
            bool inside = false;

            Vector2 touchpos = rect.InverseTransformPoint(touch);

            if (rect.rect.Contains(touchpos))
            {
                return true;
            }

            return inside;
        }

#if UNITY_EDITOR

        public static string GetScriptPath<T>() where T : class
        {
            string[] guids = AssetDatabase.FindAssets(typeof(T).Name + " t:script");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return Path.GetFullPath(path);
            }
            return null;
        }

        public static string GetPrefabPath<T>(string prefabName) where T : class
        {
            // Busca activos con el nombre especificado y que sean prefabs
            string[] guids = AssetDatabase.FindAssets(prefabName + " t:prefab");
            if (guids.Length > 0)
            {
                // Obtén la ruta del primer prefab encontrado
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return path; //return Path.GetFullPath(path);
            }
            return null;
        }

        public static string GetSpritePath<T>(string imageName) where T : class
        {
            // Busca activos con el nombre especificado y que sean de tipo Texture2D
            string[] guids = AssetDatabase.FindAssets(imageName + " t:Texture2D");
            if (guids.Length > 0)
            {
                // Obtén la ruta del primer activo encontrado
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return path;
            }
            return null;
        }
#endif
    }
}
