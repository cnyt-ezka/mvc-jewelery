using System;

namespace MVC.Base.Runtime.Abstract.Controller
{
    public interface IMVCCommand : IMVCCommandBody
    {
        void Execute();
        void Jump<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody;
        void JumpBack<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody;
    }
    
    public interface IMVCCommand<TExecute> : IMVCCommandBody
    {
        void Execute(TExecute executeParameter);
        void Jump<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody;
        void JumpBack<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody;
    }
    
    public interface IMVCCommand<TExecute1, TExecute2> : IMVCCommandBody
    {
        void Execute(TExecute1 executeParameter, TExecute2 executeParam2);
        void Jump<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody;
        void JumpBack<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody;
    }
    
    public interface IMVCCommand<TExecute1, TExecute2, TExecute3> : IMVCCommandBody
    {
        void Execute(TExecute1 executeParam1, TExecute2 executeParam2, TExecute3 executeParam3);
        void Jump<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody;
        void JumpBack<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody;
    }
    
    public interface IMVCCommand<TExecute1, TExecute2, TExecute3, TExecute4> : IMVCCommandBody
    {
        void Execute(TExecute1 executeParam1, TExecute2 executeParam2, TExecute3 executeParam3, TExecute4 executeParam4);
        void Jump<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody;
        void JumpBack<TMVCCommand>(params object[] sequenceData) where TMVCCommand : IMVCCommandBody;
    }
}