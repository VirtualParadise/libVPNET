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
using VpNetFramework.Common.ActionInterpreter.Attributes;
using VpNetFramework.Common.ActionInterpreter.Interfaces;

namespace VpNetFramework.Common.ActionInterpreter.Commands.Extended
{
    [Serializable]
    public sealed class ACSpecularMap : IActionCommand
    {
        private string _url;

        public ACSpecularMap(string url)
        {
            _url = url;
        }

        public ACSpecularMap() { }

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
            get { return "specularmap"; }
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
