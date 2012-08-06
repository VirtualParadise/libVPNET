using System.Runtime.Serialization;

namespace VpNet.NativeApi
{
#if (WCF)
    /// <summary>
    /// Reason Codes
    /// </summary>
    [DataContract]
    public enum ReasonCode
    {
        /// <summary>
        /// Operation Successfull
        /// </summary>
        [DataMember] Success = 0,
        /// <summary>
        /// Incorrect API Version
        /// </summary>
        [DataMember]
        VersionMismatch = 1,
        /// <summary>
        /// Instance not initalized
        /// </summary>
        [DataMember]
        NotInitialized = 2,
        /// <summary>
        /// Instance already initialized
        /// </summary>
        [DataMember]
        AlreadyInitialized = 3,
        /// <summary>
        /// String too long
        /// </summary>
        [DataMember]
        StringTooLong = 4,
        /// <summary>
        /// Invalid password
        /// </summary>
        [DataMember]
        InvalidPassword = 5,
        /// <summary>
        /// World not found
        /// </summary>
        [DataMember]
        WorldNotFound = 6,
        /// <summary>
        /// World login error
        /// </summary>
        [DataMember]
        WorldLoginError = 7,
        /// <summary>
        /// Not in world
        /// </summary>
        [DataMember]
        NotInWorld = 8,
        /// <summary>
        /// Connection error
        /// </summary>
        [DataMember]
        ConnectionError = 9,
        /// <summary>
        /// No instance
        /// </summary>
        [DataMember]
        NoInstance = 10,
        /// <summary>
        /// Not immplemented
        /// </summary>
        [DataMember]
        NotImplemented = 11,
        /// <summary>
        /// No such attribute available
        /// </summary>
        [DataMember]
        NoSuchAttribute = 12,
        /// <summary>
        /// Operation not allowed
        /// </summary>
        [DataMember]
        NotAllowed = 13,
        /// <summary>
        /// Universe database error
        /// </summary>
        [DataMember]
        DatabaseError = 14,
        /// <summary>
        /// No such user exists
        /// </summary>
        [DataMember]
        NoSuchUser = 15,
        /// <summary>
        /// Timeout
        /// </summary>
        [DataMember]
        Timeout = 16
    }
#endif
#if (!WCF)
    /// <summary>
    /// Reason Codes
    /// </summary>
    public enum ReasonCode
    {
        /// <summary>
        /// Operation Successfull
        /// </summary>
        Success = 0,
        /// <summary>
        /// Incorrect API Version
        /// </summary>
        VersionMismatch = 1,
        /// <summary>
        /// Instance not initalized
        /// </summary>
        NotInitialized = 2,
        /// <summary>
        /// Instance already initialized
        /// </summary>
        AlreadyInitialized = 3,
        /// <summary>
        /// String too long
        /// </summary>
        StringTooLong = 4,
        /// <summary>
        /// Invalid password
        /// </summary>
        InvalidPassword = 5,
        /// <summary>
        /// World not found
        /// </summary>
        WorldNotFound = 6,
        /// <summary>
        /// World login error
        /// </summary>
        WorldLoginError = 7,
        /// <summary>
        /// Not in world
        /// </summary>
        NotInWorld = 8,
        /// <summary>
        /// Connection error
        /// </summary>
        ConnectionError = 9,
        /// <summary>
        /// No instance
        /// </summary>
        NoInstance = 10,
        /// <summary>
        /// Not immplemented
        /// </summary>
        NotImplemented = 11,
        /// <summary>
        /// No such attribute available
        /// </summary>
        NoSuchAttribute = 12,
        /// <summary>
        /// Operation not allowed
        /// </summary>
        NotAllowed = 13,
        /// <summary>
        /// Universe database error
        /// </summary>
        DatabaseError = 14,
        /// <summary>
        /// No such user exists
        /// </summary>
        NoSuchUser = 15,
        /// <summary>
        /// Timeout
        /// </summary>
        Timeout = 16
    }
#endif
}
