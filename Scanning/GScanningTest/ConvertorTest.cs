using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GScanning;
using System.Linq;

namespace GScanningTest
{
    [TestClass]
    public class ConvertorTest
    {
        [TestMethod]
        public void TestConvertDepthToColor()
        {
            ushort[] depthData = { 0, 2000, 4000, 6000, 8000 , 10000};
            byte[] colors = Convertor.ConvertDepthToColor(depthData);
            Assert.IsTrue(colors.SequenceEqual(new byte[] { 0, 63, 127, 191, 255, 0}));
        }
    }
}
