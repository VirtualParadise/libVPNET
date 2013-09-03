using System;
using System.Threading;
using VP;

namespace VPNetExamples.Examples
{
    class UserData : BaseExampleBot
    {
        public override string Name { get { return "UserData"; } }
        Instance bot;

        public override void main()
        {
            Console.WriteLine("Creating new instance, connecting and entering");
            bot = new Instance("VP Bot")
                .Login(VPNetExamples.Username, VPNetExamples.Password)
                .Enter(VPNetExamples.World);

            Console.WriteLine("Fetching attributes...");
            bot.Data.UserAttributes += getAttributes;
            bot.Data.GetUserAttributes(0);
            bot.Data.GetUserAttributes(236526236);
            bot.Data.GetUserAttributes(1);
            bot.Data.GetUserAttributes(2);
            bot.Data.GetUserAttributes(3);
            bot.Data.GetUserAttributes(4);
            bot.Data.GetUserAttributes(182);

            while (true)
                bot.Wait(1000);
        }

        void getAttributes(Instance sender, User user)
        {
            Console.WriteLine("Attributes: ID {0}, name {1}, registered {2}", user.ID, user.Name, user.RegistrationTime);
            Console.WriteLine("Attributes: Online {0}, email {1}, last login {2}", user.OnlineTime, user.Email, user.LastLogin);
        }

        public override void dispose()
        {
            bot.Data.UserAttributes -= getAttributes;
            bot.Dispose();
        }
    }
}
