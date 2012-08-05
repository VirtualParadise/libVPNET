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
    /// 
    /// </summary>
    [ACEnumType]
    [Serializable]
    public enum MediaPlayType
    {
        /// <summary>
        /// Default.
        /// </summary>
        [ACEnumBinding(new[] { "" })]
        Play,
        /// <summary>
        /// Stop allows named objects can be used to stop running media.
        /// </summary>
        [ACEnumBinding(new[] { "stop" })]
        Stop,
        /// <summary>
        /// Pause is used to pause running media. Triggering a subsequent pause causes the media to continue playing where it was paused before.
        /// Note: Live-feeds cannot be paused. The streaming will continue, but the video rendering and the sound will be switched off / muted during the pause. Also note that the set or pause argument should be used before other parameters are defined.
        /// </summary>
        [ACEnumBinding(new[] { "pause" })]
        Pause
    }
}