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
using VPNetExamples.Common.ActionInterpreter.Interfaces;
using VPNetExamples.Common.ActionInterpreter.Types;
using VpNetFramework.Common.ActionInterpreter.Interfaces;

namespace VpNetFramework.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// The astart command starts an animation that has been set up using the animate command and sets it in either looping or non-looping mode. The default is for the astart command to start the animation in non-looping mode. 
    /// </summary>
    [Serializable]
    [Obsolete("Currently not implemented in this VPNet implementation.")]
    public sealed class ACAStart : IActionCommand
    {
        private string _name;
        private bool _isGlobal;
        private AnimationFlagType _flag;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommandAStart"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isGlobal">if set to <c>true</c> [is global].</param>
        public ACAStart(string name, bool isGlobal, AnimationFlagType flag)
        {
            _name = name;
            _isGlobal = isGlobal;
            _flag = flag;
        }

        public ACAStart(){}

        /// <summary>
        /// The flag argument specifies whether or not the animation will run "looping." flag can be either "on", "true", or "yes" to make the animation looping, and either "off", "false", or "no" to make it non-looping. A looping animation repeats endlessly, while a non-looping animation stops when it reaches the last frame and causes an Adone condition to be triggered for the object when the animation completes.
        /// </summary>
        /// <value>The flag.</value>
        public AnimationFlagType Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        /// <summary>
        /// The optional global argument will cause triggers to initiate the command for all users have the object in view. Without it, the command will be triggered exclusively for the user who activates the trigger (bump, activate, adone). By default, commands are not global.
        /// </summary>
        /// <value><c>true</c> if this instance is global; otherwise, <c>false</c>.</value>
        public bool IsGlobal
        {
            get { return _isGlobal; }
            set { _isGlobal = value; }
        }

        /// <summary>
        /// The optional name argument specifies the name of the object containing the animation to be started. Object names are assigned via the name command.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "astart"; }
        }

        public string LiteralPart { get; set; }

        #endregion

        #region ICommandGroups Members

        public System.Collections.Generic.IList<ICommandGroup> CommandGroups
        {
            get;
            set;
        }

        #endregion
    }
}
