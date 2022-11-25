using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Screen
{
    public class ScreenLayer : MonoBehaviour
    {
        public int childCount => transform.childCount;

        public Transform GetChild(int index)
        {
            return transform.GetChild(index);
        }
    }
}