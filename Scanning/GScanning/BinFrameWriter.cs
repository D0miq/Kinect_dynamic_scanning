namespace GScanning
{
    using System;
    using System.IO;

    /// <summary>
    /// An instance of the <see cref="BinFrameWriter"/> class represents a binary writer.
    /// The writer produces binary files into Scans\\ directory which is created in the application directory.
    /// </summary>
    /// <seealso cref="IFrameWriter"/>
    public class BinFrameWriter : IFrameWriter
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="BinFrameWriter"/> class.
        /// </summary>
        public BinFrameWriter() { }

        /// <summary>
        /// Writes the given <paramref name="frame"/> into binary file.
        /// </summary>
        /// <param name="frame">The given frame that is written into binary file.</param>
        public void WriteFrame(Frame frame)
        {
            MainForm.SetStatusText("Writing frame " + frame.ID + " into a file.");
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
                MainForm.SetStatusText("Frame " + frame.ID + " was written into the file.");
                Log.Info("Frame " + frame.ID + " was written into the file.");
            }
            catch (Exception e)
            {
                MainForm.SetStatusText("Frame " + frame.ID + " was not written into the file.");
                Log.Error(e.Message + " Frame " + frame.ID + " was not written into the file.");
            }
        }
    }
}
