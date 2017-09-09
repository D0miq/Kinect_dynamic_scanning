namespace Scanning
{
    using System;
    using System.Threading;
    using Microsoft.Kinect;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FinishedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class FrameHandler
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        private uint numberOfFrames;

        /// <summary>
        /// 
        /// </summary>
        private int framesCounter = 0;

        /// <summary>
        /// 
        /// </summary>
        private Action<Frame> frameAction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfFrames"></param>
        /// <param name="frameAction"></param>
        public FrameHandler(uint numberOfFrames, Action<Frame> frameAction)
        {
            Log.Debug("Number of Frames: " + numberOfFrames);
            this.numberOfFrames = numberOfFrames;
            this.frameAction = frameAction;
        }

        /// <summary>
        /// 
        /// </summary>
        public event FinishedEventHandler Finished;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Handler_FrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            try
            {
                MultiSourceFrame multiSourceFrame = e.FrameReference.AcquireFrame();
                if (multiSourceFrame != null)
                {
                    Frame frame = new Frame(multiSourceFrame);
                    if (frame.AcquireData())
                    {
                        frame.ID = ++this.framesCounter;
                        Log.Info("Frame number " + this.framesCounter + " was read properly.");
                        if (this.framesCounter <= this.numberOfFrames)
                        {
                            Log.Info("Starting given action in new thread.");
                            Thread thread = new Thread(new ThreadStart(() => this.frameAction.Invoke(frame)));
                            thread.Start();
                        }
                        else
                        {
                            Log.Info("Scanning finished.");
                            this.OnFinished(EventArgs.Empty);
                        }
                    } 
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Frame was not read properly.");
                Log.Error(exception.Message + " Frame was not read properly.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFinished(EventArgs e)
        {
            this.Finished?.Invoke(this, e);
        }
    }
}
