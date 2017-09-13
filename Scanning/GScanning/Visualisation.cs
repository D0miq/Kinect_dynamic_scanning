namespace GScanning
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;

    /// <summary>
    /// An instance of the <see cref="Visualisation"/> class reppresents a previewer which handles frames from kinect on its own and visualises them on <see cref="Bitmap"/>.
    /// </summary>
    public class Visualisation
    {
        /// <summary>
        /// Bitmap used for visualisation.
        /// </summary>
        public static readonly WriteableBitmap Bitmap = new WriteableBitmap(WIDTH, HEIGHT, 96.0, 96.0, PixelFormats.Gray8, null);

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Map depth range to byte range.
        /// </summary>
        private const int MAP_DEPTH_TO_BYTE = 8000 / 256;

        /// <summary>
        /// Height of a frame.
        /// </summary>
        private const int HEIGHT = 424;

        /// <summary>
        /// Width of a frame.
        /// </summary>
        private const int WIDTH = 512;

        /// <summary>
        /// Initializes a new instance of the <see cref="Visualisation"/> class.
        /// </summary>
        public Visualisation() { }

        /// <summary>
        /// Handles incoming frames from <see cref="MultiSourceFrameReader"/> and visualises them.
        /// </summary>
        /// <param name="sender">The sender of an event.</param>
        /// <param name="e">Arguments of an event.</param>
        /// <seealso cref="Kinect"/>
        public void Visualisation_FrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            try
            {
                MultiSourceFrame multiSourceFrame = e.FrameReference.AcquireFrame();
                if (multiSourceFrame != null)
                {
                    Frame frame = new Frame(multiSourceFrame);
                    if (frame.AcquireDepthData())
                    {
                        Log.Info("Frames was read properly.");
                        this.Visualise(frame);
                    }
                }
            }
            catch (Exception exception)
            {
                MainForm.SetStatusText("Frame was not read properly.");
                Log.Error(exception.Message + " Frame was not read properly.");
            }
        }

        /// <summary>
        /// Visualises given <paramref name="frame"/>.
        /// </summary>
        /// <param name="frame">The read frame which is visualised.</param>
        private void Visualise(Frame frame)
        {
            byte[] depthColors = this.GetColorFromDepth(frame.DepthData);
            Bitmap.WritePixels(
                new Int32Rect(0, 0, Bitmap.PixelWidth, Bitmap.PixelHeight),
                depthColors,
                Bitmap.PixelWidth,
                0);
        }

        /// <summary>
        /// Transforms depths to colors.
        /// </summary>
        /// <param name="depthData">The array of depth data.</param>
        /// <returns>An array of color data.</returns>
        private byte[] GetColorFromDepth(ushort[] depthData)
        {
            byte[] depthColors = new byte[depthData.Length];
            Parallel.For(0, depthData.Length, index =>
            {
                ushort depth = depthData[index];
                depthColors[index] = (byte)(depth / MAP_DEPTH_TO_BYTE);
            });
            return depthColors;
        }
    }
}
