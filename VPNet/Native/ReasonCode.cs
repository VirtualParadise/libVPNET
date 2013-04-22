namespace VP.Native
{
    /// <summary>
    /// Reason Codes
    /// </summary>
    public enum ReasonCode
    {
        /// <summary>
        /// Operation Successfull
        /// </summary>
        Success,
        /// <summary>
        /// Incorrect API Version
        /// </summary>
        VersionMismatch,
        /// <summary>
        /// Instance not initalized
        /// </summary>
        NotInitialized,
        /// <summary>
        /// Instance already initialized
        /// </summary>
        AlreadyInitialized,
        /// <summary>
        /// String too long
        /// </summary>
        StringTooLong,
        /// <summary>
        /// Invalid password
        /// </summary>
        InvalidPassword,
        /// <summary>
        /// World not found
        /// </summary>
        WorldNotFound,
        /// <summary>
        /// World login error
        /// </summary>
        WorldLoginError,
        /// <summary>
        /// Not in world
        /// </summary>
        NotInWorld,
        /// <summary>
        /// Connection error
        /// </summary>
        ConnectionError,
        /// <summary>
        /// No instance
        /// </summary>
        NoInstance,
        /// <summary>
        /// Not immplemented
        /// </summary>
        NotImplemented,
        /// <summary>
        /// No such attribute available
        /// </summary>
        NoSuchAttribute,
        /// <summary>
        /// Operation not allowed
        /// </summary>
        NotAllowed,
        /// <summary>
        /// Universe database error
        /// </summary>
        DatabaseError,
        /// <summary>
        /// No such user exists
        /// </summary>
        NoSuchUser,
        /// <summary>
        /// Timeout
        /// </summary>
        Timeout,
        /// <summary>
        /// Currently not in universe
        /// </summary>
        NotInUniverse,
        /// <summary>
        /// Invalid arguments provided
        /// </summary>
        InvalidArguments
    }
}
