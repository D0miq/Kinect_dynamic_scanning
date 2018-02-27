namespace Scanning
{
    using System;
    using System.Threading;
    using Microsoft.Kinect;

    /// <summary>
    /// The <see cref="Program"/> class is the main class of the application and serves as an entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The number of frames that will be processed by the application.
        /// </summary>
        private static uint framesCount;

        /// <summary>
        /// The instance of the <see cref="ManualResetEvent"/> class. It is used to prevent closing of the application.
        /// </summary>
        private static ManualResetEvent waitHandle = new ManualResetEvent(false);

        /// <summary>
        /// The instance of the <see cref="Kinect"/> class.
        /// </summary>
        private static Kinect kinect = Kinect.Instance;

        /// <summary>
        /// Starts the entire application.
        /// </summary>
        /// <param name="args">Input agruments of the application.</param>
        public static void Main(string[] args)
        {
            Log.Info("Start of the application.");
            for (int i = 0; i < args.Length; i++)
            {
                Log.Debug("args[" + i + "]: " + args[i]);
            }

            CheckInputArguments(args);

            IFrameWriter frameWriter = new BinFrameWriter();
            FrameHandler frameHadler = new FrameHandler(framesCount, frameWriter.WriteFrame);
            frameHadler.Finished += Handler_ReadingFinished;

            kinect.OpenMultiSourceFrameReader(frameHadler.Handler_FrameArrived);
            kinect.Start();

            waitHandle.WaitOne();

            Log.Info("End of the main thread.");
        }

        /// <summary>
        /// Checks input arguments.
        /// </summary>
        /// <param name="args">Input agruments of the application.</param>
        private static void CheckInputArguments(string[] args)
        {
            if (args.Length == 0 || !uint.TryParse(args[0], out framesCount))
            {
                Console.WriteLine("The entered arguments were not valid. Please restart the application and use right values." + "\n" +
                    "Right usage:" + "\n" +
                    "Scanning.exe <numberOfFrames>" + "\n" +
                    "<numberOfFrames> = 0-" + uint.MaxValue + " (Keep in mind this value affects run time)");
                Log.Fatal("The entered program arguments were not valid.");
                Environment.Exit(1);
            }

            Log.Debug("Number of frames: " + framesCount);
        }

        /// <summary>
        /// Handles <see cref="FrameHandler.Finished"/>, stops kinect and ends the application.
        /// </summary>
        /// <param name="sender">The sender of an event.</param>
        /// <param name="e">Arguments of an event.</param>
        private static void Handler_ReadingFinished(object sender, EventArgs e)
        {
            Console.WriteLine("All frames were scanned. Please wait until writing is finished...");
            kinect.Stop();
            waitHandle.Set();
        }
    }
}
