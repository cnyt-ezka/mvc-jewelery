using MVC.Base.Runtime.Abstract.Data.ValueObject;
using UnityEngine;

namespace MVC.Base.Runtime.Abstract.Function
{
    public interface IPanel
    {
        IPanelVO vo { get; set; }
        GameObject gameObject { get; }
    }
}