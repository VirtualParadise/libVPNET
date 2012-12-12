using VPNetExamples.Common.ActionInterpreter.Attributes;

namespace VPNetExamples.Common.ActionInterpreter.Commands
{
    public interface IACName
    {
        /// <summary>
        /// The name argument is the only argument required for this command. Note that no more than 15 characters are allowed.
        /// </summary>
        /// <value>The name.</value>
        [ACItemBinding(CommandInterpretType.SingleArgument)]
        string Name { get; set; }
    }
}