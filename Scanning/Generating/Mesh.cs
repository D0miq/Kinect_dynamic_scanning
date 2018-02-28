namespace Generating
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading.Tasks;
    using Microsoft.Kinect;

    /// <summary>
    /// An instance of the <see cref="Mesh"/> class represents a final mesh created from depth and color data.
    /// </summary>
    /// <seealso cref="IMeshWriter"/>
    /// <seealso cref="Frame"/>
    public class Mesh
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The array of vertexes.
        /// </summary>
        private CameraSpacePoint[] vertexes;

        /// <summary>
        /// The array of colors.
        /// </summary>
        private Color[] colors;

        /// <summary>
        /// The list of triangles.
        /// </summary>
        private List<int[]> triangles;

        /// <summary>
        /// The array of indices.
        /// </summary>
        private int[] indices;

        /// <summary>
        /// The length of <see cref="vertexes"/> and <see cref="colors"/>.
        /// </summary>
        private int freeIndex = 0;

        /// <summary>
        /// A width of a depth frame.
        /// </summary>
        private int depthWidth = KinectSensor.GetDefault().DepthFrameSource.FrameDescription.Width;

        /// <summary>
        /// A height of a depth frame.
        /// </summary>
        private int depthHeight = KinectSensor.GetDefault().DepthFrameSource.FrameDescription.Height;

        /// <summary>
        /// Gets the Vertexes property that represents vertexes <see cref="vertexes"/> of a mesh.
        /// </summary>
        public CameraSpacePoint[] Vertexes
        {
            get
            {
                return this.vertexes;
            }
        }

        /// <summary>
        /// Gets the Colors property that represents colors <see cref="colors"/> of a mesh.
        /// </summary>
        public Color[] Colors
        {
            get
            {
                return this.colors;
            }
        }

        /// <summary>
        /// Gets the Triangles property that represents triangles <see cref="triangles"/> of a mesh.
        /// </summary>
        public List<int[]> Triangles
        {
            get
            {
                return this.triangles;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mesh"/> class.
        /// </summary>
        private Mesh()
        {
            this.indices = new int[this.depthWidth * this.depthHeight];
            for (int i = 0; i < this.indices.Length; i++)
            {
                this.indices[i] = -1;
            }

            this.triangles = new List<int[]>();
        }

        /// <summary>
        /// Transfers points from camera view into world coordinates
        /// </summary>
        private void CreateTriangles()
        {
            bool[] used = new bool[this.depthHeight * this.depthWidth];

            for (int x = 1; x < this.depthWidth - 1; x++)
            {
                for (int y = 1; y < this.depthHeight - 1; y++)
                {
                    if (this.CheckVertex((this.depthWidth * y) + x - 1)
                        && this.CheckVertex((this.depthWidth * y) + x)
                        && this.CheckVertex((this.depthWidth * y) + x + 1)
                        && this.CheckVertex((this.depthWidth * (y + 1)) + x - 1)
                        && this.CheckVertex((this.depthWidth * (y + 1)) + x)
                        && this.CheckVertex((this.depthWidth * (y + 1)) + x + 1)
                        && this.CheckVertex((this.depthWidth * (y - 1)) + x - 1)
                        && this.CheckVertex((this.depthWidth * (y - 1)) + x)
                        && this.CheckVertex((this.depthWidth * (y - 1)) + x + 1))
                    {
                        used[(this.depthWidth * y) + x] = true;
                    }
                }
            }

            for (int x = 0; x < this.depthWidth - 1; x++)
            {
                for (int y = 0; y < this.depthHeight - 1; y++)
                {
                    if (used[(this.depthWidth * y) + x]
                        && used[(this.depthWidth * y) + x + 1]
                        && used[(this.depthWidth * (y + 1)) + x]
                        && used[(this.depthWidth * (y + 1)) + x + 1])
                    {
                        int i1 = this.indices[(this.depthWidth * y) + x];
                        if (i1 < 0)
                        {
                            i1 = this.freeIndex;
                            this.freeIndex++;
                            this.indices[(this.depthWidth * y) + x] = i1;
                        }

                        int i2 = this.indices[(this.depthWidth * y) + x + 1];
                        if (i2 < 0)
                        {
                            i2 = this.freeIndex;
                            this.freeIndex++;
                            this.indices[(this.depthWidth * y) + x + 1] = i2;
                        }

                        int i3 = this.indices[(this.depthWidth * (y + 1)) + x];
                        if (i3 < 0)
                        {
                            i3 = this.freeIndex;
                            this.freeIndex++;
                            this.indices[(this.depthWidth * (y + 1)) + x] = i3;
                        }

                        int i4 = this.indices[(this.depthWidth * (y + 1)) + x + 1];
                        if (i4 < 0)
                        {
                            i4 = this.freeIndex;
                            this.freeIndex++;
                            this.indices[(this.depthWidth * (y + 1)) + x + 1] = i4;
                        }

                        this.triangles.Add(new int[] { i1 + 1, i2 + 1, i3 + 1 });
                        this.triangles.Add(new int[] { i2 + 1, i4 + 1, i3 + 1 });
                    }
                }
            }

            this.Reorder();
        }

        /// <summary>
        /// Reorders colors and vertexes.
        /// </summary>
        private void Reorder()
        {
            CameraSpacePoint[] reorderedPoints = new CameraSpacePoint[this.freeIndex];
            Color[] reorderedColors = new Color[this.freeIndex];
            Parallel.For(0, this.depthHeight * this.depthWidth, index =>
            {
                if (this.indices[index] > 0)
                {
                    reorderedPoints[this.indices[index]] = this.vertexes[index];
                    reorderedColors[this.indices[index]] = this.colors[index];
                }
            });

            this.vertexes = reorderedPoints;
            this.colors = reorderedColors;
        }

        /// <summary>
        /// Checks coordinates.
        /// </summary>
        /// <param name="index">Index of the pixel.</param>
        /// <returns>Data evaluation.</returns>
        private bool CheckVertex(int index)
        {
            if (!double.IsInfinity(this.vertexes[index].X))
            {
                if (!double.IsInfinity(this.vertexes[index].Y))
                {
                    if (!double.IsInfinity(this.vertexes[index].Z))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// An instance of the <see cref="Builder"/> class represents a builder of the <see cref="Mesh"/>.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Colors of a mesh.
            /// </summary>
            private Color[] colors;

            /// <summary>
            /// Vertexes of a mesh.
            /// </summary>
            private CameraSpacePoint[] cameraSpacePoints;

            /// <summary>
            /// Kinect sensor.
            /// </summary>
            private KinectSensor kinect;

            /// <summary>
            /// Number of depth points.
            /// </summary>
            private int spacePointsCount;

            /// <summary>
            /// Width of color frame.
            /// </summary>
            private int colorFrameWidth;

            /// <summary>
            /// Height of color frame.
            /// </summary>
            private int colorFrameHeight;

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            /// <param name="depthData">Depth values.</param>
            public Builder(ushort[] depthData)
            {
                this.kinect = KinectSensor.GetDefault();
                this.spacePointsCount = depthData.Length;
                this.colorFrameWidth = this.kinect.ColorFrameSource.FrameDescription.Width;
                this.colorFrameHeight = this.kinect.ColorFrameSource.FrameDescription.Height;
                this.colors = new Color[this.spacePointsCount];
                this.cameraSpacePoints = new CameraSpacePoint[this.spacePointsCount];
                this.kinect.CoordinateMapper.MapDepthFrameToCameraSpace(depthData, this.cameraSpacePoints);
            }

            /// <summary>
            /// Creates a mesh with speciied characteristics.
            /// </summary>
            /// <returns>The created mesh.</returns>
            public Mesh Build()
            {
                Mesh mesh = new Mesh
                {
                    vertexes = this.cameraSpacePoints,
                    colors = this.colors
                };

                mesh.CreateTriangles();
                return mesh;
            }

            /// <summary>
            /// Prepares colors for a mesh.
            /// </summary>
            /// <param name="colorsData">Color data.</param>
            /// <returns>An instance of the <see cref="Builder"/></returns>
            public Builder AddColors(byte[] colorsData)
            {
                ColorSpacePoint[] colorSpacePoints = new ColorSpacePoint[this.spacePointsCount];
                this.kinect.CoordinateMapper.MapCameraPointsToColorSpace(this.cameraSpacePoints, colorSpacePoints);
                byte[] mappedColors = Mapper.MapColorToDepth(colorsData, colorSpacePoints, this.colorFrameWidth, this.colorFrameHeight);
                Color[] colors = Mapper.MapRgbaToColors(mappedColors);
                this.colors = colors;
                return this;
            }
        }
    }
}
