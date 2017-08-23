using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scanning
{
    class Frame
    {
        private static int framesCounter = 0;

        private const int BYTES_PER_PIXEL = 4;

        private ushort[] depthData;

        private byte[] colorData;

        private FrameDescription depthFrameDescription;

        private FrameDescription colorFrameDescription;

        public static int FramesCounter
        {
            get
            {
                return Interlocked.CompareExchange(ref framesCounter, 0, 0);
            }
        }

        public ushort[] DepthData
        {
            get
            {
                return depthData;
            }
        }

        public byte[] ColorData
        {
            get
            {
                return colorData;
            }
        }

        public Frame(KinectSensor kinect)
        {
            depthFrameDescription = kinect.DepthFrameSource.FrameDescription;
            colorFrameDescription = kinect.ColorFrameSource.FrameDescription;
        }

        public void ProcessDepthFrame(MultiSourceFrame multiSourceFrame)
        {
            DepthFrameReference depthFrameReference = multiSourceFrame.DepthFrameReference;
            using (DepthFrame depthFrame = depthFrameReference.AcquireFrame())
            {
                if (depthFrame == null)
                {
                     depthData = null;
                }

                depthData = new ushort[depthFrameDescription.LengthInPixels];
                depthFrame.CopyFrameDataToArray(depthData);
            }
        }

        public void ProcessColorFrame(MultiSourceFrame multiSourceFrame)
        {
            ColorFrameReference colorFrameReference = multiSourceFrame.ColorFrameReference;
            using (ColorFrame colorFrame = colorFrameReference.AcquireFrame())
            {
                if (colorFrame == null)
                {
                    colorData = null;
                }

                colorData = new byte[colorFrameDescription.LengthInPixels * BYTES_PER_PIXEL];
                colorFrame.CopyConvertedFrameDataToArray(colorData, ColorImageFormat.Rgba);
            }
        }

        public void WriteFrame()
        {
            Interlocked.Increment(ref framesCounter);
            Console.WriteLine("Frame " + Interlocked.CompareExchange(ref framesCounter, 0, 0) + " is written into file.");
            try
            {
                BinaryWriter bw = new BinaryWriter(new FileStream("frame" + Interlocked.CompareExchange(ref framesCounter, 0, 0), FileMode.Create));
                bw.Write(this.depthData.Length);
                bw.Write(this.colorData.Length);
                foreach (ushort depthPixel in this.depthData)
                {
                    bw.Write(depthPixel);
                }
                foreach (byte colorPixel in this.colorData)
                {
                    bw.Write(colorPixel);
                }
                bw.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot write to file.");
                Interlocked.Decrement(ref framesCounter);
            }
        }
    }
}
