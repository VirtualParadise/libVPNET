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

namespace VPNetExamples.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// The camera command can be used to place the camera within the scene. Note that this command will move the camera, but there is nothing to prevent the user from simply switching back to their desired view. Also, this command does not offer the ability to adjust the field of view. If you need to lock the user into a given view, or if you need to adjust the field of view (FOV), then you should use a camera object. Also it should be noted the user's controls do not change if the camera view shifts. This can make it very hard for users to navigate if they find the view suddenly looking down on themselves from one side. For this reason, this is a special command and a user must have the rights to use special commands in order to construct an object that uses the camera command.
    /// </summary>
    [Serializable]
    public sealed class ACCamera : IActionCommand
    {
        private string _location;
        private string _target;
        private bool _isGlobal;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommandCamera"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="target">The target.</param>
        /// <param name="isGlobal">if set to <c>true</c> [is global].</param>
        public ACCamera(string location, string target, bool isGlobal)
        {
            _location = location;
            _target = target;
            _isGlobal = isGlobal;
        }

        public ACCamera(){}

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
        /// Target defines where the camera will look. If you specify the word "user" then the camera will watch the user. Otherwise, the camera will look at the object specified. If the object is moving, then the camera will track to follow the object. If you omit both location and target, it will reset the current view to the default first-person view. Also, if you assign both target and location to the same place, then the command will be ignored. Putting user as both the location and target will cause some strange effects.
        /// </summary>
        /// <value>The target.</value>
        public string Target
        {
            get { return _target; }
            set { _target = value; }
        }

        /// <summary>
        /// Location defines where the camera will be positioned. If you specify the word user then the camera will be placed at the user's eye position. Otherwise, this is assumed to be a name given to another object using the name command. If you omit this value then the camera will be located at the object's origin. Note that if the camera is attached to an object, it will follow that object even if it is moving via the rotate or move commands. If the Target option is not used, the keywords locked, chase, front and first_person set the user's camera view back to the desired mode. (e.g. activate camera location=locked)
        /// </summary>
        /// <value>The location.</value>
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "camera"; }
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
