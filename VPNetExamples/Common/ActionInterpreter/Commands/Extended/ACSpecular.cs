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
    public sealed class ACSpecular : IActionCommand
    {
        private float _specular;

        /// <summary>
        /// Initializes a new instance of the <see cref="ACSpecular"/> class.
        /// </summary>
        /// <param name="specular">The specular.</param>
        /// <Author>8/5/2012 3:01 AM cube3</Author>
        public ACSpecular(float specular)
        {
            _specular = specular;
        }

        public ACSpecular(){}

        /// <summary>
        /// This command sets the specular component of the current surface lighting properties.
        /// </summary>
        /// <value>
        /// The specular
        /// </value>
        [ACItemBinding("", CommandInterpretType.SingleArgument)]
        public float Specular
        {
            get { return _specular; }
            set { _specular = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "specular"; }
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
