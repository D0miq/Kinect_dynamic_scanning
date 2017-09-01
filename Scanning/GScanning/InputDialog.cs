using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GScanning
{
    public partial class InputDialog : Form
    {
        public string Value
        {
            get { return inputTextBox.Text.Trim(); }
        }

        public InputDialog(string title, string header)
        {
            InitializeComponent();

            this.Text = title;
            this.headerLabel.Text = header;
        }
    }
}
