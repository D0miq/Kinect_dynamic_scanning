namespace Generating
{
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using Microsoft.Kinect;

    /// <summary>
    /// An instance of the <see cref="ObjMeshWriter"/> represents a writer to obj format.
    /// </summary>
    public class ObjMeshWriter : IMeshWriter
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Format of numbers in generated file.
        /// </summary>
        private NumberFormatInfo numberFormatInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjMeshWriter"/> class.
        /// </summary>
        public ObjMeshWriter()
        {
            this.numberFormatInfo = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = string.Empty,
                NumberDecimalDigits = 10
            };
        }

        /// <summary>
        /// Writes the given <paramref name="mesh"/> into obj file.
        /// </summary>
        /// <param name="mesh">The given mesh that is written into file.</param>
        /// <param name="fileName">Name of an output file.</param>
        public void WriteMesh(Mesh mesh, string fileName)
        {
            Log.Debug("File name: " + fileName);
            string path = "Meshes//" + fileName + ".obj";
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                for (int i = 0; i < mesh.Vertexes.Length; i++)
                {
                    CameraSpacePoint point = mesh.Vertexes[i];
                    Color color = mesh.Colors[i];
                    streamWriter.WriteLine(
                        "v {0} {1} {2} {3} {4} {5}",
                        point.X.ToString("N", this.numberFormatInfo),
                        point.Y.ToString("N", this.numberFormatInfo),
                        point.Z.ToString("N", this.numberFormatInfo),
                        (color.R / 255.0).ToString("N", this.numberFormatInfo),
                        (color.G / 255.0).ToString("N", this.numberFormatInfo),
                        (color.B / 255.0).ToString("N", this.numberFormatInfo));
                }

                foreach (int[] triangle in mesh.Triangles)
                {
                    streamWriter.WriteLine("f {0} {1} {2}", triangle[0], triangle[1], triangle[2]);
                }
            }
        }
    }
}
