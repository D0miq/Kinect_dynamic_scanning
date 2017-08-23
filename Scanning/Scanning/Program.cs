using System;
using System.IO;
using Microsoft.Kinect;
using System.Threading;

namespace Scanning
{
    class Program
    {
        static KinectSensor kinect;

        static uint framesCount;

        static void Input()
        {
            Console.WriteLine("Enter a number of frames.");
            while (!UInt32.TryParse(Console.ReadLine(), out framesCount))
            {
                Console.WriteLine("The entered number was not valid. Please reenter the number of frames.");
            }
        }

        static void PrepareKinect()
        {
            kinect = KinectSensor.GetDefault();
            MultiSourceFrameReader multiSourceFrameReader = kinect.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.Color);
            multiSourceFrameReader.MultiSourceFrameArrived += Reader_FrameArrived;
        }

        private static void Reader_FrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            try
            {
                MultiSourceFrame multiSourceFrame = e.FrameReference.AcquireFrame();
                if (multiSourceFrame != null)
                {
                    Frame frame = new Frame(kinect);
                    frame.ProcessDepthFrame(multiSourceFrame);
                    frame.ProcessColorFrame(multiSourceFrame);
                    if(frame.DepthData != null && frame.ColorData != null)
                    {
                        Thread thread = new Thread(new ThreadStart(frame.WriteFrame));
                        thread.Start();
                    }
                }
            }
            catch
            {
                Console.WriteLine("Frame is not read properly.");
            }
        }

        static void Main(string[] args)
        {
            PrepareKinect();
            Input();
            kinect.Open();
            while(Frame.FramesCounter < framesCount)
            {
                
            }
            kinect.Close();
        }
    }
}
