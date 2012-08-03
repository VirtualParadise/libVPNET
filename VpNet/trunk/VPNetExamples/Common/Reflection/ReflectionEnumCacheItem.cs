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
using System.Collections.Generic;

namespace VPNetExamples.Common.Reflection
{
    public sealed class ReflectionEnumCacheItem
    {
        private readonly Type _enumerationType;
        private readonly List<ReflectionEnumCacheItemField> _itemFields;

        public ReflectionEnumCacheItem(Type enumerationType, List<ReflectionEnumCacheItemField> itemFields)
        {
            _enumerationType = enumerationType;
            _itemFields = itemFields;
        }

        public ReflectionEnumCacheItemField GetFieldByLiteralName(string literalName)
        {
            return ItemFields.Find(p => p.LiteralNames.Contains(literalName));
        }

        public List<ReflectionEnumCacheItemField> ItemFields
        {
            get { return _itemFields; }
        }

        public Type EnumerationType
        {
            get { return _enumerationType; }
        }
    }
}
