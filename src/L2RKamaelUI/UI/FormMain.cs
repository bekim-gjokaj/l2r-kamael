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
        private ControlHunt _Hunt = new ControlHunt();
        private ControlFight _Fight = new ControlFight();
        private ControlQuests _Quests = new ControlQuests();
        private ControlRifts _Rifts = new ControlRifts();
        private ControlTools _Tools = new ControlTools();
        private ControlPackets _Packet = new ControlPackets();
        private ControlSettings _Settings = new ControlSettings();

        private L2RPacketService _L2RPacketLogger = new L2RPacketService(0);
        public List<L2RPacketArrivalEventArgs> _Packets = new List<L2RPacketArrivalEventArgs>();

        public FormMain()
        {
            InitializeComponent();
            _L2RPacketLogger.L2RPacketArrivalEvent += OnL2RPacketArrival;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //Initialize Main Panel
            PanelMain.Controls.Clear();
            PanelMain.Controls.Add(_Home);

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            _L2RPacketLogger.StopCapture();
        }

        private void SetButtonFocus (Button btn, string NewTitle = "")
        {
            btnHome.BackColor = Color.FromArgb(64, 64, 64);
            btnHunt.BackColor = Color.FromArgb(64, 64, 64);
            btnFight.BackColor = Color.FromArgb(64, 64, 64);
            btnQuests.BackColor = Color.FromArgb(64, 64, 64);
            btnRifts.BackColor = Color.FromArgb(64, 64, 64);
            btnTools.BackColor = Color.FromArgb(64, 64, 64);
            btnPackets.BackColor = Color.FromArgb(64, 64, 64);
            btnSettings.BackColor = Color.FromArgb(64, 64, 64);


            btn.BackColor = Color.FromArgb(192, 0, 192);
            pnlHighlight.Top = btn.Top;

            if(btn.Text != NewTitle && btn.Text != "")
            {
                btn.Text = NewTitle;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if(btnHome.Text.ToUpper() == "START")
            {
                SetButtonFocus(btnHome, "STOP");
                _L2RPacketLogger.StartCapture();

            }
            else
            {
                SetButtonFocus(btnHome, "START");
                _L2RPacketLogger.StopCapture();

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
                    _Packets.Add(e);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Process packet: " + ex.ToString());
            }
        }

        private void btnPackets_Click(object sender, EventArgs e)
        {


            //Initialize Main Panel
            PanelMain.Controls.Clear();
            PanelMain.Controls.Add(_Packet);

            SetButtonFocus(btnPackets, "Packets");

        }

        private void btnFight_Click(object sender, EventArgs e)
        {


            //Initialize Main Panel
            PanelMain.Controls.Clear();
            PanelMain.Controls.Add(_Packet);

            SetButtonFocus(btnFight, "Fight");
        }

        private void btnQuests_Click(object sender, EventArgs e)
        {


            //Initialize Main Panel
            PanelMain.Controls.Clear();
            PanelMain.Controls.Add(_Packet);

            SetButtonFocus(btnQuests, "Quests");
        }

        private void btnHunt_Click(object sender, EventArgs e)
        {

            //Initialize Main Panel
            PanelMain.Controls.Clear();
            PanelMain.Controls.Add(_Packet);

            SetButtonFocus(btnHunt, "Hunt");

        }

        private void btnRifts_Click(object sender, EventArgs e)
        {

            //Initialize Main Panel
            PanelMain.Controls.Clear();
            PanelMain.Controls.Add(_Packet);

            SetButtonFocus(btnRifts, "Rifts");

        }

        private void btnTools_Click(object sender, EventArgs e)
        {

            //Initialize Main Panel
            PanelMain.Controls.Clear();
            PanelMain.Controls.Add(_Packet);

            SetButtonFocus(btnTools, "Tools");

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {

            //Initialize Main Panel
            PanelMain.Controls.Clear();
            PanelMain.Controls.Add(_Packet);

            SetButtonFocus(btnSettings, "Settings");

        }
    }
}
