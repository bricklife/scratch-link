using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace scratch_link.Dialogs
{
    public partial class AddressInUse : Form
    {
        string textContent;

        public AddressInUse()
        {
            InitializeComponent();

            pictureBox1.Image = SystemIcons.Error.ToBitmap();

            textContent = string.Format(
                "{0} was unable to start because port {1} is already in use.\n" +
                "\n" +
                "This means {0} is already running or another application is using that port.\n" +
                "\n" +
                "This application will now exit.",
                scratch_link.Properties.Resources.AppTitle,
                App.SDMPort
            );
            label1.Text = textContent;

            // detailsBox.Text is the label for the group box around the details
            textContent += "\n\n" + detailsBox.Text + ":\n";

            var pid = Fiddler.Winsock.MapLocalPortToProcessId(App.SDMPort);
            if (pid > 0)
            {
                var process = Process.GetProcessById(pid);
                AddProcessPropertyValue("Process Name", process.ProcessName);
                AddProcessPropertyValue("Window Title", process.MainWindowTitle);
                AddProcessPropertyValue("File Name", process.MainModule.FileName);
                AddProcessPropertyValue("Process ID", pid.ToString());
                AddProcessPropertyValue("TCP Port", App.SDMPort.ToString());
                labelNoResults.Visible = false;
                listView1.Visible = true;
            }
            else
            {
                textContent += labelNoResults.Text + '\n';
                labelNoResults.Visible = true;
                listView1.Visible = false;
            }
        }

        private void AddProcessPropertyValue(string property, string value)
        {
            listView1.Items.Add(new ListViewItem(new[] { property, value }));
            textContent += property + ":\t" + value + "\n";
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textContent);
        }
    }
}
