using MVC.Base.Runtime.Abstract.Model;
using MVC.Base.Runtime.Concrete.Controller;
using Runtime.Models;

namespace Runtime.Controller
{
    public class InitPoolCommand : MVCCommand
    {
        [Inject] private IPoolModel _pool { get; set; }
        [Inject] private IGameModel _game { get; set; }
        public override void Execute()
        {
            foreach (var item in _game.PoolHelper.List)
            {
                _pool.Pool(item.Key.ToString(),item.Prefab,item.Count);
            }
        }
    }
}