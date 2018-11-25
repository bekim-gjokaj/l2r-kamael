using Kamael.Packets;
using Kamael.Packets.Character;
using Kamael.Packets.Clan;
using Kamael.Packets.Status;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static Kamael.Packets.L2RPacketService;

namespace L2RKamaelUI
{
    public partial class ControlPackets : UserControl
    {
        private L2RPacketService _L2RPacketLogger = new L2RPacketService(0);
        private List<IL2RPacket> _Packets = new List<IL2RPacket>();

        public ControlPackets()
        {
            InitializeComponent();
        }

        private void frmHome_Load(object sender, EventArgs e)
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

        private void timerPackets_Tick(object sender, EventArgs e)
        {
            try
            {
                List<IL2RPacket> tmpPackets = _Packets;

                //tmpPackets.Reverse();
                s.Text = "";

                foreach (IL2RPacket pkt in tmpPackets)
                {
                    s.Text += pkt.GetType().ToString() + "\r\n";
                    if(pkt is PacketClanInfoReadResult)
                    {

                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        
    }
}