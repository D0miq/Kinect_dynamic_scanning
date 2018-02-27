namespace GScanningTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using GScanning;
    using System.Windows.Forms;
    using TypeMock.ArrangeActAssert;

    /// <summary>
    /// Tests the <see cref="MainForm"/> class.
    /// </summary>
    [TestClass]
    public class MainFormTest
    {
        /// <summary>
        /// Tests a setter of status text.
        /// </summary>
        [TestMethod]
        public void TestSetStatusText()
        {
            TextBox statusText = new TextBox();
            Isolate.NonPublic.StaticField<MainForm>("statusText").Value = statusText;
            MainForm.SetStatusText("Test");
            Assert.AreEqual("Test\n", statusText.Text);
        }
    }
}
