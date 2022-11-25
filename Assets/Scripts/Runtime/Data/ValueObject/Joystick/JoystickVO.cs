using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.ValueObject.Joystick
{
    [System.Serializable]
    [HideReferenceObjectPicker]
    public class JoystickVO
    {
        [Title("Properties")]
        [ToggleLeft] public bool IsDelta;
        [ToggleLeft] public bool IsHidden;
        [ToggleLeft] public bool IsInvisible;
        [ToggleLeft] public bool IsFixed;
        [ToggleLeft] public bool ConsiderCameraAngle;
        
        [Title("Position & Bounds")]
        [Range(0,1)] public float AnchorPosX;
        [Range(0,1)] public float AnchorPosY;
        
        [ToggleGroup("Bounds")] public bool Bounds;
        [ToggleGroup("Bounds")][LabelText("Ver")]
        [MinMaxSlider(0,1,true)]
        public Vector2 VerticalBounds;

        [ToggleGroup("Bounds")][LabelText("Hor")]
        [MinMaxSlider(0,1,true)]
        public Vector2 HorizontalBounds;

        [PropertySpace(40)]
        [ToggleGroup("SmoothStop")] public bool SmoothStop;
        [ToggleGroup("SmoothStop")] [Range(0, 1)] public float StopAnimDuration = .2f;
        
        [PropertySpace(40)]
        [Title("Joystick Templates", "current data will be lost")]
        [Button(ButtonSizes.Large)][GUIColor(.9f,.8f,.85f)]
        public void PortraitJoystick()
        {
            AnchorPosX = .5f;
            AnchorPosY = .3f;
            Bounds = false;
            VerticalBounds = Vector2.up;
            HorizontalBounds = Vector2.up;
        }
        //[Button(ButtonSizes.Large), ResponsiveButtonGroup("BTNGroup")]
        [Button(ButtonSizes.Large)][GUIColor(.9f,.8f,.85f)]
        public void LandscapeJoystickLeft()
        {
            AnchorPosX = .2f;
            AnchorPosY = .3f;
            Bounds = true;
            VerticalBounds = new Vector2(0f,.5f);
            HorizontalBounds = Vector2.up;
        }
        [Button(ButtonSizes.Large)][GUIColor(.9f,.8f,.85f)]
        public void LandscapeJoystickRight()
        {
            AnchorPosX = .8f;
            AnchorPosY = .3f;
            Bounds = true;
            VerticalBounds = new Vector2(.5f,1f);
            HorizontalBounds = Vector2.up;
        }

        public bool Test;
    }
    
    public enum JoystickID
    {
        Movement1,
        Movement2,
        Shooting
    }
}