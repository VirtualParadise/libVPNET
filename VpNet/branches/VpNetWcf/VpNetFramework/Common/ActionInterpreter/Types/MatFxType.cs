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
using VpNetFramework.Common.ActionInterpreter.Attributes;

namespace VpNetFramework.Common.ActionInterpreter.Types
{
    /// <summary>
    /// Matrial Effects.
    /// </summary>
    [ACEnumType]
    [Serializable]
    public enum MatFxType
    {
        /// <summary>
        /// 0 ... no material effect / switch off all material effects
        /// </summary>
        [ACEnumBinding(new[] { "0" })]
        None = 0,
        /// <summary>
        /// 1 ... environment mapping using camera matrix as projection viewpoint
        /// </summary>
        [ACEnumBinding(new[] { "1" })]
        EnvironmentMappingCamera = 1,
        /// <summary>
        /// 2 ... bump mapping using camera matrix as projection viewpoint
        /// </summary>
        [ACEnumBinding(new[] { "2" })]
        BumpMappingCamera = 2,
        /// <summary>
        /// 3 ... environment and bump mapping using camera matrix as projection viewpoint
        /// </summary>
        [ACEnumBinding(new[] { "3" })]
        EnvironmentBumpMappingCamera = 3,
        /// <summary>
        /// 4 ... dual texturing using camera matrix as projection viewpoint
        /// </summary>
        [ACEnumBinding(new[] { "4" })]
        DualTexturingCamera = 4,
        /// <summary>
        /// 10 ... no material effect - same as 0
        /// </summary>
        [ACEnumBinding(new[] { "10" })]
        NoMaterialEffect = 10,
        /// <summary>
        /// 11 ... environment mapping using the directional light matrix as projection viewpoint
        /// </summary>
        [ACEnumBinding(new[] { "11" })]
        EnvironmentMappingLight = 11,
        /// <summary>
        /// 12 ... bump mapping using the directional light matrix as projection viewpoint
        /// </summary>
        [ACEnumBinding(new[] { "12" })]
        BumpMappingLight = 12,
        /// <summary>
        /// 13 ... environment and bump mapping using the directional light matrix as projection viewpoint
        /// </summary>
        [ACEnumBinding(new[] { "13" })]
        EnvironmentBumpMappingLight = 13,
        /// <summary>
        /// 14 ... dual texturing using the directional light matrix as projection viewpoint
        /// </summary>
        [ACEnumBinding(new[] { "14" })]
        DualTexturingLight = 14
    }
}