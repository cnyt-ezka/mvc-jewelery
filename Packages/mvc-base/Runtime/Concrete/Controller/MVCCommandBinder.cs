using System;
using System.Collections.Generic;
using System.Linq;
using MVC.Base.Runtime.Abstract.Controller;
using MVC.Base.Runtime.Extensions.Pool;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.injector.api;
using strange.framework.api;
using strange.framework.impl;
using UnityEngine;
using Binder = strange.framework.impl.Binder;

namespace MVC.Base.Runtime.Concrete.Controller
{
    public class MVCCommandBinder : Binder, IMVCCommandBinder
    {
        [Inject] public IInjectionBinder injectionBinder { get; set; }
        
        protected Dictionary<Type, MVCPool> pools = new Dictionary<Type, MVCPool> ();

        protected HashSet<IMVCCommandBody> activeCommands = new HashSet<IMVCCommandBody>();

        protected Dictionary<IMVCCommandBody, ICommandBinding> activeSequences = new Dictionary<IMVCCommandBody, ICommandBinding> ();
        
        public override IBinding GetRawBinding ()
        {
            return new CommandBinding(resolver);
        }

        #region ReactTo

        public virtual void ReactTo (object trigger, params object[] sequenceData)
        {
            ReactTo (trigger, sequenceData, null);
        }
		
        public virtual void ReactTo(object trigger, object data, params object[] sequenceData)
        {
            if (data is IMVCPoolableCommand mvcPoolable)
                mvcPoolable.Retain ();
            
            var binding = (ICommandBinding) GetBinding (trigger);
            if (binding == null)
                return;
            
            if (binding.isSequence)
            {
                Next (binding, data, 0, sequenceData);
            }
            else
            {
                object[] values = binding.value as object[];
                int aa = values.Length + 1;
                for (int a = 0; a < aa; a++)
                {
                    Next (binding, data, a, sequenceData);
                }
            }
        }

        #endregion

        #region Next

        protected void Next(ICommandBinding binding, object data, int depth, params object[] sequenceData)
        {
            var values = binding.value as object[];
            if (depth < values.Length)
            {
                var cmd = values [depth] as Type;

                var command = InvokeCommand(cmd, binding, data, depth, sequenceData);
                ReleaseCommand(command, sequenceData);
            }
            else
            {
                if (binding.isOneOff)
                {
                    Unbind (binding);
                }
            }
        }
        
        #endregion

        #region InvokeCommand

        protected virtual IMVCCommandBody InvokeCommand(Type cmd, ICommandBinding binding, object data, int depth, params object[] sequenceData)
        {
            IMVCCommandBody command = CreateCommand (cmd, data);
            command.sequenceId = depth;
            TrackCommand (command, binding);
            ExecuteCommand (command, sequenceData);
            return command;
        }

        #endregion

        #region CreateCommand

        protected virtual IMVCCommandBody CreateCommand(object cmd, object data)
        {
            var command = GetCommand (cmd as Type);

            if (command == null)
            {
                string msg = "A Command ";
                if (data != null)
                {
                    msg += "tied to data " + data.ToString ();
                }
                msg += " could not be instantiated.\nThis might be caused by a null pointer during instantiation or failing to override Execute (generally you shouldn't have constructor code in Commands).";
                throw new CommandException(msg, CommandExceptionType.BAD_CONSTRUCTOR);
            }

            command.data = data;
            return command;
        }

        #endregion

        #region GetCommand

        protected IMVCCommandBody GetCommand(Type type)
        {
            if (pools.ContainsKey(type))
            {
                MVCPool pool = pools [type];
                IMVCCommandBody command = pool.GetInstance () as IMVCCommandBody;
                if (command.IsClean)
                {
                    injectionBinder.injector.Inject (command);
                    command.IsClean = false;
                }
                return command;
            }
            else
            {
                injectionBinder.Bind<IMVCCommandBody> ().To (type);
                var command = injectionBinder.GetInstance<IMVCCommandBody> ();
                injectionBinder.Unbind<IMVCCommandBody> ();
                return command;
            }
        }

        #endregion

        #region TrackCommand

