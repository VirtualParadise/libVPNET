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

namespace VPNetExamples.Common.ActionInterpreter.Commands.Extended
{
    [Serializable]
    public sealed class ACNormalMap : IActionCommand
    {
        private string _url;

        public ACNormalMap(string url)
        {
            _url = url;
        }

        public ACNormalMap() { }

        /// <summary>
        /// This command sets the specular component of the current surface lighting properties.
        /// </summary>
        /// <value>
        /// The specular
        /// </value>
        [ACItemBinding("", CommandInterpretType.SingleArgument)]
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "normalmap"; }
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
