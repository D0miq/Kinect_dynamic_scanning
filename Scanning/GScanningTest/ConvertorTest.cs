namespace GScanningTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using GScanning;
    using System.Linq;

    /// <summary>
    /// Tests the <see cref="Convertor"/> class.
    /// </summary>
    [TestClass]
    public class ConvertorTest
    {
        /// <summary>
        /// Tests the <see cref="Convertor.ConvertDepthToColor(ushort[])"/>
        /// </summary>
        [TestMethod]
        public void TestConvertDepthToColor()
        {
            ushort[] depthData = { 0, 2000, 4000, 6000, 8000 , 10000};
            byte[] colors = Convertor.ConvertDepthToColor(depthData);
            Assert.IsTrue(colors.SequenceEqual(new byte[] { 0, 63, 127, 191, 255, 0}));
        }
    }
}
