namespace GScanning
{
    using System.Windows.Forms;

    /// <summary>
    /// An instance of the <see cref="InputDialog"/> class reppresents an input dialog.
    /// </summary>
    public partial class InputDialog : Form
    {
        /// <summary>
        /// The Value property represents text value of <see cref="inputTextBox"/>.
        /// </summary>
        public string Value
        {
            get { return this.inputTextBox.Text.Trim(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputDialog"/> class.
        /// </summary>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="header">The header of <see cref="inputTextBox"/>.</param>
        public InputDialog(string title, string header)
        {
            this.InitializeComponent();

            this.Text = title;
            this.headerLabel.Text = header;
        }
    }
}
