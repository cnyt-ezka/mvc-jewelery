using System;
using DG.Tweening;
using Runtime.Data.ValueObject;
using Runtime.Data.ValueObject.Character;
using Runtime.Data.ValueObject.Stacker;
using Runtime.Entity.Source.Processors;
using Runtime.Entity.Source.StackableItems;
using Runtime.Entity.Stacker;
using Runtime.Views.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Views.MainCharacter
{
    public class MainCharacterView : CharacterView
    {
        [ShowInInspector]
        [DisableInEditorMode]
        public MainCharacterVO VO { get; private set; }
        [SerializeField]
        private StackerProcessor _stackerProcessor;
        public StackerProcessor Stacker => _stackerProcessor;

        [SerializeField] private Transform _characterHolder;
        
        public static readonly int IDLE = Animator.StringToHash("Idle");
        public static readonly int PULL = Animator.StringToHash("Pull");

        public Action<StackableItem> ItemAdded { get; set; }
        public Action<StackableItem> ItemRemoved { get; set; }
        public Action<StackableItem> MoneyCollected { get; set; }
        public Action<Transform, IProcessor> TryToConsumeForProcessor { get; set; }
        
        private Action _externalPlayableAnimationCallback;
        private float _externalPlayableAnimationDuration;

        public void Setup(MainCharacterVO vo)
        {
            VO = vo;
            
            Setup(vo as CharacterVO);
            SetupStackerProcessor(VO.Stacker);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            
            ItemAdded = null;
            ItemRemoved = null;
            
            if (_stackerProcessor != null)
                _stackerProcessor.TryToConsumeForProcessor = null;
        }
        public void SetupStackerProcessor(StackerVO stacker)
        {
            ItemAdded = OnItemAdded;
            ItemRemoved = OnItemRemoved;

            if (_stackerProcessor == null)
                return;
            
            _stackerProcessor.Setup(stacker, ItemAdded, ItemRemoved, MoneyCollected);

            _stackerProcessor.TryToConsumeForProcessor = TryToConsumeForProcessorListener;
        }

        private void TryToConsumeForProcessorListener(IProcessor targetProcessor)
        {
            TryToConsumeForProcessor?.Invoke(_stackerProcessor.StackHolder, targetProcessor);
        }
        private void OnItemAdded(StackableItem obj)
        {
            if (VO.States == MainCharacterStates.Carrying)
                return;
            
            ChangeState(MainCharacterStates.Carrying);
        }

        private void OnItemRemoved(StackableItem obj)
        {
            if (VO.Stacker.CurrentStack.TempStackCount == 0)
                ChangeState(MainCharacterStates.Idle);
        }

        public void ChangeState(MainCharacterStates state)
        {
            VO.States = state;
            switch (state)
            {
                case MainCharacterStates.Idle:
                    _characterActor.SetAutoRotation(true);
                    _characterActor.SetAnimationTrigger(IDLE);
                    _characterHolder.DOLocalRotate(Vector3.zero, 0.1f);
                    break;
                
                case MainCharacterStates.Carrying:
                    //_characterActor.SetAutoRotation(false);
                    _characterActor.SetAnimationTrigger(PULL);
                    _characterHolder.DOLocalRotate(Vector3.up * 180, 0.2f);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
        public void MoveToDestination(Transform target, float timelineDuration, Action callback)
        {
            _externalPlayableAnimationCallback = callback;
            _externalPlayableAnimationDuration = timelineDuration;
            
            _characterActor.SetDestination(target.position);
        }

        private void ShowCharacterMesh()
        {
            _characterHolder.gameObject.SetActive(true);
        }

        private void HideCharacterMesh()
        {
            _characterHolder.gameObject.SetActive(false);
        }

        protected override void OnReachedToDestination()
        {
            if (_externalPlayableAnimationCallback != null)
            {
                HideCharacterMesh();
                //ChangeState(MainCharacterStates.Idle);

                _externalPlayableAnimationCallback?.Invoke();
                _externalPlayableAnimationCallback = null;

                DOVirtual.DelayedCall(_externalPlayableAnimationDuration, ()=>
                {
                    ShowCharacterMesh();
                    if (VO.Stacker.CurrentStack.TempStackCount == 0)
                        ChangeState(MainCharacterStates.Idle);
                    else
                        ChangeState(MainCharacterStates.Carrying);
                });
            }
        }

        public void UpdateSpeed()
        {
            _characterActor.SetAgentSpeed(_characterActor.GetMaxSpeed());
        }
        public void UpdateCapacity()
        {
            _stackerProcessor.UpdateCapacity();
        }
        
    }

    public enum MainCharacterStates
    {
        Idle,
        Carrying
    }
}
