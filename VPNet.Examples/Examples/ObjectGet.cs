using System;

namespace VP.Examples
{
    class ObjectGet : BaseExampleBot
    {
        public override string Name
        {
            get { return "Object Get"; }
        }
        
        Instance bot;

        public override void Main(string user, string password, string world)
        {
            bot = new Instance()
                .Login(user, password, "libVPNET")
                .Enter(world);

            bot.Property.ObjectClick       += onClick;
            bot.Property.CallbackObjectGet += callbackObjectGet;

            while (!Disposing)
                bot.Pump();
        }

        void onClick(Instance sender, ObjectClick click)
        {
            Console.WriteLine("Got a click on object ID #{0}, requesting full data...", click.Id);
            bot.Property.GetObject(click.Id);
        }

        void callbackObjectGet(Instance sender, ReasonCode result, VPObject obj)
        {
            if (result == ReasonCode.ObjectNotFound)
                Console.WriteLine("Callback result: Object ID#{0} not found ", obj.Id);
            else
                Console.WriteLine("Callback result: Object model {0}, action {1}, pos {2} ", obj.Model, obj.Action, obj.Position);
        }

        public override void Dispose()
        {
            if (bot != null)
                bot.Dispose();
        }
    }
}
