/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/

using System;
using VPNetExamples.Common.ActionInterpreter.Attributes;

namespace VPNetExamples.Common.ActionInterpreter.Types
{
    /// <summary>
    /// Specifies whether the cursor should be displayed.
    /// </summary>
    [ACEnumType]
    [Serializable]
    public enum AwBooleanType
    {
        /// <summary>
        /// No binding needed, this is tor internal compatibility.
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// On OR true OR yes: show the mouse. 
        /// </summary>
        [ACEnumBinding(new[]{"on","true","yes"})]
        True = 1,
        /// <summary>
        /// off OR false OR no: hide the mouse cursor.
        /// </summary>
        [ACEnumBinding(new[] { "off", "false", "no" })]
        False = -1
    }
}