﻿using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;
using strange.framework.api;

namespace MVC.Base.Runtime.Extensions.Pool
{
    public class MVCPool : IPool, IMVCPoolableCommand
    {
        [Inject]
        public IInstanceProvider instanceProvider { get; set; }

        /// Stack of instances still in the Pool.
        protected Stack instancesAvailable = new Stack ();

        /// A HashSet of the objects checked out of the Pool.
        protected HashSet<object> instancesInUse = new HashSet<object> ();

        protected int _instanceCount;

        public MVCPool () : base()
        {
            size = 0;
            constraint = BindingConstraintType.POOL;
            uniqueValues = true;
			
            overflowBehavior = PoolOverflowBehavior.EXCEPTION;
            inflationType = PoolInflationType.DOUBLE;
        }
        
        #region IManagedList implementation

        public virtual IManagedList Add (object value)
        {
            failIf(value.GetType () != poolType, "Pool Type mismatch. Pools must consist of a common concrete type.\n\t\tPool type: " + poolType.ToString() + "\n\t\tMismatch type: " + value.GetType ().ToString(), PoolExceptionType.TYPE_MISMATCH);
            _instanceCount++;
            instancesAvailable.Push (value);
            return this;
        }

        public virtual IManagedList Add (object[] list)
        {
            foreach (object item in list)
                Add (item);

            return this;
        }

        public virtual IManagedList Remove (object value)
        {
            _instanceCount--;
            removeInstance (value);
            return this;
        }

        public virtual IManagedList Remove (object[] list)
        {
            foreach (object item in list)
                Remove (item);

            return this;
        }

        public virtual object value 
        {
            get 
            {
                return GetInstance ();
            }
        }
        #endregion
        
		#region ISemiBinding region
		public virtual bool uniqueValues{get;set;}
		public virtual Enum constraint { get; set; }

		#endregion

		#region IPool implementation

		/// The object Type of the first object added to the pool.
		/// Pool objects must be of the same concrete type. This property enforces that requirement. 
		public Type poolType { get; set; }

		public int instanceCount
		{
			get
			{
				return _instanceCount;
			}
		}

		virtual public object GetInstance ()
		{
			// Is an instance available?
			if (instancesAvailable.Count > 0)
			{
				object retv = instancesAvailable.Pop ();
				instancesInUse.Add (retv);
				return retv;
			}

			int instancesToCreate = 0;

			//New fixed-size pool. Populate.
			if (size > 0)
			{
				if (instanceCount == 0)
				{
					//New pool. Add instances.
					instancesToCreate = size;
				}
				else
				{
					//Illegal overflow. Report and return null
					failIf (overflowBehavior == PoolOverflowBehavior.EXCEPTION,
						"A pool has overflowed its limit.\n\t\tPool type: " + poolType,
						PoolExceptionType.OVERFLOW);

					if (overflowBehavior == PoolOverflowBehavior.WARNING)
					{
						Console.WriteLine ("WARNING: A pool has overflowed its limit.\n\t\tPool type: " + poolType, PoolExceptionType.OVERFLOW);
					}
					return null;
				}
			}
			else
			{
				//Zero-sized pools will expand.
				if (instanceCount == 0 || inflationType == PoolInflationType.INCREMENT)
				{
					instancesToCreate = 1;
				}
				else
				{
					instancesToCreate = instanceCount;
				}
			}

			if (instancesToCreate > 0)
			{
				failIf (instanceProvider == null, "A Pool of type: " + poolType + " has no instance provider.", PoolExceptionType.NO_INSTANCE_PROVIDER);

				for (int a = 0; a < instancesToCreate; a++)
				{
					object newInstance = instanceProvider.GetInstance (poolType);
					Add (newInstance);
				}
				return GetInstance ();
			}

			//If not, return null
			return null;
		}

		virtual public void ReturnInstance (object value)
		{
			if (instancesInUse.Contains (value))
			{
				if (value is IPoolable)
				{
					(value as IPoolable).Restore ();
				}
				instancesInUse.Remove (value);
				instancesAvailable.Push (value);
			}
		}

		virtual public void Clean()
		{
			instancesAvailable.Clear ();
			instancesInUse = new HashSet<object> ();
			_instanceCount = 0;
		}

		virtual public int available
		{
			get
			{
				return instancesAvailable.Count;
			}
		}

		virtual public int size { get; set; }

		virtual public PoolOverflowBehavior overflowBehavior { get; set; }

		virtual public PoolInflationType inflationType { get; set; }

		#endregion

		#region IPoolable implementation

		public void Restore ()
		{
			Clean ();
			size = 0;
		}

		public void Retain()
		{
			retain = true;
		}

		public void Release(params object[] sequenceData)
		{
			retain = false;
		}


		public bool retain{ get; set; }

		#endregion
		
		/// <summary>
		/// Permanently removes an instance from the Pool
		/// </summary>
		/// In the event that the removed Instance is in use, it is removed from instancesInUse.
		/// Otherwise, it is presumed inactive, and the next available object is popped from
		/// instancesAvailable.
		/// <param name="value">An instance to remove permanently from the Pool.</param>
		virtual protected void removeInstance(object value)
		{
			failIf (value.GetType() != poolType, "Attempt to remove a instance from a pool that is of the wrong Type:\n\t\tPool type: " + poolType.ToString() + "\n\t\tInstance type: " + value.GetType().ToString(), PoolExceptionType.TYPE_MISMATCH);
			if (instancesInUse.Contains(value))
			{
				instancesInUse.Remove (value);
			}
			else
			{
				instancesAvailable.Pop ();
			}
		}

		protected void failIf(bool condition, string message, PoolExceptionType type)
		{
			if (condition)
			{
				throw new PoolException(message, type);
			}
		}
    }
}