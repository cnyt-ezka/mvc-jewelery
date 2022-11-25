using System;

namespace MVC.Base.Runtime.Abstract.Controller
{
    public interface IMVCCommandBody
    {
        Type[] GetGenericArguments();
        /// Flag to indicate that a pooled Command has been restored to its pristine state.
        /// The CommandBinder will use this to determine if re-Injection is required.
        bool IsClean { get; set; }

        /// The property set by `Retain` and `Release` to indicate whether the Command should be cleaned up on completion of the `Execute()` method. 
        bool retain { get; }

        /// The property set to true by a Cancel() call.
        /// Use cancelled internally to determine if further execution is warranted, especially in
        /// asynchronous calls.
        bool cancelled { get; set; }

        /// A payload injected into the Command. Most commonly, this an IEvent.
        object data { get; set; }

        //The ordered id of this Command, used in sequencing to find the next Command.
        int sequenceId{ get; set; }

        /// Keeps the Command in memory. Use only in conjunction with `Release()`
        void Retain();

        /// Allows a previous Retained Command to be disposed.
        void Release(params object[] sequenceData);

        /// Inidcates that the Command failed
        /// Used in sequential command groups to terminate the sequence
        void Fail();

        /// Inform the Command that further Execution has been terminated
        void Cancel ();
    }
}