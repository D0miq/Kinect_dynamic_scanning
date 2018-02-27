namespace GScanningTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using GScanning;
    using Microsoft.Kinect;
    using TypeMock.ArrangeActAssert;

    /// <summary>
    /// Tests the <see cref="FrameHandler"/> class.
    /// </summary>
    [TestClass]
    public class FrameTest
    {
        /// <summary>
        /// Tested <see cref="Frame"/> class.
        /// </summary>
        private Frame frame;
        /// <summary>
        /// The mock <see cref="MultiSourceFrame"/>.
        /// </summary>
        private MultiSourceFrame multiSourceFrame;

        /// <summary>
        /// Initializes fields before every test method.
        /// </summary>
        [TestInitialize()]
        public void Initialize()
        {
            multiSourceFrame = Isolate.Fake.Instance<MultiSourceFrame>();
            frame = new Frame(multiSourceFrame);
        }

        /// <summary>
        /// Tests best case scenario of a <see cref="Frame.AcquireData"/>.
        /// </summary>
        [TestMethod]
        public void TestAcquireData()
        {
            Assert.IsTrue(frame.AcquireData());
        }

        /// <summary>
        /// Tests a <see cref="Frame.AcquireData"/> when depth data are null.
        /// </summary>
        [TestMethod]
        public void TestAcquireDataDepthNull()
        {
            Isolate.WhenCalled(() => multiSourceFrame.DepthFrameReference.AcquireFrame()).WillReturn(null);
            Assert.IsFalse(frame.AcquireData());
        }

        /// <summary>
        /// Tests a <see cref="Frame.AcquireData"/> when color data are null.
        /// </summary>
        [TestMethod]
        public void TestAcquireDataColorNull()
        {
            Isolate.WhenCalled(() => multiSourceFrame.ColorFrameReference.AcquireFrame()).WillReturn(null);
            Assert.IsFalse(frame.AcquireData());
        }

        /// <summary>
        /// Tests a <see cref="Frame.AcquireData"/> when both depth and color data are null.
        /// </summary>
        [TestMethod]
        public void TestAcquireDataBothNull()
        {
            Isolate.WhenCalled(() => multiSourceFrame.DepthFrameReference.AcquireFrame()).WillReturn(null);
            Isolate.WhenCalled(() => multiSourceFrame.ColorFrameReference.AcquireFrame()).WillReturn(null);
            Assert.IsFalse(frame.AcquireData());
        }

        /// <summary>
        /// Tests a getter and setter of <see cref="Frame.ID"/>.
        /// </summary>
        [TestMethod]
        public void TestID()
        {
            frame.ID = 10;
            Assert.AreEqual(10, frame.ID);
        }

        /// <summary>
        /// Tests a getter of <see cref="Frame.DepthData"/>.
        /// </summary>
        [TestMethod]
        public void TestDepthData()
        {
            ushort[] array = { 1, 2, 3 };
            Isolate.NonPublic.InstanceField(frame, "depthData").Value = array;

            Assert.AreEqual(array, frame.DepthData);
        }

        /// <summary>
        /// Tests a getter of <see cref="Frame.ColorData"/>.
        /// </summary>
        [TestMethod]
        public void TestColorData()
        {
            byte[] array = { 1, 2, 3 };
            Isolate.NonPublic.InstanceField(frame, "colorData").Value = array;

            Assert.AreEqual(array, frame.ColorData);
        }
    }
}
