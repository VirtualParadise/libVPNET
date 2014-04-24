using System;
using System.Configuration;

namespace VP.Tests
{
    static class TestExtensions
    {
        /// <summary>
        /// Logs in the instance using the configured login credentials and given botname
        /// </summary>
        public static Instance TestLogin(this Instance inst, string name)
        {
            return inst.Login(Settings.Username, Settings.Password, name);
        }

        /// <summary>
        /// Makes this instance enter the configured test world
        /// </summary>
        public static Instance EnterTestWorld(this Instance inst)
        {
            return inst.Enter(Settings.World).Pump(1000);
        }
    }
}
