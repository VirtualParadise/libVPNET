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

namespace VpNetFramework.Common.ActionInterpreter
{
    public class ActionException : Exception
    {
        private readonly string _message;

        public ActionException(string message)
        {
            _message = message;
        }

        public string Message1
        {
            get { return _message; }
        }
    }
}
