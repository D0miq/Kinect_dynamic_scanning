namespace Generating
{
    using System;
    using System.IO;

    /// <summary>
    /// 
    /// </summary>
    public class BinFileReader : IFileReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="depthData"></param>
        /// <param name="colorData"></param>
        public void ReadFile(string filePath, out ushort[] depthData, out byte[] colorData)
        {
            try
            {
                BinaryReader br = new BinaryReader(new FileStream(filePath, FileMode.Open));
                int depthLength = br.ReadInt32();
                int colorLength = br.ReadInt32();
                depthData = new ushort[depthLength];
                for (int i = 0; i < depthLength; i++)
                {
                    depthData[i] = br.ReadUInt16();
                }

                colorData = br.ReadBytes(colorLength);
                br.Close();
            }
            catch
            {
                Console.WriteLine(filePath + " was not read properly!");
                depthData = null;
                colorData = null;
            }
        }
    }
}
