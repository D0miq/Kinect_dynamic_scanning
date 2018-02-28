namespace GScanningTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Kinect;
    using TypeMock.ArrangeActAssert;
    using GScanning;
    using System.Threading;

    /// <summary>
    /// Tests the <see cref="FrameHandler"/> class.
    /// </summary>
    [TestClass]
    public class FrameHandlerTest
    {
        /// <summary>
        /// The mock <see cref="MultiSourceFrameReader"/>.
        /// </summary>
        private MultiSourceFrameReader sender;
        /// <summary>
        /// The mock <see cref="MultiSourceFrameArrivedEventArgs"/>.
        /// </summary>
        private MultiSourceFrameArrivedEventArgs eventArgs;
        /// <summary>
        /// The counter of read frames.
        /// </summary>
        private int counter;
        /// <summary>
        /// The ID of a frame.
        /// </summary>
        private int frameId;
        /// <summary>
        /// If finish event occurred.
        /// </summary>
        private bool isFinished;
        /// <summary>
        /// Semaphore.
        /// </summary>
        private ManualResetEventSlim finishedEvent;

        /// <summary>
        /// Initializes fields before every test method.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            sender = Isolate.Fake.Instance<MultiSourceFrameReader>();
            eventArgs = Isolate.Fake.Instance<MultiSourceFrameArrivedEventArgs>();
            finishedEvent = new ManualResetEventSlim();
            counter = 0;
            frameId = 0;
            isFinished = false;
        }

        /// <summary>
        /// Tests the <see cref="FrameHandler.Handler_FrameArrived(object, MultiSourceFrameArrivedEventArgs)"/> with an event running on a current thread.
        /// </summary>
        [TestMethod]
        public void TestFrameArrivedSingleThread()
        {
            FrameHandler frameHandler = new FrameHandler(1, ActionFrame, false);
            frameHandler.Handler_FrameArrived(sender, eventArgs);
            Assert.AreEqual(1, counter);
        }

        /// <summary>
        /// Tests the <see cref="FrameHandler.Handler_FrameArrived(object, MultiSourceFrameArrivedEventArgs)"/> with an event running on a new thread.
        /// </summary>
        [TestMethod]
        public void TestFrameArrivedMultithread()
        {
            FrameHandler frameHandler = new FrameHandler(1, ActionFrame);
            frameHandler.Handler_FrameArrived(sender, eventArgs);
            finishedEvent.Wait();
            Assert.AreEqual(1, counter);
        }

        /// <summary>
        /// Tests the <see cref="FrameHandler.Handler_FrameArrived(object, MultiSourceFrameArrivedEventArgs)"/> with null <see cref="MultiSourceFrame"/>.
        /// </summary>
        [TestMethod]
        public void TestFrameArrivedNullMultiSourceFrame()
        {
            Isolate.WhenCalled(() => eventArgs.FrameReference.AcquireFrame()).WillReturn(null);
            FrameHandler frameHandler = new FrameHandler(1, ActionFrame, false);
            frameHandler.Handler_FrameArrived(sender, eventArgs);
            Assert.AreEqual(0, counter);
            Assert.AreEqual(0, frameId);
        }

        /// <summary>
        /// Tests a call of <see cref="FrameHandler.OnFinished(EventArgs)"/> in <see cref="FrameHandler.Handler_FrameArrived(object, MultiSourceFrameArrivedEventArgs)"/>.
        /// </summary>
        [TestMethod]
        public void TestFrameArrivedFinished()
        {
            FrameHandler frameHandler = new FrameHandler(0, ActionFrame, false);
            frameHandler.Finished += Finished;
            frameHandler.Handler_FrameArrived(sender, eventArgs);
            Assert.IsTrue(isFinished);
            Assert.AreEqual(0, counter);
            Assert.AreEqual(0, frameId);
        }

        /// <summary>
        /// Tests getter and setter of <see cref="FrameHandler.IsMultithreaded"/>.
        /// </summary>
        [TestMethod]
        public void TestIsMultithreaded()
        {
            FrameHandler frameHandler = new FrameHandler(0, ActionFrame);
            frameHandler.IsMultithreaded = false;
            Assert.IsFalse(frameHandler.IsMultithreaded);
        }

        /// <summary>
        /// Event performed after a frame is read.
        /// </summary>
        /// <param name="frame">The read frame.</param>
        private void ActionFrame(Frame frame)
        {
            counter++;
            frameId = frame.ID;
            finishedEvent.Set();
        }

        /// <summary>
        /// Event called when reading is finished.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">Event parameters.</param>
        private void Finished(object sender, EventArgs e)
        {
            isFinished = true;
        }
    }
}
