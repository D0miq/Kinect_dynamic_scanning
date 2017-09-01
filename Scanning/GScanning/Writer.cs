using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GScanning
{
    class Writer
    {
        private static int counter = 0;

        public static int Counter
        {
            get
            {
                return counter;
            }
        }

        private int index;

        private Frame frame;

        public Writer(Frame frame)
        {
            counter++;
            this.index = counter;
            this.frame = frame;
        }

        public void Write()
        {
            try
            {
                BinaryWriter bw = new BinaryWriter(new FileStream("frame" + index, FileMode.Create));
                bw.Write(this.frame.DepthData.Length);
                bw.Write(this.frame.ColorData.Length);
                foreach (ushort depthPixel in this.frame.DepthData)
                {
                    bw.Write(depthPixel);
                }
                foreach (byte colorPixel in this.frame.ColorData)
                {
                    bw.Write(colorPixel);
                }
                bw.Close();
            }
            catch
            {
                Form1.StatusLabel.Text = "Frame" + index + " was not written into a file.";
            }
        }
    }
}