        protected void TrackCommand (IMVCCommandBody command, ICommandBinding binding)
        {
            if (binding.isSequence)
            {
                activeSequences.Add(command, binding);
            }
            else
            {
                activeCommands.Add(command);
            }
        }

        #endregion

        #region ExecuteCommand

        protected void ExecuteCommand(IMVCCommandBody command, params object[] sequenceData)
        {
            MVCCommandUtils.ExecuteCommand(command, sequenceData);
        }

        #endregion

        #region ReleaseCommand

        public virtual void ReleaseCommand (IMVCCommandBody command, params object[] sequenceData)
        {
            if (command.retain)
                return;
            
            var t = command.GetType ();
            if (pools.ContainsKey (t))
            {
                pools [t].ReturnInstance (command);
            }
            if (activeCommands.Contains(command))
            {
                activeCommands.Remove (command);
            }
            else if (activeSequences.ContainsKey(command))
            {
                ICommandBinding binding = activeSequences [command];
                var data = command.data;
                activeSequences.Remove (command);
                
                Next (binding, data, command.sequenceId + 1, sequenceData);
            }
        }
        
        #endregion

        #region JumpCommand

        public virtual void Jump<TCommand>(IMVCCommandBody command, params object[] sequenceData) where TCommand : IMVCCommandBody
        {
            if(command.retain)
                return;
            
            var commandType = command.GetType ();
            if (pools.ContainsKey (commandType))
            {
                pools [commandType].ReturnInstance (command);
            }
            if (activeCommands.Contains(command))
            {
                activeCommands.Remove (command);
            }
            else if (activeSequences.ContainsKey(command))
            {
                ICommandBinding binding = activeSequences [command];
                var commandListInSequence = (binding.value as object[]).ToList();
                var nextCommand = commandListInSequence.FirstOrDefault(x => x as Type == typeof(TCommand));
                if (nextCommand == null)
                {
                    Debug.LogError("There is no command in sequence -" + typeof(TCommand).Name);
                    return;
                }
                
                var nextCommandId = commandListInSequence.IndexOf(nextCommand);
                if (nextCommandId <= command.sequenceId)
                {
                    Debug.LogError("Recursive Jump Detected! " + command.GetType().Name + " => " + nextCommand.GetType().Name);
                    return;
                }
                
                var data = command.data;
                activeSequences.Remove (command);
                
                Next (binding, data, nextCommandId, sequenceData);
            }
        }

        public virtual void JumpBack<TCommand>(IMVCCommandBody command, params object[] sequenceData)
            where TCommand : IMVCCommandBody
        {
            if(command.retain)
                return;
            
            var commandType = command.GetType ();
            if (pools.ContainsKey (commandType))
            {
                pools [commandType].ReturnInstance (command);
            }
            if (activeCommands.Contains(command))
            {
                activeCommands.Remove (command);
            }
            else if (activeSequences.ContainsKey(command))
            {
                ICommandBinding binding = activeSequences [command];
                var commandListInSequence = (binding.value as object[]).ToList();
                var nextCommand = commandListInSequence.FirstOrDefault(x => x as Type == typeof(TCommand));
                if (nextCommand == null)
                {
                    Debug.LogError("There is no command in sequence -" + typeof(TCommand).Name);
                    return;
                }
                
                var nextCommandId = commandListInSequence.IndexOf(nextCommand);
                if (nextCommandId >= command.sequenceId)
                {
                    Debug.LogError("Recursive Jump Detected! " + command.GetType().Name + " => " + nextCommand.GetType().Name);
                    return;
                }
                
                var data = command.data;
                activeSequences.Remove (command);
                
                Next (binding, data, nextCommandId, sequenceData);
            }
        }
        
        #endregion
        
        #region Others

        public virtual void Stop(object key)
        {
            if (key is IMVCCommand && activeSequences.ContainsKey(key as IMVCCommand))
            {
                removeSequence (key as IMVCCommand);
            }
            else
            {
                ICommandBinding binding = GetBinding (key) as ICommandBinding;
                if (binding == null)
                    return;
                
                if (activeSequences.ContainsValue (binding))
                {
                    foreach(KeyValuePair<IMVCCommandBody, ICommandBinding> sequence in activeSequences)
                    {
                        if (sequence.Value == binding)
                        {
                            var command = sequence.Key;
                            removeSequence (command);
                        }
                    }
                }
            }
        }
        
