using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace L2RKamaelUI.UI
{
    public partial class FormMain : Form
    {
        private ControlHome _Home = new ControlHome();
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
    }
}
