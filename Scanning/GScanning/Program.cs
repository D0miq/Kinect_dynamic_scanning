namespace GScanning
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// The <see cref="Program"/> class is the main class of the application and serves as an entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Log.Info("Starting the application.");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
