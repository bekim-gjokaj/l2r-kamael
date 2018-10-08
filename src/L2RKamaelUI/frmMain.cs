using Kamael.Packets;
using System;
using System.Windows.Forms;
using SharpPcap;
using PacketDotNet;
using Kamael.Packets.Clan;
using System.Threading.Tasks;
using Kamael;

namespace Kamael.UI
{
    public partial class frmMain : Form
    {
        public ICaptureDevice device { get; set; }
        public frmMain()
        {
            InitializeComponent();
            device = ConfigureDevice();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var packetService = new L2RPacketService();
            Task.Run(async () => await packetService.StartAsync(ConfigureDevice()));
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


        private ICaptureDevice ConfigureDevice()
        {
            /* Retrieve the device list  part of initialization*/
            CaptureDeviceList devices = CaptureDeviceList.Instance;
            var iface = L2RPacketService.Initialization(Globals.args);
            ICaptureDevice device = CaptureDeviceList.Instance[1];
            //Register our handler function to the 'packet arrival' event
            device.OnPacketArrival +=
                new PacketArrivalEventHandler(PacketCapturer);
            return device;
        }


        public static async void PacketCapturer(object sender, CaptureEventArgs e)
        {
            DateTime time = e.Packet.Timeval.Date;
            int len = e.Packet.Data.Length;
            IL2RPacket l2rPacket = null;

            Packet packet = PacketDotNet.Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);

            TcpPacket tcpPacket = (PacketDotNet.TcpPacket)packet.Extract(typeof(PacketDotNet.TcpPacket));
            if (tcpPacket != null)
            {
                IPPacket ipPacket = (IPPacket)tcpPacket.ParentPacket;
                System.Net.IPAddress srcIp = ipPacket.SourceAddress;
                System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
                int srcPort = tcpPacket.SourcePort;
                int dstPort = tcpPacket.DestinationPort;
                byte[] payloadData = tcpPacket.PayloadData;

                Console.WriteLine("{0}:{1}:{2}.{3}\tLen={4}\t{5}:{6} -> {7}:{8}",
                time.Hour, time.Minute, time.Second, time.Millisecond, len,
                srcIp, srcPort, dstIp, dstPort);

                // Decrypt and process incoming packets
                if (srcPort == 12000)
                {
                    l2rPacket = L2RPacketService.AppendIncomingData(payloadData);

                    if (l2rPacket is PacketClanMemberKillNotify)
                    {
                        //Task.Run(async () => await NotifyKill((PacketClanMemberKillNotify)l2rPacket));
                    }
                }
            }
        }
    }
}