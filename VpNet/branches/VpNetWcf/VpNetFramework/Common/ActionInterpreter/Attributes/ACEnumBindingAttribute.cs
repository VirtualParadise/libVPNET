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
using VpNetFramework.Common.ActionInterpreter.Interfaces;

namespace VpNetFramework.Common.ActionInterpreter.Attributes
{
    public sealed class ACEnumBindingAttribute : Attribute, IACLiteralNames
    {
        public string[] LiteralNames { get; set; }

        public ACEnumBindingAttribute(string[] literalNames)
        {
            LiteralNames = literalNames;
        }
    }
}
