using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Events;

namespace FormatGames.VirtualJoystick.Lite
{

#if UNITY_EDITOR

    [CustomEditor(typeof(Joystick))]
    public class Editor : UnityEditor.Editor
    {
        public GUIStyle FoldoutStyle;

        private HierarchyManager m_HierarchyManager;

        void OnDestroy()
        {

#if UNITY_EDITOR


            if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (Time.frameCount != 0 && Time.renderedFrameCount != 0)//not loading scene
                {
                    Joystick virtualJoystick = (target) as Joystick;

                    if (!virtualJoystick)
                    {
                        if (virtualJoystick.Body)
                        {
                            DestroyImmediate(virtualJoystick.Body.gameObject);
                        }
                    }
                }
            }
#endif
        }

        void OnEnable()
        {
            if (FoldoutStyle == null)
            {
                FoldoutStyle = new GUIStyle(EditorStyles.foldout);

                FoldoutStyle.fixedHeight = 0;
                FoldoutStyle.fixedWidth = 1;
            }
        }

        void OnSceneGUI()
        {
            if(Application.isPlaying) { return; }


            Joystick virtualJoystick = (target) as Joystick;

            m_HierarchyManager ??= new();

            float Handle_Ratio = m_HierarchyManager.LocalOffsetToWorldPosition(virtualJoystick.Handle.rectTransform,"x", "-", (virtualJoystick.Ratio / 2));
            float Handle_Size_Clamp = m_HierarchyManager.LocalOffsetToWorldPosition(virtualJoystick.Handle.rectTransform,"x", "-", (virtualJoystick.Handle.rectTransform.sizeDelta.x / 2));


            // indicates the handle position will be clamped in the middle of red circle line
            Handles.color = Color.red;
            //Handles.DrawWireDisc(virtualJoystick.Body.transform.position, Vector3.forward, virtualJoystick.Ratio);

            Handles.DrawWireDisc(virtualJoystick.Body.transform.position, Vector3.forward, Handle_Ratio);

            // indicates the handle position will beclamped inside white circle bounds
            Handles.color = Color.white;
            Handles.DrawWireDisc(virtualJoystick.Body.transform.position, Vector3.forward, Handle_Ratio + Handle_Size_Clamp);


            // DEAD ZONE : indicates joystick will return the axis once the handle position overpass the blue circle
            Handles.color = Color.blue;
            Handles.DrawWireDisc(virtualJoystick.Body.transform.position, Vector3.forward, Handle_Ratio * virtualJoystick.DeadZone / 100);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Joystick virtualJoystick = (target) as Joystick;

            Undo.RecordObject(virtualJoystick, "Virtual Joystick Change");

            Undo.RecordObject(virtualJoystick.Body, "Virtual Joystick Change");
            Undo.RecordObject(virtualJoystick.Handle, "Virtual Joystick Change");
            Undo.RecordObject(virtualJoystick.HandleShadow, "Virtual Joystick Change");
            Undo.RecordObject(virtualJoystick.BackGround, "Virtual Joystick Change");

            if (virtualJoystick.ShowInspector)
            {
                GUILayout.Space(14);
                DrawDefaultInspector();
                GUILayout.Space(14);
            }

            Bar(virtualJoystick, "joystick", "Joystick", false);

            if (virtualJoystick.Unfold)
            {
                GUILayout.Space(8);
                EditorGUILayout.LabelField("Types");
                GUILayout.Space(8);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("axisType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("axisConstraints"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("joystickType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("transitionType"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("disabledMode"));


                // CONFIGURATION


                GUILayout.Space(8);EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);GUILayout.Space(8);

                virtualJoystick.Ratio = (float)EditorGUILayout.Slider("Ratio", virtualJoystick.Ratio, 0, virtualJoystick.JoystickSize * 2);
                virtualJoystick.DeadZone = (float)EditorGUILayout.Slider("DeadZone", virtualJoystick.DeadZone, 1, 100);
               
                virtualJoystick.JoystickSize = (float)EditorGUILayout.Slider("Background Size", virtualJoystick.JoystickSize, 10, 900);
                virtualJoystick.BackgroundAlpha = (float)EditorGUILayout.Slider("Background Alpha", virtualJoystick.BackgroundAlpha, 0, 100);
                virtualJoystick.HandleShadowSize = (float)EditorGUILayout.Slider("Handle Shadow Size", virtualJoystick.HandleShadowSize, 10, 400);
                virtualJoystick.HandleShadowAlpha = (float)EditorGUILayout.Slider("Handle Shadow Alpha", virtualJoystick.HandleShadowAlpha, 0, 100);
                virtualJoystick.HandleSize = (float)EditorGUILayout.Slider("Handle Size", virtualJoystick.HandleSize, 10, 400);
                virtualJoystick.HandleAlpha = (float)EditorGUILayout.Slider("Handle Alpha", virtualJoystick.HandleAlpha, 0, 100);

                GUILayout.Space(8);

                // ADVANZED

                if (virtualJoystick.disabledMode != Joystick.DisabledMode.Invisible)
                {
                    GUILayout.Space(8); EditorGUILayout.LabelField("Faint Alpha", EditorStyles.boldLabel); GUILayout.Space(8);

                    virtualJoystick.MinimumDisabledAlpha = (float)EditorGUILayout.Slider("Disabled Alpha", virtualJoystick.MinimumDisabledAlpha, 1, 100);
                }


                if (virtualJoystick.transitionType != Joystick.TransitionType.Default)
                {
                    GUILayout.Space(8); EditorGUILayout.LabelField("Transition Alpha", EditorStyles.boldLabel); GUILayout.Space(8);

                    virtualJoystick.MinimumHighlight = (float)EditorGUILayout.Slider("Minimum Highlight", virtualJoystick.MinimumHighlight, 1, 100);
                    virtualJoystick.MaximumHighlight = (float)EditorGUILayout.Slider("Maximum Highlight", virtualJoystick.MaximumHighlight, 1, 100);
                }

                // CONDITIONS

                GUILayout.Space(8); EditorGUILayout.LabelField("Settings Options", EditorStyles.boldLabel); GUILayout.Space(8);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("AnimatedMode"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("ShowInspector"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("OnRealaseAxis"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TrackLastDirection"));

                if (virtualJoystick.AnimatedMode)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("RotateBackground"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("RotateHandle"));
                }

                if (virtualJoystick.AnimatedMode)
                {
                    virtualJoystick.ResetHandleSpeed = (float)EditorGUILayout.Slider("Reset Handle Speed", virtualJoystick.ResetHandleSpeed, 1, 20);
                }


                // REFRENCES


                GUILayout.Space(8);EditorGUILayout.LabelField("References", EditorStyles.boldLabel); GUILayout.Space(8);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("TouchArea"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Debugger"));


                // APPEARANCE

                GUILayout.Space(8);EditorGUILayout.LabelField("Appearance", EditorStyles.boldLabel);GUILayout.Space(8);

                virtualJoystick.BackGround.sprite = (Sprite)EditorGUILayout.ObjectField("BackGround", virtualJoystick.BackGround.sprite, typeof(Sprite), false, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                virtualJoystick.HandleShadow.sprite = (Sprite)EditorGUILayout.ObjectField("Handle Shadow", virtualJoystick.HandleShadow.sprite, typeof(Sprite), false, GUILayout.Height(EditorGUIUtility.singleLineHeight));
                virtualJoystick.Handle.sprite = (Sprite)EditorGUILayout.ObjectField("Handle", virtualJoystick.Handle.sprite, typeof(Sprite), false, GUILayout.Height(EditorGUIUtility.singleLineHeight));

                virtualJoystick.BackGround.color = EditorGUILayout.ColorField("BackGround", virtualJoystick.BackGround.color);
                virtualJoystick.HandleShadow.color = EditorGUILayout.ColorField("Handle Shadow", virtualJoystick.HandleShadow.color);
                virtualJoystick.Handle.color = EditorGUILayout.ColorField("Handle", virtualJoystick.Handle.color);


                // CUSTOM EVENTS

                if (virtualJoystick.joystickType == Joystick.JoystickType.Custom)
                {
                    GUILayout.Space(8);
                    EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
                    GUILayout.Space(8);

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onTouch"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onRealase"));
                }

                GUILayout.Space(14);


                GUILayout.Space(8);
                EditorGUILayout.HelpBox(
                    "Warning: 'if the  TOUCH  events dont work in GameView, it must me due to Simulate Touch Input from Mouse or Pen' is not enabled. Please go to Window > Analysis > Input Debugger and enable it to test joystick functionality in Game View.",
                    MessageType.Warning);
                GUILayout.Space(14);
            }

            Bar(virtualJoystick, "arrow", "Arrow", false);

            if (GUI.changed)
            {
                if (virtualJoystick.Enable)
                {
                    virtualJoystick.Body.hideFlags = HideFlags.None;
                }
                else
                {
                    virtualJoystick.Body.hideFlags = HideFlags.HideInHierarchy;
                }

                virtualJoystick.UpdateConfig();

                EditorUtility.SetDirty(virtualJoystick);

                EditorUtility.SetDirty(virtualJoystick.Body);
                EditorUtility.SetDirty(virtualJoystick.Handle);
                EditorUtility.SetDirty(virtualJoystick.HandleShadow);
                EditorUtility.SetDirty(virtualJoystick.BackGround);

                serializedObject.ApplyModifiedProperties();

                //center joystick on the scene view

                GameObject selectedObject = Selection.activeGameObject;

                if (selectedObject != null)
                {
                    SceneView sceneView = SceneView.lastActiveSceneView;

                    if (sceneView != null && sceneView.in2DMode)
                    {
                        sceneView.LookAt(selectedObject.transform.position);
                    }
                }
            }
        }

        public void Bar(Joystick joystick, string property, string label, bool Bottom)
        {
            Color Background = new Color32(48, 48, 48, 200);
            Color Line = new Color32(0, 0, 0, 100);
            float ElemtnsSpace = 0;
            float BackGroundSize = 30;
            float DefaultTopSpace = 6;

            //-------------------- BACKGROUND ---------------------//

            Rect LineRect_top = EditorGUILayout.GetControlRect(false, 0);
            EditorGUI.DrawRect(new Rect(0, LineRect_top.y - 5, EditorGUIUtility.currentViewWidth, 0.5f), Line); // line

            Rect BackgroundRect = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(new Rect(0, BackgroundRect.y - DefaultTopSpace, EditorGUIUtility.currentViewWidth, BackGroundSize), Background); // Background



            GUILayout.Space(-8 + (BackGroundSize / DefaultTopSpace)); EditorGUILayout.BeginHorizontal(GUILayout.Width(20)); EditorGUILayout.BeginVertical(); GUILayout.Space(ElemtnsSpace + 2);



            //-------------------- FOULD OUT ---------------------//

            if (property == "joystick")
            {
                joystick.Unfold = EditorGUILayout.BeginFoldoutHeaderGroup(joystick.Unfold, GUIContent.none, FoldoutStyle);
                EditorGUILayout.EndFoldoutHeaderGroup();
            }



            EditorGUILayout.EndVertical(); EditorGUILayout.BeginVertical(); GUILayout.Space(ElemtnsSpace);



            //-------------------- TOGGLE ---------------------//

            if (property == "joystick")
            {
                joystick.Enable = EditorGUILayout.BeginToggleGroup("  " + label, joystick.Enable);
                EditorGUILayout.EndToggleGroup();
            }


            EditorGUILayout.EndVertical(); EditorGUILayout.EndHorizontal();

            if (Bottom)
            {
                GUILayout.Space(-2);
            }
            else
            {
                GUILayout.Space(8);
            }
        }
    }
   
#endif
}
