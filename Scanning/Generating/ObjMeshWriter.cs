namespace Generating
{
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using Microsoft.Kinect;

    /// <summary>
    /// 
    /// </summary>
    public class ObjMeshWriter : IMeshWriter
    {
        /// <summary>
        /// 
        /// </summary>
        private NumberFormatInfo numberFormatInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjMeshWriter"/> class.
        /// </summary>
        public ObjMeshWriter()
        {
            this.numberFormatInfo = new NumberFormatInfo();
            this.numberFormatInfo.NumberDecimalSeparator = ".";
            this.numberFormatInfo.NumberGroupSeparator = string.Empty;
            this.numberFormatInfo.NumberDecimalDigits = 10;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesh"></param>
        public void WriteMesh(Mesh mesh, string fileName)
        {
            string path = "Meshes//" + fileName + ".obj";
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                for (int i = 0; i < mesh.vertexes.Length; i++)
                {
                    CameraSpacePoint point = mesh.vertexes[i];
                    Color color = mesh.colors[i];
                    streamWriter.WriteLine(
                        "v {0} {1} {2} {3} {4} {5}",
                        point.X.ToString("N", this.numberFormatInfo),
                        point.Y.ToString("N", this.numberFormatInfo),
                        point.Z.ToString("N", this.numberFormatInfo),
                        (color.R / 255.0).ToString("N", this.numberFormatInfo),
                        (color.G / 255.0).ToString("N", this.numberFormatInfo),
                        (color.B / 255.0).ToString("N", this.numberFormatInfo));
                }

                foreach (int[] triangle in mesh.triangles)
                {
                    streamWriter.WriteLine("f {0} {1} {2}", triangle[0], triangle[1], triangle[2]);
                }
            }
        }
    }
}
