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

namespace VPNetExamples.Common.Reflection
{
    public class ReflectionEnumCache
    {
        private readonly List<ReflectionEnumCacheItem> _enumerations;

        public ReflectionEnumCache(List<ReflectionEnumCacheItem> enumerations)
        {
            _enumerations = enumerations;
        }

        public List<ReflectionEnumCacheItem> Enumerations
        {
            get { return _enumerations; }
        }
    }

}
