using System;
using System.Collections.Generic;
using VPNetExamples.Common.ActionInterpreter.Interfaces;

namespace VPNetExamples.Common.ActionInterpreter.Triggers
{
    [Serializable]
    public sealed class ATUnknown : ICommandGroup
    {
        #region IActionTrigger Members

        public string LiteralCommands
        {
            get; set;
        }

        #endregion

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "N/A"; }
        }

        public string LiteralPart {get;set;}

        #endregion

        #region IActionTrigger Members

        public List<IActionCommand> Commands
        {
            get; set;
        }

        #endregion
    }
}
