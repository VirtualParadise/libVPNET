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
using VPNetExamples.Common.ActionInterpreter.Interfaces;

namespace VPNetExamples.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// The link command allows an object to be "attached" to a Mover with the specified name.
    /// 
    /// In order for the command to work correctly, the Mover must have "Linking Enabled" checked in the Mover options. There must also be a Name specified in the Mover, matching the movername specified in the link command.
    /// Note that the linked object and the mover must be built using the same object owner.
    /// </summary>
    [Serializable]
    [Obsolete("Currently not implemented in this VPNet implementation.")]
    public sealed class ACLink : IActionCommand
    {
        private string _moverName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommandLink"/> class.
        /// </summary>
        /// <param name="moverName">Name of the mover.</param>
        public ACLink(string moverName)
        {
            _moverName = moverName;
        }

        public ACLink(){}

        /// <summary>
        /// Gets or sets the name of the mover to link to object to.
        /// </summary>
        /// <value>The name of the mover.</value>
        [ACItemBinding(CommandInterpretType.SingleArgument)]
        public string MoverName
        {
            get { return _moverName; }
            set { _moverName = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "link"; }
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
