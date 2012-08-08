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
    /// FX specifies a sound effect applied to the media.
    /// </summary>
    [ACEnumType]
    [Serializable]
    public enum MediaSoundFx
    {
        /// <summary>
        /// Regular sound / mono or stereo, depending on the sound source.
        /// </summary>
        [ACEnumBinding(new[] { "" })]
        None,
        /// <summary>
        /// causes the sound to pan according to your avatar's position.
        /// </summary>
        [ACEnumBinding(new[] { "3d" })]
        _3D
    }
}