namespace Generating
{
    using System;
    using System.Linq;
    using Microsoft.Kinect;
    using System.Windows.Forms;

    /// <summary>
    /// The <see cref="Program"/> class is the main class of the application and serves as an entry point.
    /// </summary>
    public class Program
    {
        private static bool withColors = false;

        private static void CheckArguments(string[] args)
        {
            if (args.Length != 0 && args[0].ToLower().Equals("-colors"))
            {
                withColors = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="fileNames"></param>
        private static void SelectFiles(out string[] filePaths, out string[] fileNames)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Filter = "bin files (*.bin)|*.bin";
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    fileNames = dialog.SafeFileNames;
                    filePaths = dialog.FileNames;
                }
                else
                {
                    Console.WriteLine("No files were chosen. I am closing the application...");
                    fileNames = null;
                    filePaths = null;
                    Environment.Exit(1);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            KinectSensor kinect = KinectSensor.GetDefault();
            kinect.Open();

            CheckArguments(args);

            Console.WriteLine("Please choose binary files that will be transformed into object files.");
            string[] filesPaths;
            string[] fileNames;

            SelectFiles(out filesPaths, out fileNames);
            foreach (var np in filesPaths.Zip(fileNames, Tuple.Create))
            {
                ushort[] depthData;
                byte[] colorData;
                IFileReader reader = new BinFileReader();
                reader.ReadFile(np.Item1, out depthData, out colorData);
                if (depthData != null && colorData != null)
                {
                    var builder = new Mesh.Builder(depthData);
                    if (withColors)
                    {
                        builder.AddColors(colorData);
                    }

                    Mesh mesh = builder.Build();
                    IMeshWriter writer = new ObjMeshWriter();
                    writer.WriteMesh(mesh, np.Item2);
                    Console.WriteLine(np.Item2 + " was generated into object file.");
                }
            }

            kinect.Close();
        }
    }
}
