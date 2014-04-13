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
            return inst.Enter(Settings.World);
        }

        /// <summary>
        /// Logs in the instance using the configured login credentials and then enters
        /// the configured test world
        /// </summary>
        public static Instance TestLoginAndEnter(this Instance inst, string name)
        {
            return inst.TestLogin(name).EnterTestWorld();
        }

        /// <summary>
        /// Logs in as "SDKPunch", entering the configured test world
        /// </summary>
        public static Instance AsPunch(this Instance inst)
        {
            return inst.TestLoginAndEnter(Names.Punch);
        }

        /// <summary>
        /// Logs in as "SDKJudy", entering the configured test world
        /// </summary>
        public static Instance AsJudy(this Instance inst)
        {
            return inst.TestLoginAndEnter(Names.Judy);
        }

        /// <summary>
        /// Logs in as "SDKCmdrData", entering the configured test world
        /// </summary>
        public static Instance AsData(this Instance inst)
        {
            return inst.TestLoginAndEnter(Names.Data);
        }
    }
}
