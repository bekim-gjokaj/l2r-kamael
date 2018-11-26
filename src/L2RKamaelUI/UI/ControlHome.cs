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

            foreach (ICaptureDevice dev in CaptureDeviceList.Instance)
            {
                string str = string.Format("{0} {1}", dev.Name, dev.Description);
                comboBox1.Items.Add(str);
            }
        }

        private void timerPackets_Tick(object sender, EventArgs e)
        {
            try
            {
                FormMain formMain = (FormMain)ParentForm;
                List<L2RPacketArrivalEventArgs> tmpPackets = formMain._Packets;

                //tmpPackets.Reverse();
                //txtConsole.Text = "";

                foreach (L2RPacketArrivalEventArgs pkt in tmpPackets)
                {
                    //txtConsole.Text += pkt.Packet.GetType().ToString() + "\r\n";

                    if (pkt.Packet is PacketSightEnterNotify)
                    {
                        UpdateMapSightEnter(pkt.Packet);
                    }
                    else if (pkt.Packet is PacketStatusMovement)
                    {
                        UpdateMapMoveNotice(pkt.Packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void UpdateMapSightEnter(IL2RPacket pkt)
        {
            PacketSightEnterNotify pktSightEnter = (PacketSightEnterNotify)pkt;
            foreach (PacketOtherPlayer player in pktSightEnter.Players)
            {
                if (chart1.Series.FindByName(player.PlayerUID.ToString()) == null)
                {
                    chart1.Series.Add(player.PlayerUID.ToString());
                    chart1.Series[player.PlayerUID.ToString()].ChartType = SeriesChartType.Point;
                    chart1.Series[player.PlayerUID.ToString()].Points.AddXY(player.XPos, player.YPos);
                    chart1.Series[player.PlayerUID.ToString()].ChartArea = "ChartArea1";
                    chart1.Series[player.PlayerUID.ToString()].LegendText = player.PlayerName;
                }
                else
                {
                    foreach (DataPoint point in chart1.Series[player.PlayerUID.ToString()].Points)
                    {
                        point.XValue = player.XPos;
                        point.YValues[0] = player.YPos;
                    }
                }
            }
        }

        private void UpdateMapMoveNotice(IL2RPacket pkt)
        {
            try
            {

                PacketStatusMovement pktStatMove = (PacketStatusMovement)pkt;

                if (chart1.Series.FindByName(pktStatMove.playerId.ToString()) == null)
                {
                    decimal xpos = 0, ypos = 0;
                    decimal.TryParse(pktStatMove.playerDestXpos.ToString(), out xpos);
                    decimal.TryParse(pktStatMove.playerDestYpos.ToString(), out ypos);
                    

                    if (xpos != 0 && ypos != 0)
                    {
                        chart1.Series.Add(pktStatMove.playerId.ToString());
                        chart1.Series[pktStatMove.playerId.ToString()].ChartType = SeriesChartType.Point;
                        chart1.Series[pktStatMove.playerId.ToString()].Points.AddXY(xpos, ypos);
                        chart1.Series[pktStatMove.playerId.ToString()].ChartArea = "ChartArea1"; 
                    }
                }
                else
                {
                    foreach (DataPoint point in chart1.Series[pktStatMove.playerId.ToString()].Points)
                    {
                        decimal xpos, ypos;
                        decimal.TryParse(pktStatMove.playerDestXpos.ToString(), out xpos);
                        decimal.TryParse(pktStatMove.playerDestYpos.ToString(), out ypos);



                        if (xpos != 0 && ypos != 0)
                        {
                            point.XValue = pktStatMove.playerDestXpos;
                            point.YValues[0] = pktStatMove.playerDestYpos;
                        }
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