namespace Scanning
{
    using Microsoft.Kinect;

    /// <summary>
    /// 
    /// </summary>
    public class Frame
    {
        /// <summary>
        /// 
        /// </summary>
        private const int BYTES_PER_PIXEL = 4;

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        private byte[] colorData;

        /// <summary>
        /// 
        /// </summary>
        private ushort[] depthData;

        /// <summary>
        /// 
        /// </summary>
        private MultiSourceFrame multiSourceFrame;

        private int id = 0;

        /// <summary>
        /// 
        /// </summary>
        public byte[] ColorData
        {
            get
            {
                return colorData;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ushort[] DepthData
        {
            get
            {
                return depthData;
            }
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="multiSourceFrame"></param>
        public Frame(MultiSourceFrame multiSourceFrame)
        {
            this.multiSourceFrame = multiSourceFrame;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool AcquireData()
        {
            return this.AcquireDepthData() && this.AcquireColorData();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private bool AcquireDepthData()
        {
            DepthFrameReference depthFrameReference = this.multiSourceFrame.DepthFrameReference;
            using (DepthFrame depthFrame = depthFrameReference.AcquireFrame())
            {
                if (depthFrame == null)
                {
                    this.depthData = null;
                    Log.Warn("Frame does not contain depth data.");
                    return false;
                }

                FrameDescription depthFrameDescription = depthFrame.FrameDescription;
                this.depthData = new ushort[depthFrameDescription.LengthInPixels];
                depthFrame.CopyFrameDataToArray(this.depthData);
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool AcquireColorData()
        {
            ColorFrameReference colorFrameReference = this.multiSourceFrame.ColorFrameReference;
            using (ColorFrame colorFrame = colorFrameReference.AcquireFrame())
            {
                if (colorFrame == null)
                {
                    this.colorData = null;
                    Log.Warn("Frame does not contain color data.");
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
