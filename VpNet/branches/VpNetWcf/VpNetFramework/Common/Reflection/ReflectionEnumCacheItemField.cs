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

using System.Collections.Generic;

namespace VpNetFramework.Common.Reflection
{
    public sealed class ReflectionEnumCacheItemField
    {
        private readonly string _name;
        private readonly int _value;
        private readonly List<string> _literalNames;

        public ReflectionEnumCacheItemField(string name, int value, List<string> literalNames)
        {
            _name = name;
            _value = value;
            _literalNames = literalNames;
        }

        public List<string> LiteralNames
        {
            get { return _literalNames; }
        }

        public int Value
        {
            get { return _value; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
