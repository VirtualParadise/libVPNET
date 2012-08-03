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
namespace VPNetExamples.Common.ActionInterpreter
{
    /// <summary>
    /// Determines how the command item should be interpreted.
    /// </summary>
    public enum CommandInterpretType
    {
        /// <summary>
        /// Format is in name value pairs, seperated by =
        /// </summary>
        NameValuePairs,
        /// <summary>
        /// Format is in name value pairs, seperated by space
        /// </summary>
        NameValuePairsSpace,
        /// <summary>
        /// Property values are seperated by spaces.
        /// </summary>
        Space,
        /// <summary>
        /// Property is a literal name/flag (enumeration), which binds to a boolean property.
        /// </summary>
        Flag,
        /// <summary>
        /// Property is a literal /flag which binds to a boolean property.
        /// </summary>
        FlagSlash,
        /// <summary>
        /// Command has a single argument. for example: crate group groupname.
        /// groupname will be bound to the property decorated as single argument.
        /// </summary>
        SingleArgument,
    }
}