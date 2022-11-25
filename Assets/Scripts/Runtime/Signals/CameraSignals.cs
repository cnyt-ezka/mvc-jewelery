using Runtime.Views.Camera;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Runtime.Signals
{
    public class CameraSignals
    {
        public Signal<GameObject> SetTarget = new ();
        public Signal<CameraKey> Change = new();
        public Signal<CameraKey, float, CameraKey> ChangeSequential = new();
    }
}