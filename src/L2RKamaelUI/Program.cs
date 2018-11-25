using System;
using System.Windows.Forms;
using Kamael;
using L2RKamaelUI.UI;

namespace Kamael.UI
{
    

    internal static class Kamael
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}