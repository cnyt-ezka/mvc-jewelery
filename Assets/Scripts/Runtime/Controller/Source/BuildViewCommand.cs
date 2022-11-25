using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Concrete.Controller;
using Runtime.Data.ValueObject;
using Runtime.Data.ValueObject.Character;
using Runtime.Data.ValueObject.Source;
using Runtime.Models;
using Runtime.Views.MainCharacter;
using Runtime.Views.Source.AreaBase;
using UnityEngine;

namespace Runtime.Controller.Source
{
    public class BuildViewCommand <T> : MVCVoidFunction<T, Transform> where T : TransformVO
    {
        [Inject] private IPoolModel _pool { get; set; }
        [Inject] private IGameModel _game { get; set; }
        
        public override void Execute(T vo, Transform parent)
        {
            if (vo is ProcessorAreaVO processorArea)
            {
                var poolkey = _game.GetPoolKey(processorArea.ID);
                var view = _pool.Get(poolkey, parent).GetComponent<ProcessorAreaView>();
                view.Setup(processorArea);
            }
            else if (vo is ActivationAreaVO activationArea)
            {
                var poolkey = _game.GetPoolKey(activationArea.ID);
                var view = _pool.Get(poolkey, parent).GetComponent<ActivationAreaView>();
                view.Setup(activationArea);
            }
            else if (vo is MainCharacterVO mainCharacterVO)
            {
                var poolkey = _game.GetPoolKey(mainCharacterVO.ID);
                var view = _pool.Get(poolkey, parent).GetComponent<MainCharacterView>();
                view.Setup(mainCharacterVO);
            }
        }
    }
}