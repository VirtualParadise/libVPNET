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
namespace VPNetExamples.Common.ActionInterpreter.Interfaces
{
    public interface ILiteralAction
    {   
        /// <summary>
        /// Gets the literal command, which is the command before interpretation from the original action string.
        /// </summary>
        /// <value>The literal command.</value>
        string LiteralAction { get; }
        /// <summary>
        /// Gets or sets the literal part, which contain the command's properties from the original action string.
        /// </summary>
        /// <value>The literal part.</value>
        string LiteralPart { get; set; }
    }
}
