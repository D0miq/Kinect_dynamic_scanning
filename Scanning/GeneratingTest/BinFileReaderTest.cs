namespace GeneratingTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Generating;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Test the <see cref="BinFrameReader"/> class.
    /// </summary>
    [TestClass]
    public class BinFileReaderTest
    {
        /// <summary>
        /// The frame reader.
        /// </summary>
        private IFileReader binFileReader;

        /// <summary>
        /// The name of a file.
        /// </summary>
        private const string FILE_NAME = "testFile";

        /// <summary>
        /// Initializes a new instance of the <see cref="BinFileReaderTest"/> class.
        /// </summary>
        public BinFileReaderTest()
        {
            binFileReader = new BinFileReader();
        }

        /// <summary>
        /// Tests the <see cref="BinFileReader.ReadFile"/>. It tests content of a file.
        /// </summary>
        [TestMethod]
        public void TestReadFile()
        {
            ushort[] depthData = { 1, 2, 3 };
            byte[] colorData = { 1, 2, 3 };
            BinaryWriter bw = new BinaryWriter(new FileStream(FILE_NAME, FileMode.Create));

            bw.Write(depthData.Length);
            Array.ForEach(depthData, bw.Write);
            bw.Write(colorData.Length);
            Array.ForEach(colorData, bw.Write);

            bw.Close();

            Frame frame = binFileReader.ReadFile(FILE_NAME);
            Assert.IsTrue(frame.DepthData.SequenceEqual(depthData));
            Assert.IsTrue(frame.ColorData.SequenceEqual(colorData));
        }
    }
}
