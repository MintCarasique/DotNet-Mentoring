using System;
using System.Windows.Forms;
using HelloWorldStandard;

namespace HelloWorldForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            var name = inputTextBox.Text;

            var hello = new HelloWorld();

            resultTextBox.Text = hello.ReturnHelloMessage(name);
        }
    }
}
