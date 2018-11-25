using Kamael.Packets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Kamael.Packets.L2RPacketService;

namespace L2RKamaelUI.UI
{
    public partial class FormMain : Form
    {
        private ControlHome _Home = new ControlHome();
        private ControlPackets _Packet = new ControlPackets();
        private L2RPacketService _L2RPacketLogger = new L2RPacketService(0);
        public List<IL2RPacket> _Packets = new List<IL2RPacket>();
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //Initialize Main Panel
            PanelMain.Controls.Clear();
            PanelMain.Controls.Add(_Home);

        }

        private void btnHome_Click(object sender, EventArgs e)
        {

            _L2RPacketLogger.L2RPacketArrivalEvent += OnL2RPacketArrival;
            _L2RPacketLogger.StartCapture();
        }


        private void OnL2RPacketArrival(object sender, L2RPacketArrivalEventArgs e)
        {
            try
            {
                //L2RPacketService proceesses the incoming payload and translates it to a concrete class
                IL2RPacket l2rPacket = e.Packet;
                if (l2rPacket != null)
                {
                    _Packets.Add(l2rPacket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Process packet: " + ex.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Initialize Main Panel
            PanelMain.Controls.Clear();
            PanelMain.Controls.Add(_Packet);
        }
    }
}
