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
using VpNetFramework.Common.ActionInterpreter.Interfaces;

namespace VpNetFramework.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// The examine command marks the object as an object that can be "examined", which means users can hold down the left mouse button on the object and move the mouse to rotate it in three dimensions in order to examine all sides of it without having to move themselves. The object will rotate around it's own object axis. When an object is examinable, the mouse cursor changes to a four-direction arrow when placed over the object in order to indicate that it can be examined with the mouse.
    /// </summary>
    [Serializable]
    [Obsolete("Currently not implemented in this VPNet implementation.")]
    public sealed class ACExamine : IActionCommand
    {
        /// <summary>
        /// Single command, no parameters.
        /// </summary>
        public ACExamine()
        {
            
        }

        public string LiteralAction
        {
            get { return "examine"; }
        }

        public string LiteralPart { get; set; }

        #region ICommandGroups Members

        public System.Collections.Generic.IList<ICommandGroup> CommandGroups
        {
            get;
            set;
        }

        #endregion

    }
}
