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
    /// The type specifies the type of light source, which can be either "point" or "spot". "Point" light sources shine equally in all directions, and are the default if no type is specified. "Spot" light sources shine a "cone" of light in a particular direction.
    /// </summary>
    [ACEnumType]
    [Serializable]
    public enum LightType
    {
        /// <summary>
        /// "Point" light sources shine equally in all directions, and are the default if no type is specified.
        /// </summary>
        [ACEnumBinding(new[]{"point"})]
        Point,
        /// <summary>
        /// "Spot" light sources shine a "cone" of light in a particular direction.
        /// </summary>
        [ACEnumBinding(new[]{"spot"})]
        Spot
    }
}