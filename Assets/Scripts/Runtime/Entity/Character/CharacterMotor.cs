using System;
using MVC.Base.Runtime.Extensions;
using Runtime.Data.ValueObject;
using Runtime.Data.ValueObject.Character;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Entity.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMotor : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        public CharacterMovementVO VO { get; private set; }

        public bool HasReachedToDestination { get; private set; }

        private Action _onReachedToDestinationListener;
        
        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void Setup(CharacterMovementVO vo, Action onReachedToDestinationListener)
        {
            VO = vo;

            _onReachedToDestinationListener = onReachedToDestinationListener;

            _navMeshAgent.enabled = true;
            
            SetAgentSpeed(VO.CurrentMovementSpeed);
        }

        private void Update()
        {
            if (HasReachedToDestination == false)
                CheckHasReachedToDestination();
        }

        private void CheckHasReachedToDestination()
        {
            if (IsAgentActive() && HasReachedToDestination == false)
            {
                if (!_navMeshAgent.pathPending)
                {
                    if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                    {
                        if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                        {
                            HasReachedToDestination = true;
                            _onReachedToDestinationListener?.Invoke();
                        }
                    }
                }
            }
        }

        public float GetCurrentSpeed()
        {
            return _navMeshAgent.velocity.magnitude;
        }

        public float GetMaxSpeed()
        {
            return VO.MaxMovementSpeed;
        }

        public bool IsAgentActive()
        {
            return _navMeshAgent.enabled;
        }

        public void EnableAgent()
        {
            _navMeshAgent.enabled = true;
        }

        public void DisableAgent()
        {
            _navMeshAgent.enabled = false;
        }

        public void SetAgentSpeed(float speed)
        {
            VO.CurrentMovementSpeed = speed;
            _navMeshAgent.speed = VO.CurrentMovementSpeed;
        }

        public void SetDestination(Vector3 destination)
        {
            if (IsAgentActive() == false)
                EnableAgent();

            HasReachedToDestination = false;
            _navMeshAgent.isStopped = false;
            
            _navMeshAgent.SetDestination(destination);
        }
        
        public void RotateTo(Quaternion lookRotation)
        {
            _navMeshAgent.transform.localRotation = Quaternion.Lerp(_navMeshAgent.transform.localRotation, lookRotation,.5f);
        }
        
        public void RotateTo(Vector3 direction, bool direct = false)
        {
            if (direction == Vector3.zero)
                return;

            if (direct)
                _navMeshAgent.transform.localRotation = Quaternion.LookRotation(direction);
            else
                _navMeshAgent.transform.localRotation = Quaternion.Lerp(_navMeshAgent.transform.localRotation, Quaternion.LookRotation(direction), .5f);
        }

        public void SetAutoRotation(bool isOn)
        {
            _navMeshAgent.updateRotation = isOn;
        }
        
        public void StopMovement()
        {
            if (IsAgentActive() == false)
                return;
            
            _navMeshAgent.isStopped = true;
            HasReachedToDestination = true;
            
            _onReachedToDestinationListener?.Invoke();
        }
    }
}