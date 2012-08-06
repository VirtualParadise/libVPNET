namespace VpNetWcfHostConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new InstanceServer();
            server.ConnectWcf();
 
        }
    }
}
