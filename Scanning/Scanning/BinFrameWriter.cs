namespace Scanning
{
    using System;
    using System.IO;

    /// <summary>
    /// 
    /// </summary>
    public class BinFrameWriter : IFrameWriter
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        public BinFrameWriter() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        public void WriteFrame(Frame frame)
        {
            Console.WriteLine("Writing frame " + frame.ID + " into a file.");
            Log.Info("Writing frame " + frame.ID + " into a file.");
            try
            {
                string path = "Scans\\Frame" + frame.ID + ".bin";
                Log.Debug("Path: " + path);
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create));

                bw.Write(frame.DepthData.Length);
                bw.Write(frame.ColorData.Length);
                Array.ForEach(frame.DepthData, bw.Write);
                Array.ForEach(frame.ColorData, bw.Write);

                bw.Close();
                Console.WriteLine("Frame " + frame.ID + " was written into the file.");
                Log.Info("Frame " + frame.ID + " was written into the file.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Frame " + frame.ID + " was not written into the file.");
                Log.Error(e.Message + " Frame " + frame.ID + " was not written into the file.");
            }
        }
    }
}
