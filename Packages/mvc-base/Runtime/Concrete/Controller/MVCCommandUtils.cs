using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MVC.Base.Runtime.Abstract.Controller;
using UnityEngine;

namespace MVC.Base.Runtime.Concrete.Controller
{
    public static class MVCCommandUtils
    {
        public static Type[] GetCommandExecuteType(this Type type)
        {
            var currentType = type;

            while (currentType != null)
            {
                currentType = type.BaseType;
                if(currentType.Name.Contains("MVCCommand"))
                    break;
            }

            return currentType.GetGenericArguments();
        }

        public static bool IsCommandParameterValid(this Type currentParameterType, Type commandParamType)
        {
            if (currentParameterType == commandParamType)
                return true;

            if (commandParamType.IsInterface)
            {
                var result = currentParameterType.GetInterface(commandParamType.FullName) != null;
                return result;
            }
            else
            {
                var result = currentParameterType.IsSubclassOf(commandParamType);
                return result;
            }
        }

        public static object Cast(this object obj, Type type)
        {
            var methodInfo = typeof(MVCCommandUtils).GetMethod("Cast",
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);

            var castedObj = methodInfo.MakeGenericMethod(type).Invoke(null, new []{ obj });
            return castedObj;
        }

        private static TCast Cast<TCast>(object obj)
        {
            return (TCast) obj;
        }

        public static void ExecuteCommand(IMVCCommandBody commandBody, params object[] sequenceDataList)
        {
            if(commandBody == null)
                return;
            
            var commandGenericTypes = commandBody.GetGenericArguments();
            if(commandGenericTypes == null || commandGenericTypes.Length == 0)
                ExecuteCommand(commandBody);
            else
            {
                var methodInfo = commandBody.GetType().GetMethod("Execute");
                methodInfo.Invoke(commandBody, sequenceDataList);
                // var methodInfo = typeof(MVCCommandUtils)
                //     .GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic)
                //     .Where(x => x.Name == "ExecuteCommand")
                //     .FirstOrDefault(x => x.GetGenericArguments().Length == commandGenericTypes.Length);
                //
                // var invokeParameters = new List<object>();
                // invokeParameters.Add(commandBody);
                // for (var ii = 0; ii < commandGenericTypes.Length; ii++)
                // {
                //     var targetType = commandGenericTypes[ii];
                //     var paramType = sequenceDataList[ii].GetType();
                //
                //     if (!IsCommandParameterValid(paramType, targetType))
                //     {
                //         Debug.LogError("Execute Command with wrong type: " + commandBody.GetType().Name + " Param: " + paramType.Name + " => " + targetType.Name);
                //         return;
                //     }
                //     
                //     invokeParameters.Add(sequenceDataList[ii]);
                // }
                //
                // methodInfo.MakeGenericMethod(commandGenericTypes).Invoke(null, invokeParameters.ToArray());
            }
        }

        private static void ExecuteCommand(IMVCCommandBody commandBody)
        {
            var cmd = commandBody as IMVCCommand;
            cmd.Execute();
        }

        private static void ExecuteCommand<T1>(IMVCCommandBody commandBody, T1 t1)
        {
            var cmd = commandBody as IMVCCommand<T1>;
            cmd.Execute(t1);
        }
        
        private static void ExecuteCommand<T1, T2>(IMVCCommandBody commandBody, T1 t1, T2 t2)
        {
            var cmd = commandBody as IMVCCommand<T1, T2>;
            cmd.Execute(t1, t2);
        }
        
        private static void ExecuteCommand<T1, T2, T3>(IMVCCommandBody commandBody, T1 t1, T2 t2, T3 t3)
        {
            var cmd = commandBody as IMVCCommand<T1, T2, T3>;
            cmd.Execute(t1, t2, t3);
        }
        
        private static void ExecuteCommand<T1, T2, T3, T4>(IMVCCommandBody commandBody, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            var cmd = commandBody as IMVCCommand<T1, T2, T3, T4>;
            cmd.Execute(t1, t2, t3, t4);
        }
    }
}