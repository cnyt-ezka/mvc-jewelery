using strange.extensions.command.api;
using strange.framework.api;

namespace MVC.Base.Runtime.Abstract.Controller
{
    public interface IMVCCommandBinder : IBinder
    {
        void ReactTo (object trigger, params object[] sequenceData);
        void ReactTo (object trigger, object data, params object[] sequenceData);
        
        void ReleaseCommand(IMVCCommandBody command, params object[] sequenceData);
        
        void Stop(object key);
        
        new ICommandBinding Bind<T>();
        new ICommandBinding Bind(object value);
        new ICommandBinding GetBinding<T>();


        void Jump<TCommand>(IMVCCommandBody command, params object[] sequenceData) where TCommand : IMVCCommandBody;
        void JumpBack<TCommand>(IMVCCommandBody command, params object[] sequenceData) where TCommand : IMVCCommandBody;


        void RunCommand<TCommand>() where TCommand : IMVCCommand;
        void RunCommand<TCommand, TParam>(TParam param) where TCommand : IMVCCommand<TParam>;
        void RunCommand<TCommand, TParam1, TParam2>(TParam1 param1, TParam2 param2) where TCommand : IMVCCommand<TParam1, TParam2>;
        void RunCommand<TCommand, TParam1, TParam2, TParam3>(TParam1 param1, TParam2 param2, TParam3 param3) where TCommand : IMVCCommand<TParam1, TParam2, TParam3>;
        void RunCommand<TCommand, TParam1, TParam2, TParam3, TParam4>(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4) where TCommand : IMVCCommand<TParam1, TParam2, TParam3, TParam4>;
    }
}