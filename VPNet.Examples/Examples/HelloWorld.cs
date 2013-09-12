
namespace VP.Examples
{
    class HelloWorld : BaseExampleBot
    {
        public override string Name
        {
            get { return "Hello, world!"; }
        }
        
        Instance bot;

        public override void Main(string user, string password, string world)
        {
            bot = new Instance()
                .Login(user, password, "libVPSDK")
                .Enter(world)
                .Say("Hello, {0}!", "World")
                .Pump()
                .Leave();              
        }

        public override void Dispose()
        {
            if (bot != null)
                bot.Dispose();
        }
    }
}
