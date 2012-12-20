using System;
using System.IO;
using System.Runtime.InteropServices;

namespace VP.Native
{
    internal static class DataConverters
    {
        /// <summary>
        /// Converts terrain node data to a 2D TerrainCell array
        /// </summary>
        public static TerrainCell[,] TerrainNodeData(byte[] data)
        {
            var cells = new TerrainCell[8,8];

            using (var memStream = new MemoryStream(data))
            {   
                var array = new byte[8];
                for (var i = 0; i < 64; i++)
                {
                    // Source: http://geekswithblogs.net/taylorrich/archive/2006/08/21/88665.aspx
                    if (memStream.Read(array, 0, 8) < 8)
                        throw new Exception("Unexpected end of byte array");

                    GCHandle pin = GCHandle.Alloc(array, GCHandleType.Pinned);
                    TerrainCell cell = (TerrainCell) Marshal.PtrToStructure(pin.AddrOfPinnedObject(), typeof(TerrainCell));
                    pin.Free();

                    int row = i % 8;
                    int col = (i - row) / 8;
                    cells[col, row] = cell;
                }
            }

            return cells;
        }
    }
}
