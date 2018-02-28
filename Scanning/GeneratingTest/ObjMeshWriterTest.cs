namespace GeneratingTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Generating;
    using TypeMock.ArrangeActAssert;
    using Microsoft.Kinect;
    using System.Drawing;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Tests the <see cref="ObjMeshWriter"/> class.
    /// </summary>
    [TestClass]
    public class ObjMeshWriterTest
    {
        /// <summary>
        /// The name of a file.
        /// </summary>
        private const string FILE_NAME = "testFile";

        /// <summary>
        /// Tests the <see cref="ObjMeshWriter.WriteMesh(Mesh, string)"/>. It tests file existance and its content.
        /// </summary>
        [TestMethod]
        public void TestWriteMesh()
        {
            IMeshWriter meshWriter = new ObjMeshWriter();
            Mesh mesh = Isolate.Fake.Instance<Mesh>();
            CameraSpacePoint[] vertexes = { new CameraSpacePoint { X = 1, Y = 1, Z = 1 }, new CameraSpacePoint { X = 2, Y = 2, Z = 1 }, new CameraSpacePoint { X = 1, Y = 2, Z = 1 } };
            Color[] colors = { Color.Red, Color.Lime, Color.Blue };
            List<int[]> triangles = new List<int[]>(new int[][]{ new int[] { 1, 2, 3} });
            Isolate.WhenCalled(() => mesh.Vertexes).WillReturn(vertexes);
            Isolate.WhenCalled(() => mesh.Colors).WillReturn(colors);
            Isolate.WhenCalled(() => mesh.Triangles).WillReturn(triangles);
            meshWriter.WriteMesh(mesh, FILE_NAME);
            Assert.IsTrue(File.Exists("Meshes\\" + FILE_NAME + ".obj"));

            StreamReader streamReader = new StreamReader("Meshes\\" + FILE_NAME + ".obj");
            Assert.AreEqual("v 1.0000000000 1.0000000000 1.0000000000 1.0000000000 0.0000000000 0.0000000000", streamReader.ReadLine());
            Assert.AreEqual("v 2.0000000000 2.0000000000 1.0000000000 0.0000000000 1.0000000000 0.0000000000", streamReader.ReadLine());
            Assert.AreEqual("v 1.0000000000 2.0000000000 1.0000000000 0.0000000000 0.0000000000 1.0000000000", streamReader.ReadLine());
            Assert.AreEqual("f 1 2 3", streamReader.ReadLine());
            streamReader.Close();
        }
    }
}
