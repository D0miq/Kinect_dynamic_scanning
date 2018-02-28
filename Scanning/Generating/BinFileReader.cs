namespace Generating
{
    using System;
    using System.IO;

    /// <summary>
    /// An instance of the <see cref="BinFileReader"/> represents a reader of frame data.
    /// </summary>
    /// <seealso cref="IFileReader"/>
    public class BinFileReader : IFileReader
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Reads data from a binary file and returns them as a frame.
        /// </summary>
        /// <param name="filePath">Path of the file.</param>
        /// <returns>The frame that holds frame data.</returns>
        public Frame ReadFile(string filePath)
        {
            Log.Debug("Filepath: " + filePath);
            try
            {
                BinaryReader br = new BinaryReader(new FileStream(filePath, FileMode.Open));
                int depthLength = br.ReadInt32();
                Log.Debug("Depth data length: " + depthLength);

                ushort[] depthData = new ushort[depthLength];
                for (int i = 0; i < depthLength; i++)
                {
                    depthData[i] = br.ReadUInt16();
                }

                int colorLength = br.ReadInt32();
                Log.Debug("Color data length: " + colorLength);

                byte[] colorData = br.ReadBytes(colorLength);

                br.Close();

                return new Frame(depthData, colorData);
            }
            catch
            {
                Log.Error(filePath + " was not read properly!");
                Console.WriteLine(filePath + " was not read properly!");
                return null;
            }
        }
    }
}
