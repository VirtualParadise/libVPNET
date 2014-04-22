using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VP.Extensions;

namespace VP.Tests
{
    [TestClass]
    public class UtilityTests
    {
        [TestMethod]
        public void UnicodeSplit()
        {
            var chunks = Unicode.ChunkByByteLimit(Strings.TooLong);
            Assert.IsTrue(chunks.Length == 3);

            foreach (var chunk in chunks)
                Assert.IsTrue( Encoding.UTF8.GetByteCount(chunk) <= 255 );
        }

        [TestMethod]
        public void StringExt()
        {
            var expectedA = "[BotName]";
            var expectedB = "[Commander Data]";
            var expectedC = "[Commander Riker]";
            var actualA   = "BotName".AsBotName();
            var actualB   = "[Commander Data]".AsBotName();
            var actualC   = "[[Commander Riker".AsBotName();

            Assert.AreEqual(expectedA, actualA);
            Assert.AreEqual(expectedB, actualB);
            Assert.AreEqual(expectedC, actualC);
        }

        /// <remarks>
        /// https://docs.google.com/spreadsheets/d/1zK9xh0se9edJRqWWdNZn7USKsZwdkrrnKrEJe1pf0RA/edit?usp=sharing
        /// </remarks>
        [TestMethod]
        public void TerrainCalculation_Reference()
        {
            var calcA = new TerrainCalculation(-9.2F, -32.5F);

            Assert.IsTrue(calcA.ScaleFactor == 1f);

            Assert.IsTrue(calcA.Tile.X == -1);
            Assert.IsTrue(calcA.TileCell.X == 22);
            Assert.IsTrue(calcA.Node.X == 2);
            Assert.IsTrue(calcA.NodeCell.X == 6);

            Assert.IsTrue(calcA.Tile.Z == -2);
            Assert.IsTrue(calcA.TileCell.Z == 31);
            Assert.IsTrue(calcA.Node.Z == 3);
            Assert.IsTrue(calcA.NodeCell.Z == 7);

            var calcB = new TerrainCalculation(1002.1122F, -0.2F);

            Assert.IsTrue(calcB.ScaleFactor == 1f);

            Assert.IsTrue(calcB.Tile.X == 31);
            Assert.IsTrue(calcB.TileCell.X == 10);
            Assert.IsTrue(calcB.Node.X == 1);
            Assert.IsTrue(calcB.NodeCell.X == 2);
                              
            Assert.IsTrue(calcB.Tile.Z == -1);
            Assert.IsTrue(calcB.TileCell.Z == 31);
            Assert.IsTrue(calcB.Node.Z == 3);
            Assert.IsTrue(calcB.NodeCell.Z == 7);

            var calcC = new TerrainCalculation(2.001F, 65525F, 0.5F);

            Assert.IsTrue(calcC.ScaleFactor == 0.5f);

            Assert.IsTrue(calcC.Tile.X == 0);
            Assert.IsTrue(calcC.TileCell.X == 4);
            Assert.IsTrue(calcC.Node.X == 0);
            Assert.IsTrue(calcC.NodeCell.X == 4);
                              
            Assert.IsTrue(calcC.Tile.Z == 4095);
            Assert.IsTrue(calcC.TileCell.Z == 10);
            Assert.IsTrue(calcC.Node.Z == 1);
            Assert.IsTrue(calcC.NodeCell.Z == 2);
        }

        [TestMethod]
        public void TerrainCalculation_StressPositive()
        {
            var currentOriginCell = 0;
            var currentTileOffset = 0;
            var currentNodeOffset = 0;
            var currentTile = 0;
            var currentNode = 0;

            for (var i = 0.9f; i < 50000; i += 1)
            {
                var calc = new TerrainCalculation(i, i - 0.5f);

                Assert.IsTrue(calc.ScaleFactor == 1);
                Assert.IsTrue(calc.Position.X == i);
                Assert.IsTrue(calc.Position.Z == i - 0.5f);

                Assert.IsTrue(calc.OriginCell.X == currentOriginCell);
                Assert.IsTrue(calc.OriginCell.Z == currentOriginCell);

                Assert.IsTrue(calc.Tile.X == currentTile);
                Assert.IsTrue(calc.Tile.Z == currentTile);

                Assert.IsTrue(calc.TileCell.X == currentTileOffset);
                Assert.IsTrue(calc.TileCell.Z == currentTileOffset);

                Assert.IsTrue(calc.Node.X == currentNode);
                Assert.IsTrue(calc.Node.Z == currentNode);

                Assert.IsTrue(calc.NodeCell.X == currentNodeOffset);
                Assert.IsTrue(calc.NodeCell.Z == currentNodeOffset);

                currentOriginCell++;
                currentTileOffset++;
                currentNodeOffset++;

                if (currentTileOffset >= 32)
                {
                    currentTileOffset = 0;
                    currentNodeOffset = 0;
                    currentNode       = 0;
                    currentTile++;
                }

                if (currentNodeOffset >= 8)
                {
                    currentNodeOffset = 0;
                    currentNode++;
                }
            }
        }

        [TestMethod]
        public void TerrainCalculation_StressNegative()
        {
            var currentOriginCell = -1;
            var currentTileOffset = 31;
            var currentNodeOffset = 7;
            var currentTile = -1;
            var currentNode = 3;

            for (var i = -0.1f; i < -50000; i -= 1)
            {
                var calc = new TerrainCalculation(i, i - 0.8f);

                Assert.IsTrue(calc.ScaleFactor == 1);
                Assert.IsTrue(calc.Position.X == i);

                Assert.IsTrue(calc.OriginCell.X == currentOriginCell);
                Assert.IsTrue(calc.OriginCell.Z == currentOriginCell);

                Assert.IsTrue(calc.Tile.X == currentTile);
                Assert.IsTrue(calc.Tile.Z == currentTile);

                Assert.IsTrue(calc.TileCell.X == currentTileOffset);
                Assert.IsTrue(calc.TileCell.Z == currentTileOffset);

                Assert.IsTrue(calc.Node.X == currentNode);
                Assert.IsTrue(calc.Node.Z == currentNode);

                Assert.IsTrue(calc.NodeCell.X == currentNodeOffset);
                Assert.IsTrue(calc.NodeCell.Z == currentNodeOffset);

                currentOriginCell--;
                currentTileOffset--;
                currentNodeOffset--;

                if (currentTileOffset < 0)
                {
                    currentTileOffset = 31;
                    currentNodeOffset = 7;
                    currentNode       = 3;
                    currentTile--;
                }

                if (currentNodeOffset < 0)
                {
                    currentNodeOffset = 7;
                    currentNode--;
                }
            }
        }
    }
}