using System;
using VPNetExamples.Common.ActionInterpreter.Attributes;

namespace VPNetExamples.Common.ActionInterpreter.Types
{
    /// <summary>
    /// Global type argument/enum.
    /// </summary>
    [ACEnumType]
    [Serializable]
    public enum GlobalType
    {
        /// <summary>
        /// Unspecified = non global
        /// </summary>
        NonGlobal = 0,
        /// <summary>
        /// Global flag found.
        /// </summary>
        [ACEnumBinding(new[] { "global" })]
        Global = 1,
    }
}
