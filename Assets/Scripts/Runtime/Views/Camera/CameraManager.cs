using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using MVC.Base.Runtime.Abstract.View;
using UnityEngine;

namespace Runtime.Views.Camera
{
    public class CameraManager : MVCView
    {
        public List<CameraVO> CamList;
        
        private Dictionary<CameraKey, CameraVO> _camMap = new();
        private CameraKey _activeCam = 0;

        protected override void Awake()
        {
            base.Awake();
            foreach (var cam in CamList)
            {
                _camMap.Add(cam.Key, cam);
            }
        }

        public void SetTarget(Transform target)
        {
            foreach (var cam in CamList)
            {
                if(cam.SetFollow) cam.Cam.Follow = target;
                if(cam.SetLookAt) cam.Cam.LookAt = target;
            }
        }

        public void ChangeSequential(CameraKey camKey, float nextCamDelay = 0f, CameraKey nextCamKey = CameraKey.Source)
        {
            _camMap[_activeCam].Cam.Priority = 0;
            _camMap[camKey].Cam.Priority = 100;
            _activeCam = camKey;

            Debug.Log("ChangeSequential " + camKey +" : " +nextCamDelay  +" : "+ nextCamKey );
            if (nextCamDelay != 0)
                DOVirtual.DelayedCall(nextCamDelay, ()=> ChangeSequential(nextCamKey));
        }
    }
    
    [System.Serializable]
    public class CameraVO
    {
        public CameraKey Key;
        public bool SetFollow;
        public bool SetLookAt;
        public CinemachineVirtualCamera Cam;
    }
    
    public enum CameraKey
    {
        SourceStart,
        Source,
    }
}
