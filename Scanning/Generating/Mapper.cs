namespace Generating
{
    using System;
    using System.Drawing;
    using System.Threading.Tasks;
    using Microsoft.Kinect;

    /// <summary>
    /// 
    /// </summary>
    public class Mapper
    {
        /// <summary>
        /// 
        /// </summary>
        private const int BYTES_PER_PIXEL = 4;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colorData"></param>
        /// <param name="colorSpacePoint"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static byte[] MapColorToDepth(byte[] colorData, ColorSpacePoint[] colorSpacePoint, int width, int height)
        {
            byte[] colorPixels = new byte[colorData.Length];
            Parallel.For(0, colorSpacePoint.Length, index =>
            {
                ColorSpacePoint point = colorSpacePoint[index];

                int colorX = (int) Math.Floor(point.X + 0.5);
                int colorY = (int) Math.Floor(point.Y + 0.5);

                if ((colorX >= 0) && (colorX < width) && (colorY >= 0) && (colorY < height))
                {
                    int colorImageIndex = ((width * colorY) + colorX) * BYTES_PER_PIXEL;
                    int depthPixel = index * BYTES_PER_PIXEL;

                    colorPixels[depthPixel] = colorData[colorImageIndex];
                    colorPixels[depthPixel + 1] = colorData[colorImageIndex + 1];
                    colorPixels[depthPixel + 2] = colorData[colorImageIndex + 2];
                    colorPixels[depthPixel + 3] = colorData[colorImageIndex + 3];
                }
            });

            return colorPixels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colorValues"></param>
        /// <returns></returns>
        public static Color[] MapRgbaToColors(byte[] colorValues)
        {
            Color[] colors = new Color[colorValues.Length / BYTES_PER_PIXEL];
            int k = 0;
            for (int i = 0; i < colorValues.Length; i += BYTES_PER_PIXEL)
            {
                colors[k] = Color.FromArgb(colorValues[i + 3], colorValues[i], colorValues[i + 1], colorValues[i + 2]);
                k++;
            }

            return colors;
        }
    }
}
