namespace GScanning
{
    using System.Windows.Forms;

    /// <summary>
    /// An instance of the <see cref="InputDialog"/> class reppresents an input dialog.
    /// </summary>
    public partial class InputDialog : Form
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The Value property represents text value of <see cref="inputTextBox"/>.
        /// </summary>
        public string Value
        {
            get
            {
                return this.inputTextBox.Text.Trim();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDialog"/> class.
        /// </summary>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="header">The header of <see cref="inputTextBox"/>.</param>
        public InputDialog(string title, string header)
        {
            this.InitializeComponent();
            Log.Debug("Title:" + title);
            Log.Debug("Header: " + header);
            this.Text = title;
            this.headerLabel.Text = header;
        }
    }
}
