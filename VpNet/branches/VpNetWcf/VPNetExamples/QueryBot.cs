using System;
using System.Globalization;
using System.Linq;
using VPNetExamples.Common;
using VPNetExamples.Common.ActionInterpreter.Commands;
using VpNet.Core;
using VpNet.Core.EventData;
using VpNet.Core.Structs;

namespace VPNetExamples
{
    internal class QueryBot : BaseExampleBot
    {

        public QueryBot() { }

        public QueryBot(Instance instance) : base(instance) { }


        public override void Initialize()
        {
            Instance.EventQueryCellResult += EventQueryCellResult;
            Console.Write("Enter a cell to query (x,y): ");
            var read = Console.ReadLine().Split(',');
            var ci = new CultureInfo("en-US");
            var x = int.Parse(read[0], ci);
            var y = int.Parse(read[1], ci);
            Instance.QueryCell(x,y);
        }

        void EventQueryCellResult(IInstance sender, VpObject objectData)
        {
            Console.WriteLine("{0} {1} {2} name: {3}", objectData.Id, objectData.Model, objectData.Owner, Name(objectData));
        }

        string Name(VpObject vpObject)
        {
            var interpreted = Interpreter.Interpret(vpObject);
            foreach (ACName command in Interpreter.Interpret(vpObject).SelectMany(trigger => trigger.Commands).OfType<ACName>())
            {
                return (command as ACName).Name;
            }
            return string.Empty;
        }


        public override void Disconnect()
        {
           
        }
    }
}
