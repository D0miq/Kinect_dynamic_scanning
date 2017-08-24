using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generation
{
    class Program
    {
        static void SelectFiles(out string[] filePaths, out string[] fileNames)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    fileNames = dialog.SafeFileNames;
                    filePaths = dialog.FileNames;
                }else
                {
                    Console.WriteLine("No files were chosen. I am closing the application...");
                    fileNames = null;
                    filePaths = null;
                    Environment.Exit(1);
                }
            }
        }

        static void ReadFile(string filePath, out ushort[] depthData, out byte[] colorData)
        {
            try
            {
                BinaryReader br = new BinaryReader(new FileStream(filePath, FileMode.Open));
                int depthLength = br.ReadInt32();
                int colorLength = br.ReadInt32();
                depthData = new ushort[depthLength];
                for(int i = 0; i < depthLength; i++)
                {
                    depthData[i] = br.ReadUInt16();
                }
                colorData = br.ReadBytes(colorLength);
                br.Close();
            }catch
            {
                Console.WriteLine(filePath + " was not read properly!");
                depthData = null;
                colorData = null;
            }
        }

        [STAThreadAttribute]
        static void Main(string[] args)
        {
            bool withColors = false;
            if (args.Length != 0 && args[0].ToLower().Equals("-colors"))
            {
                withColors = true;
            }

            KinectSensor kinect = KinectSensor.GetDefault();
            kinect.Open();
            Console.WriteLine("Please choose binary files that will be transformed into object files.");
            string[] filesPaths;
            string[] fileNames;
            SelectFiles(out filesPaths, out fileNames);
            foreach (var np in filesPaths.Zip(fileNames, Tuple.Create))
            {
                ushort[] depthData;
                byte[] colorData;
                ReadFile(np.Item1, out depthData, out colorData);
                if (depthData != null && colorData != null)
                {
                    var builder = new Mesh.Builder(depthData);
                    if (withColors)
                    {
                        builder.AddColors(colorData);
                    }
                    Mesh mesh = builder.Build();
                    mesh.GenerateMesh(np.Item2);
                    Console.WriteLine(np.Item2 + " was generated into object file.");
                }
            }
            kinect.Close();
        }
    }
}
