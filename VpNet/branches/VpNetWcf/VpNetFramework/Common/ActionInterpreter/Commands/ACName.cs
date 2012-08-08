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
using VPNetExamples.Common.ActionInterpreter;
using VPNetExamples.Common.ActionInterpreter.Commands;
using VPNetExamples.Common.ActionInterpreter.Interfaces;
using VpNetFramework.Common.ActionInterpreter.Attributes;
using VpNetFramework.Common.ActionInterpreter.Interfaces;

namespace VpNetFramework.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// The name command assigns a name to an object so that it may be referenced by an action on another object.
    /// This allows the implementation of multi-object behaviors, such as the where the clicking of one object causes 
    /// another object to appear or disappear, for example.
    /// 
    /// The name command has no effect by itself and must be used in conjunction with actions on other objects in order to implement behaviors.
    /// </summary>
    [Serializable]
    public sealed class ACName : IActionCommand, IACName
    {
        private string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ACName"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ACName(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ACName"/> class.
        /// </summary>
        public ACName(){}

        /// <summary>
        /// The name argument is the only argument required for this command. Note that no more than 15 characters are allowed.
        /// </summary>
        /// <value>The name.</value>
         [ACItemBinding(CommandInterpretType.SingleArgument)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

         public string LiteralAction
         {
             get { return "name"; }
         }

         /// <summary>
         /// Gets or sets the literal part, which contain the command's properties from the original action string.
         /// </summary>
         /// <value>The literal part.</value>
         public string LiteralPart
         {
             get; set;
         }

         /// <summary>
         /// Gets the command groups.
         /// </summary>
         /// <value>The command groups.</value>
         public System.Collections.Generic.IList<ICommandGroup> CommandGroups
         {
             get; set;
         }
    }
}
