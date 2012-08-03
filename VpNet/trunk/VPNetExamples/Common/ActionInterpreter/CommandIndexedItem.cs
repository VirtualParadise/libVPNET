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

using System.Text.RegularExpressions;

namespace VPNetExamples.Common.ActionInterpreter
{
    public sealed class CommandIndexedItem
    {
        public int Index { get; internal set; }
        public int Length { get; internal set; }
        public string Value { get; internal set; }

        public CommandIndexedItem(Capture group)
        {
            Index = group.Index;
            Length = group.Length;
            Value = group.Value;
        }
    }
}