namespace Scanning
{
    using System;
    using Microsoft.Kinect;

    /// <summary>
    /// 
    /// </summary>
    public class Kinect
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Kinect Instance = new Kinect();

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        private KinectSensor kinect;

        /// <summary>
        /// 
        /// </summary>
        private Kinect()
        {
            this.kinect = KinectSensor.GetDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <param name="types"></param>
        public void OpenMultiSourceFrameReader(EventHandler<MultiSourceFrameArrivedEventArgs> eventHandler)
        {
            Log.Info("Opening frame reader.");
            MultiSourceFrameReader multiSourceFrameReader = this.kinect.OpenMultiSourceFrameReader(FrameSourceTypes.Depth | FrameSourceTypes.Color);
            multiSourceFrameReader.MultiSourceFrameArrived += eventHandler;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            if (!this.kinect.IsAvailable)
            {
                Log.Info("Starting kinect.");
                this.kinect.Open();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            if (this.kinect.IsAvailable)
            {
                Log.Info("Stoping kinect.");
                this.kinect.Close();
            }
        }
    }
}
