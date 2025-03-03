using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
#endif


namespace FormatGames.VirtualJoystick.Lite
{
    [AddComponentMenu("UI/Joystick")]
    public class Joystick : JoystickTools
    {
        #region VARIABLES

        #region PUBLIC


        [Serializable] public class OnTouch : UnityEvent { }
        [HideInInspector] public OnTouch onTouch = new OnTouch();
        [Serializable] public class OnRealase : UnityEvent { }
        [HideInInspector] public OnRealase onRealase = new OnRealase();

        [Serializable] public enum DisabledMode { Invisible, Faint };
        [HideInInspector] public DisabledMode disabledMode;
        [Serializable] public enum AxisType { Angle, Cross };
        [HideInInspector] public AxisType axisType;
        [Serializable] public enum JoystickType { Default, Custom };
        [HideInInspector] public JoystickType joystickType;
        [Serializable] public enum TransitionType { Default, ShowOnTouch, HighlightOnTouch };
        [HideInInspector] public TransitionType transitionType;
        [Serializable] public enum AxisConstraints { Default, Horizontal, Vertical };
        [HideInInspector] public AxisConstraints axisConstraints;



        [SerializeField, HideInInspector] public float JoystickSize = 180f;
        [SerializeField, HideInInspector] public float HandleShadowSize = 100f;
        [SerializeField, HideInInspector] public float HandleSize = 80f;

        [SerializeField, HideInInspector] public float BackgroundAlpha = 17f;
        [SerializeField, HideInInspector] public float HandleShadowAlpha = 100f;
        [SerializeField, HideInInspector] public float HandleAlpha = 100f;

        [SerializeField, HideInInspector] public float Ratio = 100f;
        [SerializeField, HideInInspector] public float DeadZone = 90f;
        [SerializeField, HideInInspector] public float ResetHandleSpeed = 90f;
        [SerializeField, HideInInspector] public float MinimumDisabledAlpha = 50;
        [SerializeField, HideInInspector] public float MinimumHighlight = 50;
        [SerializeField, HideInInspector] public float MaximumHighlight = 100;

        [SerializeField, HideInInspector] public bool ShowOnTouch;
        [SerializeField, HideInInspector] public bool HighLightOnTouch;
        [SerializeField, HideInInspector] public bool OnRealaseAxis;
        [SerializeField, HideInInspector] public bool RunOnDesktop;
        [SerializeField, HideInInspector] public bool ShowCursor;
        [SerializeField, HideInInspector] public bool TrackLastDirection;

        [SerializeField, HideInInspector] public bool AnimatedMode;
        [SerializeField, HideInInspector] public bool RotateBackground;
        [SerializeField, HideInInspector] public bool RotateHandle;

        #endregion

        #region READONLY

        public enum Direction { NONE, UP, DOWN, RIGHT, LEFT }
        public Direction DIRECTION;

        private int FINGERID = -1;
        public bool IS_USING;
        public bool IS_OUT_OF_DEADZONE;
        private bool IS_UPDATE = false;
        private bool IS_LATEUPDATE = false;
        private bool TOUCH_DETECTED;


        // ------------------- DEFAULT VAIRABLES ----------------//

        private float DEADZONE_ON_START;
        private float RATIO_ON_START;
        private float HANDLE_SIZE_ON_START;
        private float BACKGROUND_SIZE_ON_START;
        private float JOYSTICKCANVASALPHA_OLD;

        private Vector2 JOYSTICK_DEFAULT_POSITION;
        public Vector2 TOUCH_POSITION;
        [HideInInspector] public Vector2 AXIS;
        [HideInInspector] public Vector2 ONREALESE_AXIS;
        [HideInInspector] public float ANGLE;

        #endregion

        #region REFERENCES

        [SerializeField, HideInInspector] public Joystick virtualJoystick;

