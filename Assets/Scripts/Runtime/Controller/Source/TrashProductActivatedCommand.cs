using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Concrete.Controller;
using Runtime.Data.ValueObject.Source;
using Runtime.Models;
using UnityEngine;

namespace Runtime.Controller.Source
{
    public class TrashProductActivatedCommand : MVCCommand
    {
        [Inject] private IPlayerModel _player { get; set; }
        [Inject] private IPoolModel _pool { get; set; }
        [Inject] private Transform _trashTarget { get; set; }

        public override void Execute()
        {
            Debug.Log("//=> TrashProductActivatedCommand");

            var currentLevel = _player.GetLevel();

            /*
            var basketStackerVO = 
                currentLevel.QuarterBack.Stackers.FirstOrDefault(x =>
                x.CurrentStack.Type.HasFlag(StackableType.Helmet) ||
                x.CurrentStack.Type.HasFlag(StackableType.Shoulders) || 
                x.CurrentStack.Type.HasFlag(StackableType.Shoes));
            
            if (basketStackerVO == null)
            {
                Debug.LogWarning("Equipment basket is not found!");
                return;
            }

            if (basketStackerVO.CurrentStack.IsMaxChecker == 0)
                return;
            
            for (int ii = basketStackerVO.CurrentStack.StackableItems.Count - 1; ii >= 0; ii--)
            {
                var trashedItem = basketStackerVO.CurrentStack.StackableItems[ii];
                
                basketStackerVO.CurrentStack.RemoveItem(trashedItem);
                basketStackerVO.CurrentStack.IsMaxChecker--;
                        
                Debug.LogWarning($"Product {trashedItem.Obj} successfully deleted.");

                trashedItem.Obj.transform.SetParent(_trashTarget);

                var startPosition = trashedItem.Obj.transform.localPosition;
                var endPosition = Vector3.zero;
                var peekPosition = ((startPosition + endPosition) * 0.5f).AddY(2f);

                var path = new[]{startPosition, peekPosition, endPosition};

                trashedItem.Obj.transform.DOLocalPath(path, 0.35f, PathType.CatmullRom)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        OnTrashAnimationCompletedCallback(trashedItem);
                    });
            }
            
            
            */
        }

        private void OnTrashAnimationCompletedCallback(StackableItemVO trashedItem)
        {
            _pool.Return(trashedItem.Obj);
        }
    }
}