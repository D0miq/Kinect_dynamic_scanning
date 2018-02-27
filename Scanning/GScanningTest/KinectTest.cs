namespace GScanningTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Kinect;
    using TypeMock.ArrangeActAssert;
    using GScanning;

    /// <summary>
    /// Tests the <see cref="Kinect"/> class.
    /// </summary>
    [TestClass]
    public class KinectTest
    {
        /// <summary>
        /// The mock <see cref="KinectSensor"/>.
        /// </summary>
        private KinectSensor kinectSensor;
        /// <summary>
        /// If kinect is open.
        /// </summary>
        private bool isOpen;
        /// <summary>
        /// Tested <see cref="Kinect"/> class.
        /// </summary>
        private Kinect kinect;

        /// <summary>
        /// Initializes a new instance of the <see cref="KinectTest"/> class.
        /// </summary>
        public KinectTest()
        {
            kinectSensor = Isolate.Fake.Instance<KinectSensor>();
            Isolate.WhenCalled(() => kinectSensor.Open()).DoInstead(x => isOpen = true);
            Isolate.WhenCalled(() => kinectSensor.Close()).DoInstead(x => isOpen = false);
            kinect = Kinect.Instance;
            Isolate.NonPublic.InstanceField(kinect, "kinect").Value = kinectSensor;
        }

        /// <summary>
        /// Tests if <see cref="Kinect.Start"/> opens a kinect sensor.
        /// </summary>
        [TestMethod]
        public void TestStart()
        {
            isOpen = false;
            Isolate.WhenCalled(() => kinectSensor.IsAvailable).WillReturn(false);
            kinect.Start();
            Assert.IsTrue(isOpen);
        }

        /// <summary>
        /// Tests if <see cref="Kinect.Start"/> works when kinect sensor is alredy started.
        /// </summary>
        [TestMethod]
        public void TestStartAvailable()
        {
            isOpen = true;
            Isolate.WhenCalled(() => kinectSensor.IsAvailable).WillReturn(true);
            kinect.Start();
            Assert.IsTrue(isOpen);
        }

        /// <summary>
        /// Tests if <see cref="Kinect.Start"/> closes a kinect sensor.
        /// </summary>
        [TestMethod]
        public void TestStop()
        {
            isOpen = true;
            Isolate.WhenCalled(() => kinectSensor.IsAvailable).WillReturn(true);
            kinect.Stop();
            Assert.IsFalse(isOpen);
        }

        /// <summary>
        /// Tests if <see cref="Kinect.Start"/> works when kinect sensor is alredy stopped.
        /// </summary>
        [TestMethod]
        public void TestStopNotAvailable()
        {
            isOpen = false;
            Isolate.WhenCalled(() => kinectSensor.IsAvailable).WillReturn(false);
            kinect.Stop();
            Assert.IsFalse(isOpen);
        }
    }
}
