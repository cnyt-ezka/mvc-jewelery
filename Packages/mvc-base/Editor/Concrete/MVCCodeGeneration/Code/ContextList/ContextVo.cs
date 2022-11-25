using System;
using UnityEngine;

namespace MVC.Base.Editor.Concrete.MVCCodeGeneration.Code.ContextList
{
  [Serializable]
  public class ContextVO
  {
    [SerializeField, AppContext] public int Context;

    [HideInInspector] public bool visible;
  }
}