namespace GScanning
{
    using System.Threading.Tasks;

    /// <summary>
    /// An instance of the <see cref="Convertor"/> class represents a convertor between depth and color data.
    /// </summary>
    public class Convertor
    {
        /// <summary>
        /// Map depth range to byte range.
        /// </summary>
        private const float MAP_DEPTH_TO_BYTE = byte.MaxValue / Kinect.MaxDistance;

        /// <summary>
        /// Transforms depths to colors.
        /// </summary>
        /// <param name="depthData">The array of depth data.</param>
        /// <returns>An array of color data.</returns>
        public static byte[] ConvertDepthToColor(ushort[] depthData)
        {
            byte[] colors = new byte[depthData.Length];
            Parallel.For(0, depthData.Length, index =>
            {
                ushort depth = depthData[index];
                colors[index] = depth > Kinect.MaxDistance ? (byte)0 : (byte)(depth * MAP_DEPTH_TO_BYTE);
            });

            return colors;
        }
    }
}
