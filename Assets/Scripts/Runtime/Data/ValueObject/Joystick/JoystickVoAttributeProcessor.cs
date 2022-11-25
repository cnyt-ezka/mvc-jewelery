#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Runtime.Data.ValueObject.Joystick
{
    public class JoystickVoAttributeProcessor : OdinAttributeProcessor<JoystickVO>
    {
        // public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        // {
        //     attributes.Add(new FoldoutGroupAttribute(nameof(TransformVO)));
        //     attributes.Add(new PropertyOrderAttribute(100));
        //     
        // }
    
        public override void ProcessChildMemberAttributes(
            InspectorProperty parentProperty,
            MemberInfo member,
            List<Attribute> attributes)
        {
            // attributes.Add(new HideLabelAttribute());
            // attributes.Add(new BoxGroupAttribute("Box", showLabel: false));
            //attributes.Add(new SpaceAttribute(100));
        
            if (member.Name != nameof(JoystickVO.IsDelta) )
            {
                attributes.Add(new HideIfAttribute(nameof(JoystickVO.IsDelta)));
                
            }
        }
    }
}

#endif
