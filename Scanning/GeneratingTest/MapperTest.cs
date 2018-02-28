namespace GeneratingTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Drawing;
    using Generating;
    using Microsoft.Kinect;
    using System.Linq;

    /// <summary>
    /// Test the <see cref="Mapper"/> class.
    /// </summary>
    [TestClass]
    public class MapperTest
    {
        /// <summary>
        /// Tests the <see cref="Mapper.MapColorToDepth(byte[], ColorSpacePoint[], int, int)"/>.
        /// </summary>
        [TestMethod]
        public void TestMapColorToDepth()
        {
            byte[] colorData = { 255, 0, 0, 255, 0, 255, 0, 255 , 0, 0, 255, 255, 255, 255, 0, 255};
            ColorSpacePoint spacePoint1 = new ColorSpacePoint
            {
                X = 0,
                Y = 0
            };

            ColorSpacePoint spacePoint2 = new ColorSpacePoint
            {
                X = 1,
                Y = 1
            };

            ColorSpacePoint[] spacePoints = {spacePoint1, spacePoint2};
            byte[] mappedColors = Mapper.MapColorToDepth(colorData, spacePoints, 2, 2);
            Assert.IsTrue(mappedColors.SequenceEqual(new byte[] { 255,0,0,255,255,255,0,255}));
        }

        /// <summary>
        /// Tests the <see cref="Mapper.MapRgbaToColors(byte[])"/>.
        /// </summary>
        [TestMethod]
        public void TestMapRgbaToColor()
        {
            byte[] colorData = { 255, 0, 0, 255, 0, 255, 0, 255 };

            Color[] colors = Mapper.MapRgbaToColors(colorData);

            Assert.AreEqual(colors[0].R, colorData[0]);
            Assert.AreEqual(colors[0].G, colorData[1]);
            Assert.AreEqual(colors[0].B, colorData[2]);
            Assert.AreEqual(colors[0].A, colorData[3]);
            Assert.AreEqual(colors[1].R, colorData[4]);
            Assert.AreEqual(colors[1].G, colorData[5]);
            Assert.AreEqual(colors[1].B, colorData[6]);
            Assert.AreEqual(colors[1].A, colorData[7]);
        }
    }
}
