namespace Generating
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;
    using Microsoft.Kinect;

    /// <summary>
    /// Class provides mapping between diferent values.
    /// </summary>
    public class Mapper
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
        /// Maps colors to depth points.
        /// </summary>
        /// <param name="colorData">Colors.</param>
        /// <param name="colorSpacePoint">Depth points.</param>
        /// <param name="width">Width of the color frame.</param>
        /// <param name="height">Height of the color frame.</param>
        /// <returns>Mapped colors.</returns>
        public static byte[] MapColorToDepth(byte[] colorData, ColorSpacePoint[] colorSpacePoint, int width, int height)
        {
            Log.Debug("Color width: " + width);
            Log.Debug("Color height: " + height);

            byte[] colorPixels = new byte[colorSpacePoint.Length * BYTES_PER_PIXEL];
            Parallel.For(0, colorSpacePoint.Length, index =>
            {
                ColorSpacePoint point = colorSpacePoint[index];

                int colorX = (int) Math.Round(point.X);
                int colorY = (int) Math.Round(point.Y);
                Log.Debug("X: " + colorX + ", Y: " + colorY);

                if ((colorX >= 0) && (colorX < width) && (colorY >= 0) && (colorY < height))
                {
                    int colorImageIndex = ((width * colorY) + colorX) * BYTES_PER_PIXEL;
                    int depthPixel = index * BYTES_PER_PIXEL;

                    Log.Debug("Color image index: " + colorImageIndex);
                    Log.Debug("Depth pixel index: " + depthPixel);

                    colorPixels[depthPixel] = colorData[colorImageIndex];
                    colorPixels[depthPixel + 1] = colorData[colorImageIndex + 1];
                    colorPixels[depthPixel + 2] = colorData[colorImageIndex + 2];
                    colorPixels[depthPixel + 3] = colorData[colorImageIndex + 3];
                }
            });

            return colorPixels;
        }

        /// <summary>
        /// Maps rgba values to colors.
        /// </summary>
        /// <param name="colorValues">Rgba values.</param>
        /// <returns>Colors.</returns>
        public static Color[] MapRgbaToColors(byte[] colorValues)
        {
            Color[] colors = new Color[colorValues.Length / BYTES_PER_PIXEL];
            int k = 0;
            for (int i = 0; i < colorValues.Length; i += BYTES_PER_PIXEL)
            {
                colors[k] = Color.FromArgb(colorValues[i + 3], colorValues[i], colorValues[i + 1], colorValues[i + 2]);
                Log.Debug("From: [" + colorValues[i + 3] + ", " + colorValues[i] + ", " + colorValues[i + 1] + ", " + colorValues[i + 2] + "] to " + colors[k].Name);
                k++;
            }

            return colors;
        }
    }
}
