using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GScanning
{
    class Kinect
    {
        private KinectSensor kinect;

        private MultiSourceFrameReader multiSourceFrameReader;

        private bool write = false;

        private uint maxFramesCount = 0;

        public uint MaxFramesCount
        {
            get
            {
                return maxFramesCount;
            }

            set
            {
                maxFramesCount = value;
            }
        }

        public Kinect()
        {
            this.kinect = KinectSensor.GetDefault();
            this.multiSourceFrameReader = this.kinect.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.Color);
            this.multiSourceFrameReader.MultiSourceFrameArrived += Reader_FrameArrived;
            this.kinect.Open();
        }

        public void StartWriting()
        {
            this.write = true;
        }

        public void StopWriting()
        {
            this.write = false;
        }

        private void Reader_FrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            try
            {
                MultiSourceFrame multiSourceFrame = e.FrameReference.AcquireFrame();
                if (multiSourceFrame != null)
                {
                    Frame frame = new Frame(kinect);
                    frame.ProcessDepthFrame(multiSourceFrame);
                    frame.ProcessColorFrame(multiSourceFrame);
                    if (frame.DepthData != null)
                    {
                        new Visualisation(frame).Visualise();
                        if (write && frame.ColorData != null)
                        {
                            Writer writer = new Writer(frame);
                            Thread thread = new Thread(new ThreadStart(writer.Write));
                            thread.Start();
                            if (Writer.Counter == maxFramesCount)
                            {
                                StopWriting();
                                Form1.StatusLabel.Text = "Writing is finished.";                              
                            }
                        }
                    }
                }
            }
            catch
            {
                Form1.StatusLabel.Text = "Frame is not read properly.";
            }
        }
    }
}
