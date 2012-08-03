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
using VPNetExamples.Common.ActionInterpreter.Types;

namespace VPNetExamples.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class ACTag : IActionCommand, IActionCommandName /*, IActionCommandGlobal*/
    {
        private AwBooleanType _flag;
        //private GlobalType _isGlobal;

        /// <summary>
        /// Initializes a new instance of the <see cref="ACTag"/> class.
        /// </summary>
        public ACTag(){}

        /// <summary>
        /// Initializes a new instance of the <see cref="ACTag"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="flag">The flag.</param>
        public ACTag(string name, AwBooleanType flag/*, GlobalType isGlobal*/)
        {
            _flag = flag;
            /* _isGlobal = isGlobal;*/
            Name = name;
        }

        [ACItemBinding("flag",CommandInterpretType.Flag)]
        public AwBooleanType Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        public string LiteralAction
        {
            get { return "tag"; }
        }

        public string LiteralPart
        {
            get; set;
        }

        public System.Collections.Generic.IList<ICommandGroup> CommandGroups
        {
            get; set;
        }

        [ACItemBinding("name", CommandInterpretType.NameValuePairsSpace)]
        public string Name
        {
            get; set;
        }

        //[ACItemBinding("global", CommandInterpretType.SingleArgument)]
        //public GlobalType IsGlobal
        //{
        //    get { return _isGlobal; }
        //    set { _isGlobal = value; }
        //}
    }
}
