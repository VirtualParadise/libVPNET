using System;

namespace VP.Examples
{
    class ObjectQuery : BaseExampleBot
    {
        public override string Name
        {
            get { return "Object query"; }
        }
        
        Instance bot;
        int      count = 0;

        public override void Main(string user, string password, string world)
        {
            bot = new Instance()
                .Login(user, password, "libVPNET")
                .Enter(world);

            bot.Property.QueryCellResult += onResult;
            bot.Property.QueryCellEnd    += onDone;

            Console.WriteLine("What cell X would you like to query? > ");
            var x = int.Parse( Console.ReadLine() );

            Console.WriteLine("What cell Z would you like to query? > ");
            var z = int.Parse( Console.ReadLine() );

            bot.Property.QueryCell(x, z);
            while (!Disposing)
                bot.Pump();
        }

        void onResult(Instance sender, VPObject objectData)
        {
            Console.WriteLine("Found an object ID#{0} model {1}", objectData.Id, objectData.Model);
            count++;
        }

        void onDone(Instance sender, int x, int z)
        {
            Console.WriteLine("Done scanning cell {0}x{1}; {2} objects found", x, z, count);
            Disposing = true;
        }

        public override void Dispose()
        {
            if (bot != null)
                bot.Dispose();
        }
    }
}
