using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCNetMenu
{
    static class Program
    {
        public enum SignalDisplayType
        {
            Signal_Bars
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var MainMenu = new frmNetMenu();
            MainMenu.Visible = false;
            Application.Run(MainMenu);
        }
    }
}