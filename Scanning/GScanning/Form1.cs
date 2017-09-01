using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace GScanning
{
    public partial class Form1 : Form
    {
        private static Label statusLabel;

        public static Label StatusLabel
        {
            get
            {
                return statusLabel;
            }
        }

        static Form1()
        {
            // 
            // statusLabel
            // 
            statusLabel = new System.Windows.Forms.Label();
            statusLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            statusLabel.AutoSize = true;
            statusLabel.Location = new System.Drawing.Point(492, 12);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new System.Drawing.Size(0, 13);
            statusLabel.TabIndex = 2;
        }

        private Kinect kinect;

        public Form1()
        {
            InitializeComponent();
            this.kinect = new Kinect();
            ImageControl imageControl = new ImageControl();
            imageControl.Image.Source = Visualisation.Bitmap;
            this.elementHost.Child = imageControl;
            this.tableLayoutPanel.Controls.Add(statusLabel, 2, 0);
        }     

        private void scanButton_Click(object sender, EventArgs e)
        {
            uint framesCount = 0;
            InputDialog dialog = new InputDialog("Input", "Enter a number of frames:");
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK || !UInt32.TryParse(dialog.Value, out framesCount))
            {
                statusLabel.Text = "The entered number was not valid.";
                return;
            }
            statusLabel.Text = "";
            this.kinect.MaxFramesCount += framesCount;
            this.kinect.StartWriting();
        }
    }
}
