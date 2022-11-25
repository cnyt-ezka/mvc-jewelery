using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Runtime.Data.ValueObject.Source
{
    [HideReferenceObjectPicker]
    [Serializable]
    public class ProductionVO : BaseVO
    {
        [ToggleGroup("NeedSource")]
        public bool NeedSource = true;
        [ToggleGroup("NeedSource")][ListDrawerSettings(ShowIndexLabels = true)]
        public List<StackVO> SourceStack = new ();
        [ToggleGroup("NeedSource")][ListDrawerSettings(ShowIndexLabels = true)]
        public List<ConsumeVO> RequirementsPerProduct = new ();
        public StackVO ProductStack = new ();
    }
}