        protected override IBinding performKeyValueBindings(List<object> keyList, List<object> valueList)
        {
            IBinding binding = null;

            // Bind in order
            foreach (object key in keyList)
            {
                //Attempt to resolve key as a class
                Type keyType = Type.GetType (key as string);
                Enum enumerator = null;
                if (keyType == null)
                {
                    //If it's not a class, attempt to resolve as an Enum
                    string keyString = key as string;
                    int separator = keyString.LastIndexOf(".");
                    if (separator > -1)
                    {
                        string enumClassName = keyString.Substring(0, separator);
                        Type enumType = Type.GetType (enumClassName as string);
                        if (enumType != null)
                        {
                            string enumName = keyString.Substring(separator+1);
                            enumerator = Enum.Parse (enumType, enumName) as Enum;
                        }
                    }
                }
                //If all else fails, just bind the original key
                object bindingKey = keyType ?? enumerator ?? key;
                binding = Bind (bindingKey);
            }
            foreach (object value in valueList)
            {
                Type valueType = Type.GetType (value as string);
                if (valueType == null)
                {
                    throw new BinderException("A runtime Command Binding has resolved to null. Did you forget to register its fully-qualified name?\n Command:" + value, BinderExceptionType.RUNTIME_NULL_VALUE);
                }
                binding = binding.To (valueType);
            }

            return binding;
        }
        
        /// Additional options: Once, InParallel, InSequence, Pooled
        protected override IBinding addRuntimeOptions(IBinding b, List<object> options)
        {
            base.addRuntimeOptions (b, options);
            ICommandBinding binding = b as ICommandBinding;
            if (options.IndexOf ("Once") > -1)
            {
                binding.Once ();
            }
            if (options.IndexOf ("InParallel") > -1)
            {
                binding.InParallel ();
            }
            if (options.IndexOf ("InSequence") > -1)
            {
                binding.InSequence ();
            }
            if (options.IndexOf ("Pooled") > -1)
            {
                binding.Pooled ();
            }

            return binding;
        }

        private void removeSequence(IMVCCommandBody command)
        {
            if (activeSequences.ContainsKey (command))
            {
                command.Cancel();
                activeSequences.Remove (command);
            }
        }

        new public virtual ICommandBinding Bind<T>()
        {
            return base.Bind<T>() as ICommandBinding;
        }

        new public virtual ICommandBinding Bind(object value)
        {
            return base.Bind(value) as ICommandBinding;
        }
        
        new public virtual ICommandBinding GetBinding<T>()
        {
            return base.GetBinding<T>() as ICommandBinding;
        }

        #endregion

        #region RunCommand

        public void RunCommand<TCommand>() where TCommand : IMVCCommand
        {
            var command = GetCommand(typeof(TCommand)) as IMVCCommand;
            command.Execute();
        }

        public void RunCommand<TCommand, TParam>(TParam param) where TCommand : IMVCCommand<TParam>
        {
            var command = GetCommand(typeof(TCommand));
            MVCCommandUtils.ExecuteCommand(command, param);
        }
        
        public void RunCommand<TCommand, TParam1, TParam2>(TParam1 param1, TParam2 param2) where TCommand : IMVCCommand<TParam1, TParam2>
        {
            var command = GetCommand(typeof(TCommand));
            MVCCommandUtils.ExecuteCommand(command, param1, param2);
        }
        
        public void RunCommand<TCommand, TParam1, TParam2, TParam3>(TParam1 param1, TParam2 param2, TParam3 param3)
            where TCommand : IMVCCommand<TParam1, TParam2, TParam3>
        {
            var command = GetCommand(typeof(TCommand));
            MVCCommandUtils.ExecuteCommand(command, param1, param2, param3);
        }
        
        public void RunCommand<TCommand, TParam1, TParam2, TParam3, TParam4>(TParam1 param1, TParam2 param2,
            TParam3 param3, TParam4 param4) where TCommand : IMVCCommand<TParam1, TParam2, TParam3, TParam4>
        {
            var command = GetCommand(typeof(TCommand));
            MVCCommandUtils.ExecuteCommand(command, param1, param2, param3, param4);
        }
        

        #endregion
    }
}