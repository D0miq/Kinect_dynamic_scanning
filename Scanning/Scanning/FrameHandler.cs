namespace Scanning
{
    using System;
    using System.Threading;
    using Microsoft.Kinect;

    /// <summary>
    /// Delegate for the <see cref="FrameHandler.Finished"/> event.
    /// </summary>
    /// <param name="sender">The sender of an event.</param>
    /// <param name="e">Arguments of an event.</param>
    public delegate void FinishedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// An instance of the <see cref="FrameHandler"/> class represents a frame handler that is used to process a given number of incoming frames.
    /// Everytime the frame is processed it calls a user-specified action. The action is called in a new <see cref="Thread"/> by default but can be changed with constructor or setter.
    /// The class creates an event when the execution of requested number of frames is finished to inform everybody who is listening.
    /// </summary>
    public class FrameHandler
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The requested number of frames.
        /// </summary>
        private uint targetFramesCount;

        /// <summary>
        /// The counter of processed frames.
        /// </summary>
        private int currentFramesCount = 0;

        /// <summary>
        /// The user-specified action performed with every frame.
        /// </summary>
        private Action<Frame> frameAction;

        /// <summary>
        /// The field indicates if action performed with every new frame should be called in a new thread.
        /// </summary>
        private bool isMultithreaded;

        /// <summary>
        /// Gets or sets a value indicating whether action performed with every new frame should be called in a new thread <see cref="isMultithreaded"/>.
        /// </summary>
        public bool IsMultithreaded
        {
            get
            {
                return this.isMultithreaded;
            }

            set
            {
                this.isMultithreaded = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameHandler"/> class. Multithreading is turned on.
        /// </summary>
        /// <param name="targetFramesCount">Used to stop the frame processing.</param>
        /// <param name="frameAction">Performed with every frame.</param>
        /// <seealso cref="BinFrameWriter"/>
        public FrameHandler(uint targetFramesCount, Action<Frame> frameAction)
            : this(targetFramesCount, frameAction, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameHandler"/> class.
        /// </summary>
        /// <param name="targetFramesCount">Used to stop the frame processing.</param>
        /// <param name="frameAction">Performed with every frame.</param>
        /// <param name="isMultithreaded">Indicates if action performed with every new frame should be called in a new thread.</param>
        /// <seealso cref="BinFrameWriter"/>
        public FrameHandler(uint targetFramesCount, Action<Frame> frameAction, bool isMultithreaded)
        {
            Log.Debug("Number of frames: " + targetFramesCount);
            this.targetFramesCount = targetFramesCount;
            this.frameAction = frameAction;
            this.isMultithreaded = isMultithreaded;
        }

        /// <summary>
        /// The event that fires when the <see cref="FrameHandler"/> processed all requested frames.
        /// </summary>
        public event FinishedEventHandler Finished;

        /// <summary>
        /// Handles incoming frames from <see cref="MultiSourceFrameReader"/>.
        /// </summary>
        /// <param name="sender">The sender of an event.</param>
        /// <param name="e">Arguments of an event.</param>
        /// <seealso cref="Kinect"/>
        public void Handler_FrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            try
            {
                MultiSourceFrame multiSourceFrame = e.FrameReference.AcquireFrame();
                if (multiSourceFrame != null)
                {
                    if (this.currentFramesCount < this.targetFramesCount)
                    {
                        Frame frame = new Frame(multiSourceFrame);
                        if (frame.AcquireData())
                        {
                            frame.ID = ++this.currentFramesCount;
                            Log.Info(this.currentFramesCount + " of " + this.targetFramesCount + " frames was read properly.");
                            if (this.isMultithreaded)
                            {
                                Log.Info("Starting the given action in a new thread.");
                                Thread thread = new Thread(new ThreadStart(() => this.frameAction.Invoke(frame)));
                                thread.Start();
                            }
                            else
                            {
                                Log.Info("Starting the given action in the current thread.");
                                this.frameAction.Invoke(frame);
                            }
                        }
                    }
                    else
                    {
                        Log.Info("Scanning is finished.");
                        ((MultiSourceFrameReader)sender).MultiSourceFrameArrived -= this.Handler_FrameArrived;
                        this.OnFinished(EventArgs.Empty);
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
        /// Invokes the <see cref="Finished"/> event.
        /// </summary>
        /// <param name="e">Arguments of an event.</param>
        protected virtual void OnFinished(EventArgs e)
        {
            this.Finished?.Invoke(this, e);
        }
    }
}
