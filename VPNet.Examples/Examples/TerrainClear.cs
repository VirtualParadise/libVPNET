using System;
using System.Threading;
using VP;

namespace VPNetExamples.Examples
{
    class TerrainClear : BaseExampleBot
    {
        public override string Name { get { return "Terrain clear"; } }
        Instance bot;

        public override void main()
        {
            Console.WriteLine("Creating new instance, connecting and entering");
            bot = new Instance("Seek and DestROY")
                .Login(VPNetExamples.Username, VPNetExamples.Password)
                .Enter(VPNetExamples.World);


            bot.GoTo();

            // Each node is 8x8 cells
            // Each tile is 4x4 nodes
            /*for (var i = 0; i < 16; i++)
                for (var j = 0; j < 16; j++)
                {
                    // Comment line out below to not pump during node sets
                    bot.Wait(100);
                    bot.Terrain.SetNode(
                        new TerrainNode
                        {
                            X = i,
                            Z = j,
                            Hole = false,
                            Rotation = TerrainRotation.North,
                            Texture = 6,
                            Height = 1.0f
                        }, i, j);

                }*/

            bot.Terrain.GetNode += Terrain_GetNode;

            for (var i = 63; i < 65; i++) 
            {
                for (var j = 63; j < 65; j++)
                {
                    Console.WriteLine("Clearing {0}x{1}", i, j);

                    for (var k = 0; k < 4; k++)
                    for (var l = 0; l < 4; l++)
                    {
                        var node = new TerrainNode
                        {
                            X = k,
                            Z = l,
                            Cells = new TerrainCell[8, 8]
                        };

                        bot.Terrain.SetNode( node, i, j);
                    }

                }

                bot.Wait(1000);
            }

            while (true)
                bot.Wait(0);
        }

        void Terrain_GetNode(Instance sender, TerrainNode node, int tileX, int tileZ)
        {
            Console.WriteLine("Got node for tile {0}x{1}: node {2}x{3}", tileX, tileZ, node.X, node.Z);

            foreach (var cell in node.Cells)
                if (cell.Hole || cell.Height > 0)
                    Console.WriteLine("\tCell: {0}a {1}h", cell.Height, cell.Hole);
        }

        public override void dispose()
        {
            bot.Dispose();
        }
    }
}
