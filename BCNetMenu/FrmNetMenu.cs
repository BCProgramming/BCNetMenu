using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCNetMenu.Properties;
using SimpleWifi;

namespace BCNetMenu
{
    public partial class frmNetMenu : Form
    {
        NotifyIcon nIcon = null;
        public frmNetMenu()
        {
            InitializeComponent();
        }
        private ContextMenuStrip IconMenu = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            Visible = false; //Form should be invisible. This form will likely become the settings menu as well, but we'll add an option for that in the context menu when we need it.
            this.Icon = Resources.network_computer;
            nIcon = new NotifyIcon();
            nIcon.Icon = Resources.network_computer;
            nIcon.Text = "BASeCamp Network Menu";
            nIcon.Click += NIcon_Click;
            IconMenu = new ContextMenuStrip();
            IconMenu.Items.Add(new ToolStripMenuItem("GHOST"));
            nIcon.ContextMenuStrip = IconMenu;
            IconMenu.Opening += IconMenu_Opening;
            
            nIcon.Visible = true;
        }

        private void IconMenu_Opening(object sender, CancelEventArgs e)
        {
            IconMenu.Items.Clear();
            var standardconnections = NetworkConnectionInfo.GetConnections();
            foreach (var stdcon in standardconnections)
            {
                ToolStripMenuItem tsm = new ToolStripMenuItem(stdcon.Name);
                tsm.Checked = stdcon.Connected;

                if(stdcon.Connected)
                {
                    tsm.Click += vpndisconnect_Click;
                }
                else
                {
                    tsm.Click += vpnconnect_Click;
                }
                tsm.Tag = stdcon;
                IconMenu.Items.Add(tsm);
            }
            IconMenu.Items.Add(new ToolStripSeparator());
            var wirelessconnections = NetworkConnectionInfo.GetWirelessConnections();
            foreach (var wirelesscon in wirelessconnections)
            {
                ToolStripMenuItem tsm = new ToolStripMenuItem(wirelesscon.Name);
                tsm.Checked = wirelesscon.Connected;
                tsm.Tag = wirelesscon;
                IconMenu.Items.Add(tsm);
                tsm.Click += WirelessClick;
            }
        }
        private void WirelessClick(object sender,EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            NetworkConnectionInfo coninfo = item.Tag as NetworkConnectionInfo;
            if(coninfo.APInfo!=null)
            {
                Wifi wif = new Wifi();
                
                if(coninfo.APInfo.IsConnected)
                {
                    wif.Disconnect();
                }
                else
                {
                    coninfo.APInfo.Connect(new AuthRequest(coninfo.APInfo), false);
                }
            }
        }
        private void vpndisconnect_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            NetworkConnectionInfo coninfo = item.Tag as NetworkConnectionInfo;
            NetworkConnectionInfo.DisconnectVPN(coninfo.Name);
            //throw new NotImplementedException();
        }
        private void vpnconnect_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            NetworkConnectionInfo coninfo = item.Tag as NetworkConnectionInfo;
            NetworkConnectionInfo.ConnectVPN(coninfo.Name);
            //throw new NotImplementedException();
        }

        private void NIcon_Click(object sender, EventArgs e)
        {
            //Go through all Network interfaces and create a context menu out of them
            //check off connected network interfaces
            //show menu
            //throw new NotImplementedException();
        }
    }
    public class NetworkConnectionInfo
    {
        public String Name;
        public bool Connected;
        public AccessPoint APInfo;
        public NetworkInterface NetInfo;
        public NetworkConnectionInfo(String pName,bool pConnected,AccessPoint pAPInfo,NetworkInterface pnetinfo)
        {
            Name = pName;
            Connected = pConnected;
            APInfo = pAPInfo;
            NetInfo = pnetinfo;
        }
        public static IEnumerable<NetworkConnectionInfo> GetWirelessConnections()
        {
            SimpleWifi.Wifi wifi = new Wifi();
            var AccessPoints = wifi.GetAccessPoints();
            foreach(var AccessPoint in AccessPoints)
            {
                NetworkConnectionInfo nci = new NetworkConnectionInfo(AccessPoint.Name, AccessPoint.IsConnected, AccessPoint,null);
                yield return nci;
            }
        }
        
        public static IEnumerable<NetworkConnectionInfo> GetConnections()
        {

            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) +
                        @"\Microsoft\Network\Connections\Pbk\rasphone.pbk";
            List<String> ConfiguredVPNs = new List<string>();
            const string pattern = @"\[(.*?)\]";
            var matches = Regex.Matches(System.IO.File.ReadAllText(path), pattern);

            foreach (Match m in matches)
                ConfiguredVPNs.Add(m.Groups[1].Value);

            var AllInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

            foreach(var winterface in AllInterfaces)
            {
                
                if ((winterface.NetworkInterfaceType == NetworkInterfaceType.Ppp) && (winterface.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                {
                    NetworkConnectionInfo nci = new NetworkConnectionInfo(winterface.Name, winterface.OperationalStatus == OperationalStatus.Up, null, winterface);
                    if(ConfiguredVPNs.Contains(nci.Name)) ConfiguredVPNs.Remove(nci.Name);
                    yield return nci;
                }
            }
            foreach(var iterate in ConfiguredVPNs)
            {
                yield return new NetworkConnectionInfo(iterate,false,null,null);
            }
        }
        public static void ConnectVPN(String VPNName)
        {
            System.Diagnostics.Process.Start("rasphone.exe", "-d " + VPNName);
        }
        public static void DisconnectVPN(String VPNName)
        {
            System.Diagnostics.Process.Start("rasphone.exe", "-h " + VPNName);
        }
    }
}
