using Kamael.Packets;
using System;
using System.Windows.Forms;
using SharpPcap;

namespace Kamael.UI
{
    public partial class frmMain : Form
    {
        
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            

            // Wait for 'Enter' from the user.
            //Console.ReadLine();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Stop the capturing process
            device.StopCapture();
            Console.WriteLine("-- Capture stopped.");

            // Print out the device statistics
            Console.WriteLine(device.Statistics.ToString());

            // Close the pcap device
            device.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tmpStr = "";
            for (int i = 0; i <= 11025; i++)
            {
                tmpStr += string.Format("case {0}: break;//return new \r\n", i);
                Application.DoEvents();
            }

            txtLog.Text = tmpStr;
        }
    }
}