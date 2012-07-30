using VPNetExamples.Common;

namespace VPNetExamples
{
    internal class HelloWorldBot : BaseExampleBot
    {
        public HelloWorldBot()
        {
            Instance.Say("Hello World!");
            Instance.Wait(0);
        }
    }
}