        [SerializeField, HideInInspector] public CanvasGroup JoystickCanvas;
        [SerializeField, HideInInspector] public RectTransform MainParent;
        [SerializeField, HideInInspector] public RectTransform Body;
        [SerializeField, HideInInspector] public RectTransform TouchArea;
        [SerializeField, HideInInspector] public Image BackGround;
        [SerializeField, HideInInspector] public Image HandleShadow;
        [SerializeField, HideInInspector] public Image Handle;

        [SerializeField, HideInInspector] public float calls;
        [SerializeField, HideInInspector] public Text Debugger;

        #endregion

        #region CUSTOM EDITOR

        [HideInInspector] public bool Unfold;
        [HideInInspector] public bool Enable;
        [SerializeField, HideInInspector] public bool ShowInspector;

        #endregion

        #endregion



        #region FUNCTIONS

        private void OnEnable()
        {
            isEnabled(true);

        }

        private void OnDisable()
        {
            isEnabled(false);
        }

        public void isEnabled( bool enabled)
        {
            if(enabled)
            {
                JoystickCanvas.enabled = false;
                JoystickCanvas.alpha = JOYSTICKCANVASALPHA_OLD;
            }
            else
            {
                float alpha = MinimumDisabledAlpha;
                JoystickCanvas.enabled = true;
                JOYSTICKCANVASALPHA_OLD = JoystickCanvas.alpha;
                alpha = disabledMode == Joystick.DisabledMode.Invisible ? MinimumDisabledAlpha * 0 : alpha;
                JoystickCanvas.alpha = alpha * 0.01f;
            }
        }

        void Awake()
        {
            isEnabled(this.enabled);


#if ENABLE_INPUT_SYSTEM

            EnhancedTouchSupport.Enable();
#endif

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                RunOnDesktop = false;

                return;
            }

            RunOnDesktop = true;

        }
        void Reset()
        {

#if UNITY_EDITOR
            if (gameObject.GetComponent<Joystick>() != this)
            {
                GameObject.DestroyImmediate(this);
                return;
            }

            if (gameObject.transform.childCount > 0)
            {
                foreach (Transform go in transform)
                {
                    if (go.name == "Body")
                    {
                        GameObject.DestroyImmediate(go.gameObject);

                        HierarchyManager hierarchy = new HierarchyManager();
                        hierarchy.CreateHierarrchy(this.transform, this);
                        UpdateConfig();
                    }
                }
            }

            else
            {
                HierarchyManager hierarchy = new HierarchyManager();
                hierarchy.CreateHierarrchy(this.transform, this);
                UpdateConfig();
                
            }
#endif
        }
        void Start()
        {

            Ratio = Ratio / 2;
            RATIO_ON_START = Ratio;
            DEADZONE_ON_START = Ratio * DeadZone / 100;

            HANDLE_SIZE_ON_START = HandleSize;
            BACKGROUND_SIZE_ON_START = JoystickSize;

            JOYSTICK_DEFAULT_POSITION = MainParent.anchoredPosition;

            Handle.rectTransform.localPosition = Vector3.zero;

            SetTransition(this, false);

            OnStart();
        }
        private void Update()
        {
            if (Debugger) { Debugger.text = AXIS.ToString(); }

            InputTouch();

            if (!IS_USING)
            {
                if (AnimatedMode && !IS_USING)
                {
                    Handle.rectTransform.localPosition = Vector2.Lerp(Handle.rectTransform.localPosition, Vector2.zero, Time.deltaTime * ResetHandleSpeed);
                }

                if (IS_UPDATE)
                {
                    IS_UPDATE = false;

                    if (!AnimatedMode)
                    {
                        Handle.rectTransform.localPosition = Vector3.zero;
                    }
                }
            }
        }
        private void LateUpdate()
        {
            if (!IS_USING)
            { 
                if (IS_LATEUPDATE)
                {
                    IS_LATEUPDATE = false;
                }
            }
        }
        public void UpdateConfig()
        {
            virtualJoystick = this;

            Body.GetComponent<RectTransform>().sizeDelta = new Vector2(JoystickSize, JoystickSize);
            HandleShadow.rectTransform.sizeDelta = new Vector2(HandleShadowSize, HandleShadowSize);
            Handle.rectTransform.sizeDelta = new Vector2(HandleSize, HandleSize);


            BackGround.color = new Color(BackGround.color.r, BackGround.color.g, BackGround.color.b, BackgroundAlpha * 0.01f);
            HandleShadow.color = new Color(HandleShadow.color.r, HandleShadow.color.g, HandleShadow.color.b, HandleShadowAlpha * 0.01f);
            Handle.color = new Color(Handle.color.r, Handle.color.g, Handle.color.b, HandleAlpha * 0.01f);             

        }

