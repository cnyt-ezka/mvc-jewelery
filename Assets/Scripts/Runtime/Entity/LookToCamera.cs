using UnityEngine;

namespace Runtime.Entity
{
    public class LookToCamera : MonoBehaviour
    {
        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void LateUpdate()
        {
            transform.LookAt(_cam.transform, Vector3.up);            
        }
    }
}