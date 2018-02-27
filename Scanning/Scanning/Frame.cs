namespace Scanning
{
    using Microsoft.Kinect;

    /// <summary>
    /// An instance of the <see cref="Frame"/> class represents a frame with transformed data.
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// The number of bytes needed for a color pixel.
        /// </summary>
        private const int BYTES_PER_PIXEL = 4;

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The depth data of a frame.
        /// </summary>
        private ushort[] depthData;

        /// <summary>
        /// The color data of a frame.
        /// </summary>
        private byte[] colorData;

        /// <summary>
        /// The instance of the <see cref="MultiSourceFrame"/> class.
        /// </summary>
        private MultiSourceFrame multiSourceFrame;

        /// <summary>
        /// The identifical number of a frame.
        /// </summary>
        private int id = 0;

        /// <summary>
        /// Gets the ColorData property that represents color data <see cref="colorData"/> of a frame.
        /// </summary>
        public byte[] ColorData
        {
            get
            {
                return this.colorData;
            }
        }

        /// <summary>
        /// Gets the DepthData property that represents depth data <see cref="depthData"/> of a frame.
        /// </summary>
        public ushort[] DepthData
        {
            get
            {
                return this.depthData;
            }
        }

        /// <summary>
        /// Gets or sets the ID property that represents identifical number <see cref="id"/> of a frame.
        /// </summary>
        public int ID
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Frame"/> class.
        /// </summary>
        /// <param name="multiSourceFrame">Used as a supply of frame data.</param>
        public Frame(MultiSourceFrame multiSourceFrame)
        {
            this.multiSourceFrame = multiSourceFrame;
        }

        /// <summary>
        /// Acquires all data from the <see cref="multiSourceFrame"/>.
        /// </summary>
        /// <returns>
        /// Returns true if all data was acquired correctly, false otherwise.
        /// </returns>
        public bool AcquireData()
        {
            return this.AcquireDepthData() && this.AcquireColorData();
        }

        /// <summary>
        /// Acquires depth data from the <see cref="multiSourceFrame"/>.
        /// </summary>
        /// <returns>
        /// Returns true if depth data was acquired correctly, false otherwise.
        /// </returns>
        private bool AcquireDepthData()
        {
            DepthFrameReference depthFrameReference = this.multiSourceFrame.DepthFrameReference;
            using (DepthFrame depthFrame = depthFrameReference.AcquireFrame())
            {
                if (depthFrame == null)
                {
                    this.depthData = null;
                    Log.Warn("The frame does not contain depth data.");
                    return false;
                }

                FrameDescription depthFrameDescription = depthFrame.FrameDescription;
                this.depthData = new ushort[depthFrameDescription.LengthInPixels];
                depthFrame.CopyFrameDataToArray(this.depthData);
                return true;
            }
        }

        /// <summary>
        /// Acquires color data from the <see cref="multiSourceFrame"/>.
        /// </summary>
        /// <returns>
        /// Returns true if color data was acquired correctly, false otherwise.
        /// </returns>
        private bool AcquireColorData()
        {
            ColorFrameReference colorFrameReference = this.multiSourceFrame.ColorFrameReference;
            using (ColorFrame colorFrame = colorFrameReference.AcquireFrame())
            {
                if (colorFrame == null)
                {
                    this.colorData = null;
                    Log.Warn("The frame does not contain color data.");
                    return false;
                }

                FrameDescription colorFrameDescription = colorFrame.FrameDescription;
                this.colorData = new byte[colorFrameDescription.LengthInPixels * BYTES_PER_PIXEL];
                colorFrame.CopyConvertedFrameDataToArray(this.colorData, ColorImageFormat.Rgba);
                return true;
            }
        }
    }
}
