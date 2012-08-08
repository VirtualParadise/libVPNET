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
    public sealed class ACAmbient : IActionCommand
    {
        private float _ambient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ACAmbient"/> class.
        /// </summary>
        /// <param name="ambient">The ambient.</param>
        /// <Author>8/5/2012 2:52 AM cube3</Author>
        public ACAmbient(float ambient)
        {
            _ambient = ambient;
        }

        public ACAmbient() { }

        /// <summary>
        /// This command sets the ambient component of the current surface lighting properties and hereby overrides the original ambient settings in the RWX model. The ambient component specifies how much the world's ambient light source "shines" on polygons. It should be specified in the range 0.0 to 1.0. The ambient light source shines equally on all polygons in all directions.
        /// </summary>
        /// <value>The ambient.</value>
        [ACItemBinding("", CommandInterpretType.SingleArgument)]
        public float Ambient
        {
            get { return _ambient; }
            set { _ambient = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "ambient"; }
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