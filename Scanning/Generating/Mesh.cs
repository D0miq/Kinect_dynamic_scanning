namespace Generating
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading.Tasks;
    using Microsoft.Kinect;

    /// <summary>
    /// 
    /// </summary>
    public class Mesh
    {
        /// <summary>
        /// 
        /// </summary>
        public CameraSpacePoint[] vertexes;

        /// <summary>
        /// 
        /// </summary>
        public Color[] colors;

        /// <summary>
        /// 
        /// </summary>
        public List<int[]> triangles;

        /// <summary>
        /// 
        /// </summary>
        private int[] indices;

        /// <summary>
        /// 
        /// </summary>
        private int freeIndex = 0;

        /// <summary>
        /// 
        /// </summary>
        private int depthWidth = KinectSensor.GetDefault().DepthFrameSource.FrameDescription.Width;

        /// <summary>
        /// 
        /// </summary>
        private int depthHeight = KinectSensor.GetDefault().DepthFrameSource.FrameDescription.Height;

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
        /// 
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
        /// 
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// 
            /// </summary>
            private Color[] colors;

            /// <summary>
            /// 
            /// </summary>
            private CameraSpacePoint[] cameraSpacePoints;

            /// <summary>
            /// 
            /// </summary>
            private KinectSensor kinect;

            /// <summary>
            /// 
            /// </summary>
            private FrameDescription depthFrameDescription;

            /// <summary>
            /// 
            /// </summary>
            private FrameDescription colorFrameDescription;

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            /// <param name="depthData"></param>
            public Builder(ushort[] depthData)
            {
                this.kinect = KinectSensor.GetDefault();
                this.depthFrameDescription = this.kinect.DepthFrameSource.FrameDescription;
                this.colorFrameDescription = this.kinect.ColorFrameSource.FrameDescription;
                this.colors = new Color[this.depthFrameDescription.LengthInPixels];
                this.cameraSpacePoints = new CameraSpacePoint[this.depthFrameDescription.LengthInPixels];
                this.kinect.CoordinateMapper.MapDepthFrameToCameraSpace(depthData, this.cameraSpacePoints);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public Mesh Build()
            {
                Mesh mesh = new Mesh();
                mesh.vertexes = this.cameraSpacePoints;
                mesh.colors = this.colors;
                mesh.CreateTriangles();
                return mesh;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="colorsData"></param>
            /// <returns></returns>
            public Builder AddColors(byte[] colorsData)
            {
                ColorSpacePoint[] colorSpacePoints = new ColorSpacePoint[this.depthFrameDescription.LengthInPixels];
                this.kinect.CoordinateMapper.MapCameraPointsToColorSpace(this.cameraSpacePoints, colorSpacePoints);
                byte[] mappedColors = Mapper.MapColorToDepth(colorsData, colorSpacePoints, this.colorFrameDescription.Width, this.colorFrameDescription.Height);
                Color[] colors = Mapper.MapRgbaToColors(mappedColors);
                this.colors = colors;
                return this;
            }
        }
    }
}
