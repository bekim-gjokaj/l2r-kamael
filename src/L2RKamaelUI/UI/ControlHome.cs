using Kamael.Packets;
using Kamael.Packets.Character;
using Kamael.Packets.Status;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static Kamael.Packets.L2RPacketService;

namespace L2RKamaelUI
{
    public partial class ControlHome : UserControl
    {
        private L2RPacketService _L2RPacketLogger = new L2RPacketService(0);
        private List<IL2RPacket> _Packets = new List<IL2RPacket>();

        public ControlHome()
        {
            InitializeComponent();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chart1.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            _L2RPacketLogger.L2RPacketArrivalEvent += OnL2RPacketArrival;
            _L2RPacketLogger.StartCapture();


            foreach (var dev in CaptureDeviceList.Instance)
            {
                var str = String.Format("{0} {1}", dev.Name, dev.Description);
                comboBox1.Items.Add(str);
            }
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
                txtConsole.Text = "";

                foreach (IL2RPacket pkt in tmpPackets)
                {
                    txtConsole.Text += pkt.GetType().ToString() + "\r\n";

                    if (pkt is PacketSightEnterNotify)
                    {
                        UpdateMap(pkt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void UpdateMap(IL2RPacket pkt)
        {
            PacketSightEnterNotify pktSightEnter = (PacketSightEnterNotify)pkt;
            foreach (PacketOtherPlayer player in pktSightEnter.Players)
            {
                if (chart1.Series.FindByName(player.PlayerName) == null)
                {
                    chart1.Series.Add(player.PlayerName);
                    chart1.Series[player.PlayerName].ChartType = SeriesChartType.Point;
                    chart1.Series[player.PlayerName].Points.AddXY(player.XPos, player.YPos);
                    chart1.Series[player.PlayerName].ChartArea = "ChartArea1";
                }
                else
                {
                    foreach (DataPoint point in chart1.Series[player.PlayerName].Points)
                    {
                        point.XValue = player.XPos;
                        point.YValues[0] = player.YPos;
                    }
                }
            }
        }
    }
}