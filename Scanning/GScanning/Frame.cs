using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GScanning
{
    class Frame
    {
        private const int BYTES_PER_PIXEL = 4;

        private ushort[] depthData;

        private byte[] colorData;

        private FrameDescription depthFrameDescription;

        private FrameDescription colorFrameDescription;

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
            this.depthFrameDescription = kinect.DepthFrameSource.FrameDescription;
            this.colorFrameDescription = kinect.ColorFrameSource.FrameDescription;
        }

        public void ProcessDepthFrame(MultiSourceFrame multiSourceFrame)
        {
            DepthFrameReference depthFrameReference = multiSourceFrame.DepthFrameReference;
            using (DepthFrame depthFrame = depthFrameReference.AcquireFrame())
            {
                if (depthFrame == null)
                {
                     this.depthData = null;
                    return;
                }

                this.depthData = new ushort[this.depthFrameDescription.LengthInPixels];
                depthFrame.CopyFrameDataToArray(this.depthData);
            }
        }

        public void ProcessColorFrame(MultiSourceFrame multiSourceFrame)
        {
            ColorFrameReference colorFrameReference = multiSourceFrame.ColorFrameReference;
            using (ColorFrame colorFrame = colorFrameReference.AcquireFrame())
            {
                if (colorFrame == null)
                {
                    this.colorData = null;
                    return;
                }

                this.colorData = new byte[this.colorFrameDescription.LengthInPixels * BYTES_PER_PIXEL];
                colorFrame.CopyConvertedFrameDataToArray(this.colorData, ColorImageFormat.Rgba);
            }
        }
    }
}
