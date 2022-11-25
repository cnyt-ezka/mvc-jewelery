using System;
using UnityEngine;

namespace Runtime.Entity
{
    public class ColliderTransmitter : MonoBehaviour
    {
        public Action<Collision> OnCollisionEnterAction { get; set; }
        public Action<Collision> OnCollisionStayAction { get; set; }
        public Action<Collision> OnCollisionExitAction { get; set; }
        
        public Action<Collider> OnTriggerEnterAction { get; set; }
        public Action<Collider> OnTriggerStayAction { get; set; }
        public Action<Collider> OnTriggerExitAction { get; set; }

        private void OnCollisionEnter(Collision other)
        {
            OnCollisionEnterAction?.Invoke(other);
        }

        private void OnCollisionStay(Collision other)
        {
            OnCollisionStayAction?.Invoke(other);
        }

        private void OnCollisionExit(Collision other)
        {
            OnCollisionExitAction?.Invoke(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterAction?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayAction?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitAction?.Invoke(other);
        }
    }
}