using Runtime.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Views.Camera
{
    public class CameraMediator : Mediator
    {
        [Inject] private CameraManager _view { get; set; }
        [Inject] private CameraSignals Signals { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            Signals.SetTarget.AddListener(OnSetTarget);
            Signals.Change.AddListener(OnChange);
            Signals.ChangeSequential.AddListener(OnChangeSequential);

        }

        public override void OnRemove()
        {
            base.OnRemove();
            Signals.SetTarget.RemoveListener(OnSetTarget);
            Signals.Change.RemoveListener(OnChange);
            Signals.ChangeSequential.RemoveListener(OnChangeSequential);
        }

        private void OnChange(CameraKey cameraKey)
        {
            _view.ChangeSequential(cameraKey);
        }

        private void OnChangeSequential(CameraKey cameraKey, float nextCamDelay, CameraKey nextCameraKey)
        {
            _view.ChangeSequential(cameraKey, nextCamDelay, nextCameraKey);
        }

        private void OnSetTarget(GameObject target)
        {
            _view.SetTarget(target.transform);
        }
    }
}
