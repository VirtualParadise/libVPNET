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
    /// The type argument is an integer value that one of the value shown below, where i.e. type 0 will render a the environment from the model's view at it's bounding box center point considering the reflection angle from the main camera's current position; while type 1 will do the same plus mirroring the rendered image.
    /// </summary>
    [ACEnumType]
    [Serializable]
    public enum EnviType
    {
        /// <summary>
        /// 0 ... use bbox center, dynamic reflection angle
        /// </summary>
        [ACEnumBinding(new[] { "0" })]
        BBox_Dynamic_Reflection_Angle = 0,
        /// <summary>
        /// 1 ... use bbox center, dynamic reflection angle, mirror image
        /// </summary>
        [ACEnumBinding(new[] { "1" })]
        BBox_Dynamic_Reflection_Angle_Mirror_Image = 1,
        /// <summary>
        /// 2 ... use bbox center, fixed reflection angle
        /// </summary>
        [ACEnumBinding(new[] { "2" })]
        BBox_Fixed_Reflection_Angle = 2,
        /// <summary>
        /// 3 ... use bbox center, fixed reflection angle, mirror image
        /// </summary>
        [ACEnumBinding(new[] { "3" })]
        BBox_Fixed_Reflection_Angle_Mirror_Image = 3,

        /// <summary>
        /// 10 ... use model center, dynamic reflection angle
        /// </summary>
        [ACEnumBinding(new[] { "10" })]
        Model_Dynamic_Reflection_Angle = 10,
        /// <summary>
        /// 11 ... use model center, dynamic reflection angle, mirror image
        /// </summary>
        [ACEnumBinding(new[] { "11" })]
        Model_Dynamic_Reflection_Angle_Mirror_Image = 11,
        /// <summary>
        /// 12 ... use model center, fixed reflection angle
        /// </summary>
        [ACEnumBinding(new[] { "12" })]
        Model_Fixed_Reflection_Angle = 12,
        /// <summary>
        /// 13 ... use model center, fixed reflection angle, mirror image
        /// </summary>
        [ACEnumBinding(new[] { "13" })]
        Model_Fixed_Refelction_Angle_Mirror_Image = 13
    }
}