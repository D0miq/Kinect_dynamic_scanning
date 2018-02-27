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
        /// The directory of created files.
        /// </summary>
        private const string DIRECTORY = "Scans\\";

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
                BinaryWriter bw = new BinaryWriter(new FileStream(this.PreparePath(frame.ID), FileMode.Create));

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

        /// <summary>
        /// Creates a path of a file, that will contain a frame. Function creates all directories in a path unless they already exist.
        /// </summary>
        /// <param name="frameID">ID of the frame.</param>
        /// <returns>The path of the file.</returns>
        private string PreparePath(int frameID)
        {
            string path = DIRECTORY + "Frame" + frameID + ".bin";
            Log.Debug("Path: " + path);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            return path;
        }
    }
}
