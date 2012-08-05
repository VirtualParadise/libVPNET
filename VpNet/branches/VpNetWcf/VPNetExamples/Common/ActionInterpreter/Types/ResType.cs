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
    /// The res argument is the resolution used for the rendering. Valid values are 32, 64, 128, 256, 512 and 1024. Higher resolutions will result in better quality of the rendered subscene. Lower resolutions are rendered faster, but in lower quality. 
    /// </summary>
    [ACEnumType]
    [Serializable]
    public enum ResType
    {
        /// <summary>
        /// 32x32 pixels resolution.
        /// </summary>
        [ACEnumBinding(new[] { "32" })]
        Res32 = 32,
        /// <summary>
        /// 64x64 pixels resolution.
        /// </summary>
        [ACEnumBinding(new[] { "64" })]
        Res64 = 64,
        /// <summary>
        /// 128x128 pixels resolution.
        /// </summary>
        [ACEnumBinding(new[] { "128" })]
        Res128 = 128,
        /// <summary>
        /// 256x256 pixels resolution.
        /// </summary>
        [ACEnumBinding(new[] { "256" })]
        Res256 = 256,
        /// <summary>
        /// 512x512 pixels resolution.
        /// </summary>
        [ACEnumBinding(new[] { "512" })]
        Res512 = 512,
        /// <summary>
        /// 1024x1024 pixels resolution.
        /// </summary>
        [ACEnumBinding(new[] { "1024" })]
        Res1024 = 1024
    }
}