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
    /// The frame command sets the current frame of an animation that has been set up using the animate command.
    /// </summary>
    [Serializable]
    public sealed class ACFrame : IActionCommand
    {
        private string _name;
        private int _number;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommandFrame"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="number">The number.</param>
        public ACFrame(string name, int number)
        {
            _name = name;
            _number = number;
        }

        public ACFrame(){}

        /// <summary>
        /// The number argument specifies the new frame number. It can either be an absolute frame number, or a relative frame number with a + or - in front to move the animation forward or backwards by a given number of frames.
        /// </summary>
        /// <value>The number.</value>
        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }
        /// <summary>
        /// The optional name argument specifies the name of the object with the animation. Object names are assigned via the name command.
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
            get { return "frame"; }
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
