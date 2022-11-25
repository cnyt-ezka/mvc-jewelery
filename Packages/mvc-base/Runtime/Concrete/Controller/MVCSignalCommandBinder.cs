using System;
using System.Collections.Generic;
using MVC.Base.Runtime.Abstract.Controller;
using strange.extensions.command.api;
using strange.extensions.injector.api;
using strange.extensions.injector.impl;
using strange.extensions.signal.api;
using strange.extensions.signal.impl;
using strange.framework.api;

namespace MVC.Base.Runtime.Concrete.Controller
{
    public class MVCSignalCommandBinder : MVCCommandBinder
    {
        public override void ResolveBinding(IBinding binding, object key)
        {
            base.ResolveBinding(binding, key);
            
            if (bindings.ContainsKey(key)) //If this key already exists, don't bind this again
            {
                IBaseSignal signal = (IBaseSignal)key;
                signal.AddListener(ReactTo); //Do normal bits, then assign the commandlistener to be reactTo
            }
        }

        public override void OnRemove()
        {
            foreach (object key in bindings.Keys)
            {
                IBaseSignal signal = (IBaseSignal)key;
                if (signal != null)
                {
                    signal.RemoveListener(ReactTo);
                }
            }
        }

        protected override IMVCCommandBody InvokeCommand(Type cmd, ICommandBinding binding, object data, int depth, params object[] sequenceData)
        {
            IBaseSignal signal = (IBaseSignal)binding.key;
            IMVCCommandBody command = CreateCommandForSignal(cmd, data, signal.GetTypes()); //Special signal-only command creation
            command.sequenceId = depth;
            TrackCommand(command, binding);
            ExecuteCommand(command, sequenceData);
            return command;
        }
        
        /// Create a Command and bind its injectable parameters to the Signal types
		protected IMVCCommandBody CreateCommandForSignal(Type cmd, object data, List<Type> signalTypes)
		{
			if (data != null)
			{
				object[] signalData = (object[])data;

				//Iterate each signal type, in order. 
				//Iterate values and find a match
				//If we cannot find a match, throw an error
				HashSet<Type> injectedTypes = new HashSet<Type>();
				List<object> values = new List<object>(signalData);

				foreach (Type type in signalTypes)
				{
					if (!injectedTypes.Contains(type)) // Do not allow more than one injection of the same Type
					{
						bool foundValue = false;
						foreach (object value in values)
						{
							if (value != null)
							{
								if (type.IsAssignableFrom(value.GetType())) //IsAssignableFrom lets us test interfaces as well
								{
									injectionBinder.Bind(type).ToValue(value).ToInject(false);
									injectedTypes.Add(type);
									values.Remove(value);
									foundValue = true;
									break;
								}
							}
							else //Do not allow null injections
							{
								throw new SignalException("SignalCommandBinder attempted to bind a null value from a signal to Command: " + cmd.GetType() + " to type: " + type, SignalExceptionType.COMMAND_NULL_INJECTION);
							}
						}
						if (!foundValue)
						{
							throw new SignalException("Could not find an unused injectable value to inject in to Command: " + cmd.GetType() + " for Type: " + type, SignalExceptionType.COMMAND_VALUE_NOT_FOUND);
						}
					}
					else
					{
						throw new SignalException("SignalCommandBinder: You have attempted to map more than one value of type: " + type +
							" in Command: " + cmd.GetType() + ". Only the first value of a type will be injected. You may want to place your values in a VO, instead.",
							SignalExceptionType.COMMAND_VALUE_CONFLICT);
					}
				}
			}
			IMVCCommandBody command = GetCommand(cmd);
			command.data = data;

			foreach (Type typeToRemove in signalTypes) //clean up these bindings
				injectionBinder.Unbind(typeToRemove);
			return command;
		}

        public override ICommandBinding Bind<T>()
        {
	        IInjectionBinding binding = injectionBinder.GetBinding<T>();
	        if (binding == null) //If this isn't injected yet, inject a new one as a singleton
	        {
		        injectionBinder.Bind<T>().ToSingleton();
	        }

	        T signal = injectionBinder.GetInstance<T>();
	        return base.Bind(signal);
        }

        public override ICommandBinding Bind(object value)
        {
	        IInjectionBinding binding = injectionBinder.GetBinding(value);
	        IBaseSignal signal = null;

	        if (value is Type)
	        {
		        if (binding == null) //If this isn't injected yet, inject a new one as a singleton
		        {
			        binding = injectionBinder.Bind (value) as IInjectionBinding;
			        binding.ToSingleton ();
		        }
		        signal = injectionBinder.GetInstance (value as Type) as IBaseSignal;
	        }
	        return base.Bind(signal ?? value);
        }

        /// <summary>Unbind by Signal Type</summary>
        /// <exception cref="InjectionException">If there is no binding for this type.</exception>
        public override void Unbind<T>()
        {
	        ICommandBinding binding = GetBinding<T>();
	        if (binding != null)
	        {
		        T signal = (T) injectionBinder.GetInstance<T>(); 
		        Unbind(signal, null);
	        }
        }

        /// <summary>Unbind by Signal Instance</summary>
        /// <param name="key">Instance of IBaseSignal</param>
        override public void Unbind(object key, object name)
        {
	        if (bindings.ContainsKey(key))
	        {
		        IBaseSignal signal = (IBaseSignal)key;
		        signal.RemoveListener(ReactTo); 
	        }
	        base.Unbind(key, name);
        }

        public override ICommandBinding GetBinding<T>()
        {
	        //This should be a signal, see Bind<T> above
	        T signal = (T)injectionBinder.GetInstance<T>();
	        return base.GetBinding(signal) as ICommandBinding;
        }
    }
}