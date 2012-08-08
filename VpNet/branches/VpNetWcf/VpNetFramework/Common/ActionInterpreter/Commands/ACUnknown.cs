using System;
using VPNetExamples.Common.ActionInterpreter.Interfaces;
using VpNetFramework.Common.ActionInterpreter.Interfaces;

namespace VpNetFramework.Common.ActionInterpreter.Commands
{
    [Serializable]
    public sealed class ACUnknown : IActionCommand
    {
        public ACUnknown(){}

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "N/A"; } 
        }

        #endregion

        #region ILiteralAction Members

        public string LiteralPart
        {
            get; set;
        }

        #endregion


        #region ICommandGroups Members

        public System.Collections.Generic.IList<ICommandGroup> CommandGroups
        {
            get; set;
        }

        #endregion
    }
}
