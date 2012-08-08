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
using System.Collections.Generic;
using VPNetExamples.Common.ActionInterpreter;
using VPNetExamples.Common.ActionInterpreter.Interfaces;
using VpNetFramework.Common.ActionInterpreter.Attributes;
using VpNetFramework.Common.ActionInterpreter.Interfaces;

namespace VpNetFramework.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// Using the lock command to the owner of the object. Other people that try and use the object command will not be able to. An example of a use of the lock command would be for locking doors that only you and a few of your friends can open.
    /// The lock action command can be used to lock all actions listed after it, to be used by the object owner and/or to a list of citizen numbers. It works in combination with create, activate and bump triggers.
    /// Tourists cannot take advantage of this command.
    /// 
    /// Actions listed before the lock statement are executed for everyone. All actions after the lock statement are only executed for the allowed citizens or privilege used by a citizen.
    /// Only object owners and citizens listed can execute the action command global, even if the global statement is listed before the lock statement.
    /// </summary>
    [Serializable]
    [Obsolete("Currently not implemented in this VPNet implementation.")]
    public sealed class ACLock : IActionCommand
    {
        private List<int> _owners;

        public ACLock(List<int> owners)
        {
            _owners = owners;
        }

        public ACLock(){}

        /// <summary>
        /// The owners argument allows the user to define a list of citizen numbers (separated by colons) that will be able to use the following command.
        /// </summary>
        /// <value>The owners.</value>
        [ACItemBinding("owners",':', CommandInterpretType.NameValuePairs)]
        public List<int> Owners
        {
            get { return _owners; }
            set { _owners = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "lock"; }
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
