using System.Collections.Generic;
using System.Linq;
using Runtime.Enums.Source;
using Sirenix.OdinInspector;

namespace Runtime.Data.ValueObject.Source
{
    [System.Serializable][HideReferenceObjectPicker]
    public class ProcessorAreaVO : AreaVO
    {
        [ShowIf(nameof(IsRuntimeData))]
        public int CurrentGradeIndex = 0;
        
        [TabGroup("Upgrade"),GUIColor(1f,1f,1f)]
        [ToggleLeft]
        public bool IsUpgrader = true;
        
        [TabGroup("Upgrade"),GUIColor(1f,1f,1f)]
        [ShowIf(nameof(IsUpgrader))]
        [ListDrawerSettings(ShowIndexLabels = true)]
        public List<ConsumeVO> Upgrades = new();
        
        [TabGroup("Production"),GUIColor(1f,1f,1f)]
        [ToggleLeft]
        public bool IsProducer = true;

        [TabGroup("Production"),GUIColor(1f,1f,1f)]
        [ShowIf(nameof(IsProducer))]
        [ListDrawerSettings(ShowIndexLabels = true)]
        public List<ProductionVO> ProductionGrades = new();

        #region Helper Methods

        private ProductionVO _currentProductionGrade => ProductionGrades[CurrentGradeIndex];
        
        public bool NeedSource => _currentProductionGrade.NeedSource;

        public StackVO GetSourceStack(int index)
        {
            return NeedSource ? _currentProductionGrade.SourceStack[index] : null;
        }

        public StackVO GetProductStack()
        {
            return IsProducer ? _currentProductionGrade.ProductStack : null;
        }

        public ConsumeVO GetRequirementPerProduct(StackableType itemType)
        {
            return _currentProductionGrade.RequirementsPerProduct.FirstOrDefault(x => x.Type == itemType);
        }

        public ConsumeVO GetUpgradeGrade()
        {
            return IsUpgrader ? Upgrades[CurrentGradeIndex] : null;
        }
        
        #endregion
    }
}