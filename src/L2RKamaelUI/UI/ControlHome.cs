using Kamael.Packets;
using Kamael.Packets.Character;
using Kamael.Packets.Status;
using L2RKamaelUI.UI;
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


            foreach (var dev in CaptureDeviceList.Instance)
            {
                var str = String.Format("{0} {1}", dev.Name, dev.Description);
                comboBox1.Items.Add(str);
            }
        }

        private void timerPackets_Tick(object sender, EventArgs e)
        {
            try
            {
                FormMain formMain = (FormMain)this.ParentForm;
                List<IL2RPacket> tmpPackets = formMain._Packets;

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