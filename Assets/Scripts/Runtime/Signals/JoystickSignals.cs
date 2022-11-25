using strange.extensions.signal.impl;
using UnityEngine;

namespace Runtime.Signals
{
    public class JoystickSignals
    {
        public Signal Start = new();
        public Signal<JoystickParam> Move = new();
        public Signal Stop = new();
        public Signal Hide = new();
    }

    public class JoystickParam
    {
        public float Magnitude;
        public Vector2 DirectionVector2;
        public Vector3 DirectionVector3;
        public Quaternion DirectionQuaternion;
    }
}