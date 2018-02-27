namespace GScanning
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// An instance of the <see cref="MainForm"/> class reppresents a main window of the application.
    /// It provides informations about states of the application and shows preview of scanned frames.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// <see cref="TextBox"/> used as output for user.
        /// </summary>
        private static TextBox statusText;

        /// <summary>
        /// The instance of the <see cref="FrameHandler"/> class.
        /// </summary>
        private FrameHandler frameHadler;

        /// <summary>
        /// Initializes static members of the <see cref="MainForm"/> class.
        /// </summary>
        static MainForm()
        {
            statusText = new TextBox
            {
                Anchor = (AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left,
                Location = new System.Drawing.Point(3, 3),
                Multiline = true,
                Name = "statusText",
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Size = new System.Drawing.Size(264, 378),
                TabIndex = 1,
                WordWrap = true
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            Log.Info("Creating main form.");
            this.InitializeComponent();
            ImageControl imageControl = new ImageControl();
            imageControl.Image.Source = Visualisation.Bitmap;
            this.elementHost.Child = imageControl;
            this.tableLayoutPanel.Controls.Add(statusText, 2, 0);

            Kinect kinect = Kinect.Instance;
            Visualisation visualisation = new Visualisation();
            kinect.AddMultiSourceFrameEventHandler(visualisation.Visualisation_FrameArrived);
            kinect.Start();
        }

        /// <summary>
        /// Delegate for callback which secures thread-safe access to <see cref="TextBox.Text"/> property.
        /// </summary>
        /// <param name="text">Used as new text.</param>
        private delegate void SetTextCallback(string text);

        /// <summary>
        /// Appends new text to <see cref="statusText"/>. The method secures the access is thread-safe and adds the line separator to the end of a text.
        /// </summary>
        /// <param name="text">Appended text.</param>
        public static void SetStatusText(string text)
        {
            if (statusText.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStatusText);
                statusText.Invoke(d, new object[] { text });
            }
            else
            {
                Log.Debug("Appending text: " + text);
                statusText.AppendText(text + "\n");
            }
        }

        /// <summary>
        /// Handles <see cref="Button.OnClick(EventArgs)"/> event, asks for target frames count and starts <see cref="frameHadler"/>.
        /// </summary>
        /// <param name="sender">The sender of an event.</param>
        /// <param name="e">Arguments of an event.</param>
        private void ScanButton_Click(object sender, EventArgs e)
        {
            uint framesCount = 0;
            InputDialog dialog = new InputDialog("Input", "Enter a number of frames:");
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK || !uint.TryParse(dialog.Value, out framesCount))
            {
                SetStatusText("The entered number was not valid.");
                Log.Warn("The entered number was not valid: " + framesCount);
                return;
            }

            IFrameWriter frameWriter = new BinFrameWriter();
            this.frameHadler = new FrameHandler(framesCount, frameWriter.WriteFrame);
            this.frameHadler.Finished += this.Handler_ReadingFinished;
            Kinect.Instance.AddMultiSourceFrameEventHandler(this.frameHadler.Handler_FrameArrived);
        }

        /// <summary>
        /// Handles <see cref="FrameHandler.Finished"/> event and reports status.
        /// </summary>
        /// <param name="sender">The sender of an event.</param>
        /// <param name="e">Arguments of an event.</param>
        private void Handler_ReadingFinished(object sender, EventArgs e)
        {
            SetStatusText("All frames were scanned. Please wait until writing is finished...");
        }
    }
}
