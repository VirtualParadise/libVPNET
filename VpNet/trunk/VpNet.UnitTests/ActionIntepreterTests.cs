using System.Linq;
using System.Reflection;
using NUnit.Framework;
using VPNetExamples.Common.ActionInterpreter;
using VPNetExamples.Common.ActionInterpreter.Commands;

namespace VpNet.UnitTests
{
    [TestFixture]
    public class ActionIntepreterTests
    {
        [Test]
        public void CreateNameInterpreterTest()
        {
            const string action = "create name prod1, sign bcolor=00CC00 shadow";
            var interpreter = new Interpreter(Assembly.GetAssembly(typeof(Interpreter)));
            var interpreted = interpreter.Interpret(action);
            string name = string.Empty;

            foreach (ACName command in interpreter.Interpret(action).SelectMany(trigger => trigger.Commands).OfType<ACName>())
            {
                name = (command as ACName).Name;
                break;
            }

            Assert.AreEqual(name,"prod1");
        }
    }
}
