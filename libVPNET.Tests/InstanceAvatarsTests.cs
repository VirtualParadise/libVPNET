using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VP.Extensions;

namespace VP.Tests
{
    [TestClass]
    public class InstanceAvatarsTests : InstanceTestBase
    {
        [TestMethod]
        public void Disposal()
        {
            var i = new Instance();
            i.Dispose();

            VPNetAssert.ThrowsDisposed(_ => i.Avatars.ClearUrl(0));
            VPNetAssert.ThrowsDisposed(_ => i.Avatars.Click(0));
            VPNetAssert.ThrowsDisposed(_ => i.Avatars.Click(0, Vector3.Zero));
            VPNetAssert.ThrowsDisposed(_ => i.Avatars.SendUrl(0, "", UrlTarget.Browser));
            VPNetAssert.ThrowsDisposed(_ => i.Avatars.Teleport(0, AvatarPosition.GroundZero));
            VPNetAssert.ThrowsDisposed(_ => i.Avatars.Teleport(0, "", AvatarPosition.GroundZero));
            VPNetAssert.ThrowsDisposed(_ => i.Avatars.Teleport(0, Vector3.Zero, 0, 0));
            VPNetAssert.ThrowsDisposed(_ => i.Avatars.Teleport(0, "", Vector3.Zero, 0, 0));
        }

        [TestMethod]
        public void Enter()
        {
            var cmdrData = NewCmdrData();
            var bots     = new Instance[5];
            var sessions = new HashSet<int>();
            var fired    = 0;
            var lastId   = -1;

            cmdrData.Avatars.Enter += (i, a) =>
            {
                if ( a.Name != Names.Punch.AsBotName() )
                    return;

                if (lastId == -1)
                    lastId = a.Id;
                    
                Assert.IsTrue(a.IsBot);
                Assert.AreEqual(lastId, a.Id);
                Assert.AreEqual(Samples.AvPosition, a.Position);
                Assert.AreEqual(0, a.Type);
                Assert.IsTrue( sessions.Add(a.Session) );

                fired++;
            };

            for (var i = 0; i < 5; i++)
                bots[i] = new Instance()
                    .TestLogin(Names.Punch)
                    .Enter(Settings.World, false)
                    .GoTo(Samples.AvPosition)
                    .Pump(1000);

            TestPump.AllUntil( () => fired >= 5, cmdrData );

            foreach (var bot in bots)
                bot.Dispose();

            Assert.IsTrue(fired == 5, "Avatar enter event not fired exactly five times");
        }

        [TestMethod]
        public void Change()
        {
            var cmdrData = NewCmdrData();
            var bots     = new Instance[5];
            var sessions = new HashSet<int>();
            var fired    = 0;
            var lastId   = -1;

            cmdrData.Avatars.Change += (i, a) =>
            {
                if ( a.Name != Names.Punch.AsBotName() )
                    return;

                if (lastId == -1)
                    lastId = a.Id;
                    
                Assert.IsTrue(a.IsBot);
                Assert.AreEqual(lastId, a.Id);
                Assert.AreEqual(Samples.AvPosition, a.Position);
                Assert.AreEqual(0, a.Type);
                Assert.IsTrue( sessions.Add(a.Session) );

                fired++;
            };

            for (var i = 0; i < 5; i++)
            {
                bots[i] = new Instance()
                    .TestLogin(Names.Punch)
                    .EnterTestWorld();

                TestPump.AllOnce(cmdrData);
                bots[i].GoTo(Samples.AvPosition);
                TestPump.AllOnce(cmdrData, bots[i], cmdrData);
            }

            TestPump.AllUntil( () => fired >= 5, cmdrData );

            foreach (var bot in bots)
                bot.Dispose();

            Assert.IsTrue(fired == 5, "Avatar change event not fired exactly five times");
        }

