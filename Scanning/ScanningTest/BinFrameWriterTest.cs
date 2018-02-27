namespace ScanningTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scanning;
    using TypeMock.ArrangeActAssert;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Tests the <see cref="BinFrameWriter"/> class.
    /// </summary>
    [TestClass]
    public class BinFrameWriterTest
    {
        /// <summary>
        /// The frame writer.
        /// </summary>
        private IFrameWriter binFrameWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinFrameWriterTest"/> class.
        /// </summary>
        public BinFrameWriterTest()
        {
            binFrameWriter = new BinFrameWriter();
        }

        /// <summary>
        /// Tests best case scenario of a <see cref="BinFrameWriter.WriteFrame(Frame)"/>. It tests existence of a created file and its content.
        /// </summary>
        [TestMethod]
        public void TestWriteFrame()
        {
            Frame frame = Isolate.Fake.Instance<Frame>();
            frame.ID = 1;

            ushort[] initDepthValues = { 1, 2, 3 };
            byte[] initColorValues = { 1, 2, 3 };
            Isolate.WhenCalled(() => frame.DepthData).WillReturn(initDepthValues);
            Isolate.WhenCalled(() => frame.ColorData).WillReturn(initColorValues);

            binFrameWriter.WriteFrame(frame);
            Assert.IsTrue(File.Exists("Scans\\Frame1.bin"));

            BinaryReader binaryReader = new BinaryReader(new FileStream("Scans\\Frame1.bin", FileMode.Open));
            int depthLength = binaryReader.ReadInt32();
            int colorLength = binaryReader.ReadInt32();
            Assert.AreEqual(3, depthLength);
            Assert.AreEqual(3, colorLength);

            ushort[] depthValues = { binaryReader.ReadUInt16(), binaryReader.ReadUInt16(), binaryReader.ReadUInt16() };
            byte[] colorValues = binaryReader.ReadBytes(3);
            binaryReader.Close();

            Assert.IsTrue(initColorValues.SequenceEqual(colorValues));
            Assert.IsTrue(initDepthValues.SequenceEqual(depthValues));
            
        }
    }
}
