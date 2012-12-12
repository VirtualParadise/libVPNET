using VPNetExamples.Common;
using VP;

namespace VPNetExamples
{
    internal class HelloWorldBot : BaseExampleBot
    {
        public HelloWorldBot(Instance instance) : base(instance){}

        public HelloWorldBot(){}

        public override void Initialize()
        {
            Instance.Say("Hello World!");
        }

        public override void Disconnect()
        {
            // no cleanup needed.
        }
    }
}
