using System.ComponentModel;
using Runtime.Signals;

namespace Runtime.Models
{
    public class SROptionsModel
    {
        [Inject] private GameSignals _gameSignals { get; set; }
        
        [Category("Level")]
        public void NextLevel()
        {
            //_gameSignals.NextLevel.Dispatch();
        }
    }
}