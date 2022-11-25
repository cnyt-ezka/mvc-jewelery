using System;
using Runtime.Data.ValueObject.Character;
using UnityEngine;

namespace Runtime.Entity.Character
{
    [SerializeField]
    public class CharacterHealth : MonoBehaviour
    {
        public CharacterHealthVO VO { get; private set; }

        private Action<int, int> _onHealthChangedListener;

        public void Setup(CharacterHealthVO vo, Action<int, int> onHealthChangedListener)
        {
            VO = vo;

            _onHealthChangedListener = onHealthChangedListener;
        }
        
        public void AddHealth(int amount)
        {
            VO.CurrentHealth += amount;
            
            _onHealthChangedListener?.Invoke(VO.CurrentHealth, VO.MaxHealth);
        }

        public void DecreaseHealth(int amount)
        {
            VO.CurrentHealth -= amount;
            
            _onHealthChangedListener?.Invoke(VO.CurrentHealth, VO.MaxHealth);
        }

        public void SetHealth(int amount)
        {
            VO.CurrentHealth = amount;
            
            _onHealthChangedListener?.Invoke(VO.CurrentHealth, VO.MaxHealth);
        }
    }
}