        [TestMethod]
        public void Leave()
        {
            var cmdrData = NewCmdrData();
            var bots     = new Instance[5];
            var sessions = new HashSet<int>();
            var fired    = 0;

            cmdrData.Avatars.Leave += (i, n, s) =>
            {
                if ( n != Names.Punch.AsBotName() )
                    return;

                Assert.IsTrue( sessions.Add(s) );

                fired++;
            };

            for (var i = 0; i < 5; i++)
            {
                bots[i] = new Instance()
                    .TestLogin(Names.Punch)
                    .EnterTestWorld();

                TestPump.AllOnce(cmdrData);
                bots[i].Leave();
                TestPump.AllOnce(cmdrData, bots[i], cmdrData);
            }

            TestPump.AllUntil( () => fired >= 5, cmdrData );

            foreach (var bot in bots)
                bot.Dispose();

            Assert.IsTrue(fired == 5, "Avatar leave event not fired exactly five times");
        }

        [TestMethod]
        public void Clicked()
        {
            var punch = NewPunch();
            var judy  = NewJudy();
            var fired = 0;

            punch.Avatars.Clicked += (i, a) =>
            {
                if ( a.SourceSession != SessionOf(judy) )
                    return;

                Assert.AreEqual( a.TargetSession, SessionOf(punch) );

                switch (fired)
                {
                    case 0:
                        Assert.AreEqual(Vector3.Zero, a.Position);
                        break;
                    case 1:
                        Assert.AreEqual(Samples.Vector3, a.Position);
                        break;
                }

                fired++;
            };

            judy.Avatars.Click( SessionOf(punch) );
            judy.Avatars.Click( SessionOf(punch), Samples.Vector3 );

            TestPump.AllUntil( () => fired >= 2, punch, judy );

            Assert.IsTrue(fired == 2, "Avatar click event not fired exactly twice times");
        }

        [TestMethod]
        public void Teleported()
        {
            var punch = NewPunch();
            var judy  = NewJudy();
            var fired = 0;

            punch.Avatars.Teleported += (i, s, t, w) =>
            {
                if ( s != SessionOf(judy) )
                    return;

                switch (fired)
                {
                    case 0:
                        Assert.AreEqual("", w);
                        Assert.AreEqual(Samples.AvPosition, t);
                        break;
                    case 1:
                        Assert.AreEqual(Settings.World, w);
                        Assert.AreEqual(Samples.AvPosition, t);
                        break;
                    case 2:
                        Assert.AreEqual("", w);
                        Assert.AreEqual(new AvatarPosition(Samples.Vector3, 66, 88), t);
                        break;
                    case 3:
                        Assert.AreEqual(Settings.World, w);
                        Assert.AreEqual(new AvatarPosition(Samples.Vector3, 44, 22), t);
                        break;
                }

                fired++;
            };

            judy.Avatars.Teleport(SessionOf(punch), Samples.AvPosition);
            judy.Avatars.Teleport(SessionOf(punch), Settings.World, Samples.AvPosition);
            judy.Avatars.Teleport(SessionOf(punch), Samples.Vector3, 66, 88);
            judy.Avatars.Teleport(SessionOf(punch), Settings.World, Samples.Vector3, 44, 22);

            TestPump.AllUntil( () => fired >= 4, punch, judy );

            Assert.IsTrue(fired == 4, "Avatar teleport request event not fired exactly four times");
        }

        [TestMethod]
        public void UrlRequest()
        {
            var punch = NewPunch();
            var judy  = NewJudy();
            var fired = 0;

            punch.Avatars.UrlRequest += (i, s, u, t) =>
            {
                if ( s != SessionOf(judy) )
                    return;

                switch (fired)
                {
                    case 0:
                        Assert.AreEqual(Strings.SampleURL, u);
                        Assert.AreEqual(UrlTarget.Browser, t);
                        break;
                    case 1:
                        Assert.AreEqual(Strings.SampleURLUnicode, u);
                        Assert.AreEqual(UrlTarget.Overlay, t);
                        break;
                    case 2:
                        Assert.AreEqual("", u);
                        Assert.AreEqual(UrlTarget.Overlay, t);
                        break;
                }

                fired++;
            };

            judy.Avatars.SendUrl(SessionOf(punch), Strings.SampleURL, UrlTarget.Browser);
            judy.Avatars.SendUrl(SessionOf(punch), Strings.SampleURLUnicode, UrlTarget.Overlay);
            judy.Avatars.ClearUrl(SessionOf(punch));

            TestPump.AllUntil( () => fired >= 3, punch, judy );

            Assert.IsTrue(fired == 3, "Avatar URL request event not fired exactly three times");
        }
    }
}