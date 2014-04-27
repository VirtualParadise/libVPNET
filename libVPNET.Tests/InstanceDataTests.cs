using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VP.Extensions;

namespace VP.Tests
{
    [TestClass]
    public class InstanceDataTests : InstanceTestBase
    {
        [TestMethod]
        public void Disposal()
        {
            var i = new Instance();
            i.Dispose();

            VPNetAssert.ThrowsDisposed(_ => i.Data.GetUserAttributes(0));
            VPNetAssert.ThrowsDisposed(_ => i.Data.ListWorlds());
        }

        [TestMethod]
        public void GetUserAttributes()
        {
            var cmdrData = NewCmdrData();
            var fired    = 0;

            cmdrData.Data.UserAttributes += (i, d) =>
            {
                Console.WriteLine("Attributes: {0}, {1}, {2}", d.Name, d.ID, d.RegistrationTime);
                fired++;
            };

            cmdrData.Data.GetUserAttributes(1);
            cmdrData.Data.GetUserAttributes(int.MaxValue);
            cmdrData.Data.GetUserAttributes(2);
            cmdrData.Data.GetUserAttributes(int.MaxValue - 1);
            cmdrData.Data.GetUserAttributes(3);

            TestPump.AllUntil( () => fired >= 3, cmdrData );

            Assert.IsTrue(fired == 3, "State change event not fired exactly three times");
        }

        [TestMethod]
        public void ListWorlds()
        {
            var cmdrData = NewCmdrData(false);
            var fired    = 0;

            cmdrData.Data.WorldEntry += (i, w) =>
            {
                Console.WriteLine("World: {0}, {1}, {2} users", w.Name, w.State, w.UserCount);
                fired++;
            };

            cmdrData.EnterTestWorld();
            TestPump.AllUntilTimeout(cmdrData);

            Assert.IsTrue(fired >= 1, "Initial world entry event not fired at least once");
            fired = 0;

            cmdrData.Data.ListWorlds();
            TestPump.AllUntilTimeout(cmdrData);

            Assert.IsTrue(fired > 1, "World entry event not fired at least once upon manual request");

            if (fired < 3)
                Assert.Inconclusive("World entry event did not fire more than three times. This may be because there are not enough worlds online");
        }

        [TestMethod]
        public void WorldSettings()
        {
            var cmdrData = NewCmdrData(false);
            var fired    = 0;
            var done     = false;

            cmdrData.Data.WorldSetting += (i, k, v) =>
            {
                Console.WriteLine("World setting: {0} = {1}", k, v);
                fired++;
            };

            cmdrData.Data.WorldSettingsDone += (i) => done = true;

            cmdrData.EnterTestWorld();
            TestPump.AllUntilTimeout(cmdrData);

            Assert.IsTrue(done, "World settings retrival done event never fired");

            if (fired <= 0)
                Assert.Inconclusive("World setting event never fired. This may be because the configured test world has none");
        }
    }
}