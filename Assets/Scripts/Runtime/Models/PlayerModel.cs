using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Data.ValueObject.Source;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Models
{
    public class PlayerModel : IPlayerModel
    {
        private RD_Player _playerData;
        [ShowInInspector] private PlayerVO _vo;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _playerData = Resources.Load<RD_Player>("Data/Player");
            _playerData.VO = ES3.Load("Player", _playerData.VO);
            _vo = _playerData.VO;
        }

        #region Level

        public void SaveLevel(LevelVO vo)
        {
            Debug.Log("SaveLevel");
            _vo.IsNewLevel = false;
            _vo.Level = vo;
            Save();
        }

        public bool IsNewLevel => _vo.IsNewLevel;

        public int GetLevelIndex()
        {
            return _vo.CurrentLevelIndex;
        }

        public int SetNextLevelIndex()
        {
            _vo.CurrentLevelIndex++;
            return _vo.CurrentLevelIndex;
        }

        public LevelVO GetLevel()
        {
            return _vo.Level;
        }

        public void SetNewLevel()
        {
            _vo.IsNewLevel = true;
        }

        public void SetCurrentLevel(int index)
        {
            _vo.CurrentLevelIndex = index;
        }

        public void AddArea<T>(T VO)
        {
            if (VO is ActivationAreaVO activationArea)
            {
                _playerData.VO.Level.ActivationAreas.Add(activationArea);
            }
            else if (VO is ProcessorAreaVO processorArea)
            {
                _playerData.VO.Level.ProcessorAreas.Add(processorArea);
            }
        }

        public bool HasArea<T>(T VO)
        {
            if (VO is ActivationAreaVO activationArea)
            {
                
                foreach (var existActivationAreas in _playerData.VO.Level.ActivationAreas)
                {
                    if (activationArea.ID == existActivationAreas.ID)
                        return true;
                }

                return false;
            }
            if (VO is ProcessorAreaVO processorArea)
            {
                foreach (var existProcessorArea in _playerData.VO.Level.ProcessorAreas)
                {
                    if (processorArea.ID == existProcessorArea.ID)
                        return true;
                }
                
                return false;
            }

            return false;
        }

        #endregion

        #region Currency
        
        public int GetCurrency()
        {
            return _vo.Currency.Soft;
        }

        public void AddCurrency(int amount)
        {
            _vo.Currency.Soft += amount;
        }

        public bool RemoveCurrency(int amount)
        {
            if (_vo.Currency.Soft - amount < 0)
                return false;
            
            _vo.Currency.Soft -= amount;

            return true;
        }

        public void SetCurrency(int amount)
        {
            _vo.Currency.Soft = amount;
        }
        
        #endregion

        private void Save()
        {
            _playerData.Save();
        }
    }
}
