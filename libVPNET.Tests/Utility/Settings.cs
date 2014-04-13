using System.Configuration;

namespace VP.Tests
{
    static class Settings
    {
        public static string Username = ConfigurationManager.AppSettings["Username"];
        public static string Password = ConfigurationManager.AppSettings["Password"];
        public static string World    = ConfigurationManager.AppSettings["World"];
    }
}
