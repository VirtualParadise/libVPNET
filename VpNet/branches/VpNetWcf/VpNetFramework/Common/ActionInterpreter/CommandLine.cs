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

using System.Collections.Generic;
using System.Text.RegularExpressions;
using VPNetExamples.Common.ActionInterpreter;

namespace VpNetFramework.Common.ActionInterpreter
{
    /// <summary>
    ///  A quick and dirty general command line parser. Because I am lazy :)
    /// </summary>
    public sealed class CommandLine
    {
        public string Command { get; internal set; }
        public List<CommandNameValuePair> Pairs { get; internal set; }
        public List<CommandFlag> Flags { get; internal set; }
        public List<CommandArgument> Arguments { get; internal set; }
        public string Literal { get; internal set; }

        public CommandLine(string commandLine)
        {
            Pairs = new List<CommandNameValuePair>();
            Flags = new List<CommandFlag>();
            Arguments = new List<CommandArgument>();
            Literal = commandLine;
            Disect();
        }

        /// <summary>
        /// Determines whether the specified literal command index was interpreted.
        /// </summary>
        /// <param name="index">The index.</param>
        private bool IsInterpreted(int index)
        {
            foreach(var item in Pairs)
            {
                if (index >= item.Value.Index && index < item.Value.Index + item.Value.Length)
                    return true;
                if (index >= item.Name.Index && index < item.Name.Index + item.Value.Length)
                    return true;
            }
            foreach (var item in Flags)
            {
                if (index >= item.Name.Index && index < item.Name.Index + item.Name.Length)
                    return true;
            }
            foreach (var item in Arguments)
            {
                if (index >= item.Value.Index && index < item.Value.Index + item.Value.Length)
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Disects the literal command line.
        /// </summary>
        private void Disect()
        {
            var mcmd = Regex.Match(Literal, "[!,//,a-z,A-Z,0-9]{1,}");
            var mquote = Regex.Matches(Literal, "\"(?<value>[a-z,A-Z,0-9,\\s]{0,})\"");
            var arg = Regex.Matches(Literal, " (?<value>[a-z,A-Z,0-9,#,_,-]{0,})");
            var valuepairs = Regex.Matches(Literal, "(?<name>[a-z,A-Z,0-9]{1,})[\\s]{0,}=[\\s]{0,}(?<value>[a-z,A-Z,0-9]{1,})");
            var quotedvaluepairs = Regex.Matches(Literal, "(?<name>[a-z,A-Z,0-9]{1,})[\\s]{0,}=[\\s]{0,}\"(?<value>[a-z,A-Z,0-9,\\s]{1,})\"");
            var flags = Regex.Matches(Literal, "/(?<name>[a-z,A-Z,0-9]{1,})");

            Command = mcmd.Value;

            // give preference to value pairs.
            for (int i = 0; i < valuepairs.Count; i++)
            {
                Pairs.Add(new CommandNameValuePair() { Name = new CommandIndexedItem(valuepairs[i].Groups["name"]), Value = new CommandIndexedItem(valuepairs[i].Groups["value"]) });
            }
            // then to quoted value pairs.
            for (int i = 0; i < quotedvaluepairs.Count; i++)
            {
                Pairs.Add(new CommandNameValuePair() { Name = new CommandIndexedItem(quotedvaluepairs[i].Groups["name"]), Value = new CommandIndexedItem(quotedvaluepairs[i].Groups["value"]) });
            }

            // then to flags.
            for (int i =0; i<flags.Count; i++)
            {
                Flags.Add(new CommandFlag(){Name=new CommandIndexedItem(flags[i].Groups["name"])});
            }

            // then to quoted arguments.
            for (int i = 0; i < mquote.Count; i++)
            {
                if (!IsInterpreted(mquote[i].Groups["value"].Index))
                    Arguments.Add(new CommandArgument() { Value = new CommandIndexedItem(mquote[i].Groups["value"]) });
            }

            // and then to unquoted arguments.
            for (int i = 0; i < arg.Count; i++)
            {
                if (!IsInterpreted(arg[i].Groups["value"].Index))
                    Arguments.Add(new CommandArgument() { Value = new CommandIndexedItem(arg[i].Groups["value"]) });

            }
        }
    }
}