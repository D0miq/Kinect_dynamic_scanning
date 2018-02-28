namespace Generating
{
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using Microsoft.Kinect;

    /// <summary>
    /// The <see cref="Program"/> class is the main class of the application and serves as an entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// True if application should generate colors, else otherwise.
        /// </summary>
        private static bool withColors = false;

        /// <summary>
        /// Starts the entire application.
        /// </summary>
        /// <param name="args">Input agruments of the application.</param>
        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            KinectSensor kinect = KinectSensor.GetDefault();
            kinect.Open();

            CheckArguments(args);

            Console.WriteLine("Please choose binary files that will be transformed into object files.");

            SelectFiles(out string[] filesPaths, out string[] fileNames);
            foreach (var np in filesPaths.Zip(fileNames, Tuple.Create))
            {
                IFileReader reader = new BinFileReader();
                Frame frame = reader.ReadFile(np.Item1);
                if (frame.DepthData != null && frame.ColorData != null)
                {
                    var builder = new Mesh.Builder(frame.DepthData);
                    if (withColors)
                    {
                        builder.AddColors(frame.ColorData);
                    }

                    Mesh mesh = builder.Build();
                    IMeshWriter writer = new ObjMeshWriter();
                    writer.WriteMesh(mesh, np.Item2.Substring(0, np.Item2.LastIndexOf('.')));
                    Console.WriteLine(np.Item2 + " was generated into object file.");
                }
            }

            kinect.Close();
        }

        /// <summary>
        /// Checks input arguments.
        /// </summary>
        /// <param name="args">Arguments of the program.</param>
        private static void CheckArguments(string[] args)
        {
            if (args.Length != 0 && (args[0].ToLower().Equals("-color") || args[0].ToLower().Equals("-c")))
            {
                withColors = true;
            }
        }

        /// <summary>
        /// Creates an <see cref="OpenFileDialog"/> and saves paths and names of selected files.
        /// </summary>
        /// <param name="filePaths">Paths of selected files.</param>
        /// <param name="fileNames">Names of selected files.</param>
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
    }
}
