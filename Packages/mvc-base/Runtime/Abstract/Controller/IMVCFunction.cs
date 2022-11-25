namespace MVC.Base.Runtime.Abstract.Controller
{
    public interface IMVCFunction<TReturnType> : IMVCCommandBody
    {
        public TReturnType Execute();
    }
    
    public interface IMVCFunction<TReturnType, TParam> : IMVCCommandBody
    {
        public TReturnType Execute(TParam param);
    }
    
    public interface IMVCFunction<TReturnType, TParam1, TParam2> : IMVCCommandBody
    {
        public TReturnType Execute(TParam1 param1, TParam2 param2);
    }
    
    public interface IMVCFunction<TReturnType, TParam1, TParam2, TParam3> : IMVCCommandBody
    {
        public TReturnType Execute(TParam1 param1, TParam2 param2, TParam3 param3);
    }
    
    public interface IMVCFunction<TReturnType, TParam1, TParam2, TParam3, TParam4> : IMVCCommandBody
    {
        public TReturnType Execute(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }
}