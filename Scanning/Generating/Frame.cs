namespace Generating
{
    /// <summary>
    /// An instance of the <see cref="Frame"/> class represents a container for data of frame.
    /// </summary>
    /// <seealso cref="IFileReader"/>
    public class Frame
    {
        /// <summary>
        /// The depth data of a frame.
        /// </summary>
        private readonly ushort[] depthData;

        /// <summary>
        /// The color data of a frame.
        /// </summary>
        private readonly byte[] colorData;

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
        /// Initializes a new instance of the <see cref="Frame"/> class.
        /// </summary>
        /// <param name="depthData">Depth data of the frame.</param>
        /// <param name="colorData">Color data of the frame.</param>
        public Frame(ushort[] depthData, byte[] colorData)
        {
            this.depthData = depthData;
            this.colorData = colorData;
        }
    }
}
