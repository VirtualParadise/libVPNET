using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using VPNetExamples.Common.ActionInterpreter;
using VPNetExamples.Common.ActionInterpreter.Commands;
using VPNetExamples.Common.ActionInterpreter.Commands.Extended;
using VPNetExamples.Common.ActionInterpreter.Interfaces;
using VPNetExamples.Common.ActionInterpreter.Triggers;

namespace VpNet.UnitTests
{
    [TestFixture]
    public class ActionIntepreterTests
    {
        private Interpreter _interpreter;

        [SetUp]
        public void Setup()
        {
            _interpreter = new Interpreter(Assembly.GetAssembly(typeof(Interpreter)));
        }

        [Test]
        public void CreateNameInterpreterTest()
        {
            const string action = "create name prod1, sign bcolor=00CC00 shadow";
            var interpreted = _interpreter.Interpret(action);
            Assert.AreEqual(Find<ATCreate, ACName>(interpreted).Name, "prod1");
        }
        
        [Test]
        public void CreateExtendedMaterialTest()
        {
            //const string action = "create ambient 0.1";
            const string action = "create ambient 0.1, specular 0.2, normalmap http://objectpath.com/textures/nmap.jpg, specularmap http://objectpath.com/textures/smap.jpg";
            var interpreted = _interpreter.Interpret(action);
            Assert.AreEqual(Find<ATCreate, ACNormalMap>(interpreted).Url, "http://objectpath.com/textures/nmap.jpg");
            Assert.AreEqual(Find<ATCreate, ACSpecularMap>(interpreted).Url, "http://objectpath.com/textures/smap.jpg");
            Assert.AreEqual(Find<ATCreate, ACSpecular>(interpreted).Specular, 0.2f);
            Assert.AreEqual(Find<ATCreate, ACAmbient>(interpreted).Ambient, 0.1f);
        }

        /// <summary>
        /// Finds the specified interpreted action of specified trigger type and command type.
        /// </summary>
        /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="interpretedAction">The interpreted action.</param>
        /// <returns></returns>
        /// <Author>8/4/2012 4:14 AM cube3</Author>
        private TCommand Find<TTrigger, TCommand>(IEnumerable<ICommandGroup> interpretedAction)
            where TCommand : class, IActionCommand, new()
            where TTrigger : class, ICommandGroup, new()
        {
            return (from trigger in interpretedAction from command in trigger.Commands where 
                        command is TCommand && trigger is TTrigger select command as TCommand).FirstOrDefault();
        }

        /// <summary>
        /// Finds the specified non interpreted action of specified trigger type and command type.
        /// Please use overloaded method which searches a interpreted action for speed if you are executing multiple queries
        /// for optimized speed.
        /// </summary>
        /// <typeparam name="TTrigger">The type of the trigger.</typeparam>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        /// <Author>8/4/2012 4:15 AM cube3</Author>
        private TCommand Find<TTrigger, TCommand>(string action)
            where TCommand : class, IActionCommand, new()
            where TTrigger : class, ICommandGroup, new()
        {
            var interpreter = new Interpreter(Assembly.GetAssembly(typeof(Interpreter)));
            return (from trigger in interpreter.Interpret(action) from command in trigger.Commands where 
                        command is TCommand && trigger is TTrigger select command as TCommand).FirstOrDefault();
        }

        [Test]
        public void CreateTextureInterpreterTest()
        {
            const string action = "create texture image1";
            var interpreted = _interpreter.Interpret(action);
            var result = Find<ATCreate, ACTexture>(interpreted);
            Assert.AreEqual(result.Texture,"image1");
        }
    }
}
