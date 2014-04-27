using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VP.Extensions;

namespace VP.Tests
{
    [TestClass]
    public class InstanceTerrainTests : InstanceTestBase
    {
        [TestMethod]
        public void Disposal()
        {
            var i = new Instance();
            i.Dispose();

            VPNetAssert.ThrowsDisposed(_ => i.Terrain.QueryTile(0, 0));
            VPNetAssert.ThrowsDisposed(_ => i.Terrain.QueryTile(0, 0, null));
            VPNetAssert.ThrowsDisposed(_ => i.Terrain.SetNode(Samples.TerrainNode, 0, 0));
        }

        [TestMethod]
        public void GetNode()
        {
            var cmdrData = NewCmdrData();
            var fired    = 0;

            cmdrData.Terrain.GetNode += (i, n, x, z) =>
            {
                Console.WriteLine("Get node from tile {0}x{1}: rev. {2} @ {3}x{4}", x, z, n.Revision, n.X, n.Z);

                Assert.AreEqual(16, x);
                Assert.AreEqual(32, z);
                Assert.IsTrue(n.Revision >= 0);
                Assert.IsTrue(n.X >= 0 && n.X < 4);
                Assert.IsTrue(n.Z >= 0 && n.Z < 4);
                Assert.IsTrue(n.Cells.Length == 64);

                fired++;
            };

            cmdrData.Terrain.QueryTile(16, 32);
            cmdrData.Terrain.QueryTile(16, 32, new int[4,4]
            {
                { -1, -1, -1, -1 },
                { -1, -1, -1, -1 },
                { -1, -1, -1, -1 },
                { -1, -1, -1, -1 },
            });

            TestPump.AllUntil( () => fired >= 32, cmdrData );

            Assert.IsTrue(fired == 32, "GetNode never fired exactly 32 times");
        }

        [TestMethod]
        public void SetNode()
        {
            var cmdrData = NewCmdrData();
            var fired    = 0;

            List<TerrainNode> backup = new List<TerrainNode>();

            cmdrData.Terrain.GetNode += (i, n, x, z) =>
            {
                Assert.AreEqual(4, x);
                Assert.AreEqual(8, z);

                backup.Add(n);
            };

            cmdrData.Terrain.CallbackNodeSet += (i, rc, n, x, z) =>
            {
                if (rc != ReasonCode.Success)
                    throw new Exception( "Reason code failure: " + rc.ToString() );

                Assert.AreEqual(4, x);
                Assert.AreEqual(8, z);
                Assert.IsTrue(n.Revision >= 0);
                Assert.IsTrue(n.X >= 0 && n.X < 4);
                Assert.IsTrue(n.Z >= 0 && n.Z < 4);
                Assert.IsTrue(n.Cells.Length == 64);

                fired++;
            };

            cmdrData.Terrain.QueryTile(4, 8);

            TestPump.AllUntil( () => backup.Count >= 16, cmdrData );

            foreach (var node in backup)
                cmdrData.Terrain.SetNode(node, 4, 8);

            TestPump.AllUntil( () => fired >= 16, cmdrData );

            Assert.IsTrue(fired == 16, "CallbackNodeSet never fired exactly 16 times");
        }
    }
}