namespace MVC.Base.Runtime.Extensions.Pool
{
    public interface IMVCPoolableCommand
    {
        /// <summary>
        /// Clean up this instance for reuse.
        /// </summary>
        /// Restore methods should clean up the instance sufficiently to remove prior state.
        void Restore ();

        /// <summary>
        /// Keep this instance from being returned to the pool 
        /// </summary>
        void Retain ();

        /// <summary>
        /// Release this instance back to the pool.
        /// </summary>
        /// Release methods should clean up the instance sufficiently to remove prior state.
        void Release(params object[] sequenceData);

        /// <summary>
        /// Is this instance retained?
        /// </summary>
        /// <value><c>true</c> if retained; otherwise, <c>false</c>.</value>
        bool retain { get; }
    }
}