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
        private static TextBox statusText;

        private delegate void SetTextCallback(string text);

        public static TextBox StatusText
        {
            get
            {
                return statusText;
            }
        }

        static Form1()
        {
            // 
            // statusLabel
            // 
            statusText = new System.Windows.Forms.TextBox();
            statusText.Anchor = System.Windows.Forms.AnchorStyles.None;
            statusText.AutoSize = true;
            statusText.Location = new System.Drawing.Point(492, 12);
            statusText.Name = "statusText";
            statusText.Size = new System.Drawing.Size(200, 40);
            statusText.ReadOnly = true;
            statusText.ScrollBars = ScrollBars.Vertical;
            statusText.Multiline = true;
            statusText.WordWrap = true;
            statusText.TabIndex = 2;
        }

        public static void SetStatusText(String text)
        {
            if (statusText.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStatusText);
                statusText.Invoke(d, new object[] { text });
            }
            else
            {
                statusText.AppendText(text + "\n");
            }
        }

        private Kinect kinect;

        public Form1()
        {
            InitializeComponent();
            this.kinect = new Kinect();
            ImageControl imageControl = new ImageControl();
            imageControl.Image.Source = Visualisation.Bitmap;
            this.elementHost.Child = imageControl;
            this.tableLayoutPanel.Controls.Add(statusText, 2, 0);
        }     

        private void scanButton_Click(object sender, EventArgs e)
        {
            uint framesCount = 0;
            InputDialog dialog = new InputDialog("Input", "Enter a number of frames:");
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK || !UInt32.TryParse(dialog.Value, out framesCount))
            {
                SetStatusText("The entered number was not valid.");
                return;
            }
            this.kinect.MaxFramesCount += framesCount;
            this.kinect.StartWriting();
        }
    }
}
