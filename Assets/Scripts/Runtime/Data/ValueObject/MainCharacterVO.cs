using System.Collections.Generic;
using Runtime.Data.ValueObject.Character;
using Runtime.Data.ValueObject.Stacker;
using Runtime.Views.MainCharacter;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [System.Serializable]
    public class MainCharacterVO : CharacterVO
    {
        [ShowInInspector]
        [DisableInEditorMode]
        public MainCharacterStates States { get; set; } = MainCharacterStates.Idle;
        
        public StackerVO Stacker = new();
        public int CurrentSpeedGradeIndex = 0;
        public List<UpgradeVO> SpeedUpgrades = new();
        public int CurrentIncomeGradeIndex = 0;
        public List<UpgradeVO> IncomeUpgrades = new();
        #region Helper Mothods
        
        public UpgradeVO GetCurrentSpeedUpgrade()
        {
            return SpeedUpgrades[Mathf.Clamp(CurrentSpeedGradeIndex, 0, SpeedUpgrades.Count - 1)];
        }

        public UpgradeVO GetCurrentIncomeUpgrade()
        {
            return IncomeUpgrades[Mathf.Clamp(CurrentIncomeGradeIndex, 0, IncomeUpgrades.Count - 1)];
        }
        
        public UpgradeVO GetNextSpeedUpgrade()
        {
            if (CurrentSpeedGradeIndex + 1 > SpeedUpgrades.Count - 1)
                return null;

            return SpeedUpgrades[CurrentSpeedGradeIndex + 1];
        }

        public UpgradeVO GetNextIncomeUpgrade()
        {
            if (CurrentIncomeGradeIndex + 1 > IncomeUpgrades.Count - 1)
                return null;

            return IncomeUpgrades[CurrentIncomeGradeIndex + 1];
        }
        #endregion
    }
}