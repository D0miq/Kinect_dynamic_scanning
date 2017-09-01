using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GScanning
{
    class Visualisation
    {
        /// <summary>
        /// Map depth range to byte range
        /// </summary>
        private const int MAP_DEPTH_TO_BYTE = 8000 / 256;

        private const int HEIGHT = 424;

        private const int WIDTH = 512;

        private static WriteableBitmap bitmap = new WriteableBitmap(WIDTH, HEIGHT, 96.0, 96.0, PixelFormats.Gray8, null);

        public static WriteableBitmap Bitmap
        {
            get
            {
                return bitmap;
            }
        }

        private Frame frame;

        public Visualisation(Frame frame)
        {
            this.frame = frame;
        }

        public void Visualise()
        {
            byte[] depthColors = GetColorFromDepth(this.frame.DepthData);
            bitmap.WritePixels(
                new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight),
                depthColors,
                bitmap.PixelWidth,
                0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depthData"></param>
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
