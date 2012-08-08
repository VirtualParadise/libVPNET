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
    /// The fx argument specifies one of several optional lighting "effects" that can be applied to the light source. All of the effects cause the brightness of the object to vary over time.
    /// </summary>
    [ACEnumType]
    [Serializable]
    public enum LightFxType
    {
        [ACEnumBinding(new[] { "" })]
        None,
        /// <summary>
        /// blink - light alternates equally between on and off
        /// </summary>
        [ACEnumBinding(new[] { "blink" })]
        Blink,
        /// <summary>
        /// fadein - light fades in from dark to full brightness
        /// </summary>
        [ACEnumBinding(new[] { "flash" })]
        FadeIn,
        /// <summary>
        /// fadeout - light fades out from full brightness to dark (after which it deletes itself from the object)
        /// </summary>
        [ACEnumBinding(new[] { "fadeout" })]
        FadeOut,
        /// <summary>
        /// light flickers randomly like a flame
        /// </summary>
        [ACEnumBinding(new[] { "fire" })]
        Fire,
        /// <summary>
        /// light switches off for a brief period at random intervals
        /// </summary>
        [ACEnumBinding(new[] { "flicker" })]
        Flicker,
        /// <summary>
        /// light switches on for a brief period at random intervals
        /// </summary>
        [ACEnumBinding(new[] { "flash" })]
        Flash,
        /// <summary>
        /// light fades in and then back out at regular intervals
        /// </summary>
        [ACEnumBinding(new[] { "pulse" })]
        Pulse
    }
}