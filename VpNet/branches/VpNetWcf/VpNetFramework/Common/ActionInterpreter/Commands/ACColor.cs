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
using VPNetExamples.Common.ActionInterpreter.Interfaces;
using VpNet.Core.Structs;
using VpNetFramework.Common.ActionInterpreter.Attributes;
using VpNetFramework.Common.ActionInterpreter.Interfaces;

namespace VpNetFramework.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// The color command assigns a new color to every polygon on an object.
    /// </summary>
    [Serializable]
    public sealed class ACColor : IActionCommand, IActionCommandName
    {
        private string _name;
        private Color _color;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommandColor"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        public ACColor(string name, Color color)
        {
            _name = name;
            _color = color;
        }

        public ACColor(){}

        /// <summary>
        /// The color argument specifies the color to apply. The color can either be specified as one of many preset word values or as a "raw" hexadecimal value giving the red/green/blue component values (the same format as used for the "BGCOLOR=" tag in HTML).
        /// </summary>
        /// <value>The color.</value>
        [ACItemBinding("color", CommandInterpretType.NameValuePairsSpace)]
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// The optional name argument specifies the name of the object to color. Object names are assigned via the name command.
        /// </summary>
        /// <value>The name.</value>
        [ACItemBinding("name", CommandInterpretType.NameValuePairs)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "color"; }
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