        private void InputTouch()
        {

    #if ENABLE_INPUT_SYSTEM

                if (RunOnDesktop)
                {
                    DesktopInput();
                }
                else
                {
                    NewInputSystem();
                }
    #else
                if(RunOnDesktop)
                {
                        DesktopInput();
                }
                else
                {
                        OldInputSystem();
                }  
    #endif
        }



        public void NewInputSystem()
        {
#if ENABLE_INPUT_SYSTEM

            if (Touch.activeFingers.Count <= 0) { return; }

            // Iterate through all the detected touches


            for (int i = 0; i < Touch.activeFingers.Count; i++)
            {

                Touch touch = Touch.activeTouches[i];

                Vector2 TouchPosition = touch.finger.screenPosition;
                int fingerId = touch.finger.index;


                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        AXIS = Vector2.zero;

                        if (!IS_USING)
                        {
                            if (TouchArea)
                            {
                                if (!TouchArea.gameObject.activeSelf)
                                {
                                    if (inBounds(TouchPosition, Handle.rectTransform) && FINGERID == -1)
                                    {
                                        SetTransition(this, true);

                                        Down(TouchPosition);
                                        FINGERID = fingerId;
                                        TOUCH_DETECTED = true;
                                    }
                                    else
                                    {
                                        TOUCH_DETECTED = false;
                                    }

                                    return;
                                }

                                if (inBounds(TouchPosition, TouchArea))
                                {

                                    //------------------------ CHECK IF TOUCH COLLIDED WITH A UI ELEMENT -------------------------//

                                    if (!inBounds(TouchPosition, Handle.rectTransform) && EventSystem.current.IsPointerOverGameObject(fingerId))
                                    {
                                        TOUCH_DETECTED = false;

                                        return;
                                    }

                                    //---------------------------------- OTHERWISE CONTINUE -------------------------------------//

                                    SetTransition(this, true);

                                    MainParent.position = TouchPosition;

                                    if (FINGERID == -1)
                                    {
                                        FINGERID = fingerId;
                                        TOUCH_POSITION = TouchPosition;
                                        TOUCH_DETECTED = true;
                                    }
                                }
                                else
                                {
                                    TOUCH_DETECTED = false;
                                }
                            }
                            else if (!TouchArea)
                            {
                                if (inBounds(TouchPosition, Handle.rectTransform) && FINGERID == -1)
                                {
                                    SetTransition(this, true);

                                    Down(TouchPosition);
                                    FINGERID = fingerId;
                                    TOUCH_DETECTED = true;
                                }
                                else
                                {
                                    TOUCH_DETECTED = false;
                                }
                            }
                        }

                        break;
                    case TouchPhase.Ended:


                        if (fingerId == FINGERID)
                        {

                            FINGERID = -1;
                            Drop(TouchPosition);
                        }


                        break;
                    case TouchPhase.Canceled:




                        break;
                    case TouchPhase.Moved:

                        if (fingerId == FINGERID)
                        {
                            Drag(TouchPosition);
                        }

                        break;
                    case TouchPhase.Stationary:


                        break;
                }
            }

#endif
        }

        private void DesktopInput()
        {
#if !ENABLE_INPUT_SYSTEM || true
            Vector2 TouchPosition = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {

                AXIS = Vector2.zero;

                if (!IS_USING)
                {
                    if (TouchArea)
                    {

                        if (!TouchArea.gameObject.activeSelf)
                        {
                            if (inBounds(TouchPosition, Handle.rectTransform))
                            {
                                SetTransition(this, true);

                                Down(TouchPosition);

                                TOUCH_DETECTED = true;
                            }
                            else
                            {
                                TOUCH_DETECTED = false;
                            }

                            return;
                        }


                        if (inBounds(TouchPosition, TouchArea))
                        {

                            //------------------------ CHECK IF TOUCH COLLIDED WITH A UI ELEMENT -------------------------//

                            if (!inBounds(TouchPosition, Handle.rectTransform) && EventSystem.current.IsPointerOverGameObject())
                            {
                                TOUCH_DETECTED = false;

                                return;
                            }

                            //---------------------------------- OTHERWISE CONTINUE -------------------------------------//

                            SetTransition(this, true);

                            MainParent.position = TouchPosition;

                            TOUCH_POSITION = TouchPosition;

                            TOUCH_DETECTED = true;
                        }
                        else
                        {
                            TOUCH_DETECTED = false;
                        }
                    }
                    else
                    {
                        if (inBounds(TouchPosition, Handle.rectTransform))
                        {
                            SetTransition(this, true);

                            Down(TouchPosition);
                            TOUCH_DETECTED = true;
                        }
                        else
                        {
                            TOUCH_DETECTED = false;
                        }
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                Drag(TouchPosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Drop(TouchPosition);
            }

#endif
        }

        private void OldInputSystem()
        {
#if !ENABLE_INPUT_SYSTEM
            if (Input.touchCount <= 0) { return; }

            // Iterate through all the detected touches

            for (int i = 0; i < Input.touchCount; i++)
            {

                UnityEngine.Touch touch = Input.GetTouch(i);

                Vector2 TouchPosition = touch.position;
                int fingerId = touch.fingerId;

            switch (touch.phase)
                {
                    case TouchPhase.Began:

                        AXIS = Vector2.zero;

                        if (!IS_USING)
                        {
                            if (TouchArea)
                            {
                                if (!TouchArea.gameObject.activeSelf)
                                {
                                    if (inBounds(TouchPosition, Handle.rectTransform) && FINGERID == -1)
                                    {
                                        SetTransition(this, true);

                                        Down(TouchPosition);
                                        FINGERID = fingerId;
                                        TOUCH_DETECTED = true;
                                    }
                                    else
                                    {
                                        TOUCH_DETECTED = false;
                                    }

                                    return;
                                }

                                if (inBounds(TouchPosition, TouchArea))
                                {

                                    //------------------------ CHECK IF TOUCH COLLIDED WITH A UI ELEMENT -------------------------//

                                    if (!inBounds(TouchPosition, Handle.rectTransform) && EventSystem.current.IsPointerOverGameObject(fingerId))
                                    {
                                        TOUCH_DETECTED = false;

                                        return;
                                    }

                                    //---------------------------------- OTHERWISE CONTINUE -------------------------------------//

                                    SetTransition(this, true);

                                    MainParent.position = TouchPosition;

                                    if (FINGERID == -1)
                                    {
                                        FINGERID = fingerId;
                                        TOUCH_POSITION = TouchPosition;
                                        TOUCH_DETECTED = true;
                                    }
                                }
                                else
                                {
                                    TOUCH_DETECTED = false;
                                }
                            }
                            else if (!TouchArea)
                            {
                                if (inBounds(TouchPosition, Handle.rectTransform) && FINGERID == -1)
                                {
                                    SetTransition(this, true);

                                    Down(TouchPosition);
                                    FINGERID = fingerId;
                                    TOUCH_DETECTED = true;
                                }
                                else
                                {
                                    TOUCH_DETECTED = false;
                                }
                            }
                        }

                        break;

                    case TouchPhase.Moved:

                        if (fingerId == FINGERID)
                        {
                            Drag(TouchPosition);
                        }

                        break;

                    case TouchPhase.Ended:

                        if (fingerId == FINGERID)
                        {

                            FINGERID = -1;
                            Drop(TouchPosition);
                        }

                        break;

                }
            }
#endif
        }




        private void Down(Vector3 touch)
        {
            TOUCH_POSITION = touch;

            if (joystickType == JoystickType.Custom) onTouch.Invoke();
        }
        private void Drag(Vector3 touch)
        {
            if (!TOUCH_DETECTED) return;

            IS_USING = true;
            IS_UPDATE = true;
            IS_LATEUPDATE = true;

            //////////////////////////////////////////////////////////// Handle Follow Touch position /////////////////////////////////////////////////////////

            if (Body.sizeDelta.x != BACKGROUND_SIZE_ON_START || Handle.rectTransform.sizeDelta.x != HANDLE_SIZE_ON_START)
            {
                float FixedRatio = ((Body.sizeDelta.x / 2) - (BACKGROUND_SIZE_ON_START / 2)) - ((Handle.rectTransform.sizeDelta.x / 2) - (HANDLE_SIZE_ON_START / 2));

                Ratio = RATIO_ON_START + FixedRatio;
            }

            Vector2 axisConstraintsPosition = Vector2.zero;

            switch (axisConstraints)
            {
                case AxisConstraints.Default:

                    axisConstraintsPosition = touch;

                    break;

                case AxisConstraints.Horizontal:

                    axisConstraintsPosition = new Vector2(touch.x, Handle.rectTransform.position.y);

                    break;

                case AxisConstraints.Vertical:

                    axisConstraintsPosition = new Vector2(Handle.rectTransform.position.x, touch.y);

                    break;
            }


            Handle.rectTransform.position = axisConstraintsPosition;
            Handle.rectTransform.anchoredPosition = Vector3.ClampMagnitude(Handle.rectTransform.localPosition, Ratio);


            if (AnimatedMode)
            {
                Vector3 AngleRotation = new Vector3(0, 0, ANGLE);

                if (RotateBackground)
                {
                    BackGround.rectTransform.eulerAngles = AngleRotation;
                }

                if (RotateHandle)
                {
                    Handle.rectTransform.eulerAngles = new Vector3(0, 0, ANGLE);
                }
            }


            /////////////////////////////////////////////////////////////////// RETURN AXIS ////////////////////////////////////////////////////////////////////



            if (Vector2.Distance(Vector2.zero, Handle.rectTransform.localPosition) >= DEADZONE_ON_START)
            {
                IS_OUT_OF_DEADZONE = true;

                if (!OnRealaseAxis)
                {
                    AXIS = GetAxis(this);
                }
                else
                {
                    ONREALESE_AXIS = GetAxis(this);
                }
            }
            else
            {
                IS_OUT_OF_DEADZONE = false;
                AXIS = Vector2.zero;
                ONREALESE_AXIS = Vector2.zero;

            }

            GetDirection(this);


        }
        private void Drop(Vector3 touch)
        {
            if (!TOUCH_DETECTED) return;

            SetTransition(this, false);

            BackGround.rectTransform.eulerAngles = new Vector3(0, 0, 0);
            Handle.rectTransform.eulerAngles = new Vector3(0, 0, 0);

            MainParent.anchoredPosition = JOYSTICK_DEFAULT_POSITION;

            AXIS = OnRealaseAxis == false ? Vector2.zero : ONREALESE_AXIS;

            if (!TrackLastDirection) { DIRECTION = Direction.NONE; }

            IS_USING = false;
            IS_OUT_OF_DEADZONE = false;

            if (joystickType == JoystickType.Custom) onRealase.Invoke();
        }

        #endregion

        // OnStart is called before the first frame update

        private void OnStart()
        {

        }

        public void OnStartTouch()
        {

        }

        public void OnEndTouch()
        {
            // example of usage : if "TrackLastDirection" , the direction of the joystick will be displayed on the console

            switch (DIRECTION)
            {
                case Direction.UP:

                    Debug.Log("up");

                    break;

                case Direction.DOWN:

                    Debug.Log("down");

                    break;

                case Direction.RIGHT:

                    Debug.Log("right");

                    break;

                case Direction.LEFT:

                    Debug.Log("left");

                    break;
            }
        }
    }
}