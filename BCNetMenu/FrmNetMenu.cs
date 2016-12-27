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
using Microsoft.Win32;
using SimpleWifi;
using SimpleWifi.Win32;
using SimpleWifi.Win32.Interop;
using Office2007Rendering;

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
            cboMenuAppearance.Items.AddRange(new String[] { "System", "Professional", "Office 2007" });
            this.Icon = Resources.network_computer;
            nIcon = new NotifyIcon();
            nIcon.Icon = Resources.network_computer;
            nIcon.Text = "BASeCamp Network Menu";
            nIcon.Click += NIcon_Click;
            IconMenu = new ContextMenuStrip();
            IconMenu.Items.Add(new ToolStripMenuItem("GHOST"));
            nIcon.ContextMenuStrip = IconMenu;
            IconMenu.Opening += IconMenu_Opening;
            LoadedSettings = new NetMenuSettings();
            IconMenu.Renderer = GetConfiguredToolStripRenderer();
            nIcon.Visible = true;
            Visible = false; //Form should be invisible. This form will likely become the settings menu as well, but we'll add an option for that in the context menu when we need it.
            Hide();
        }
        private ToolStripRenderer GetConfiguredToolStripRenderer()
        {
            
            if(LoadedSettings.MenuRenderer.Equals("Professional",StringComparison.OrdinalIgnoreCase))
                return new ToolStripProfessionalRenderer();
            else if(LoadedSettings.MenuRenderer.Equals("System",StringComparison.OrdinalIgnoreCase))
                return new ToolStripSystemRenderer();
            else 
                return new Office2007Renderer();


        }
        
        private int FontSize = 14;
        private void IconMenu_Opening(object sender, CancelEventArgs e)
        {
            IconMenu.Renderer = GetConfiguredToolStripRenderer();
            IconMenu.Items.Clear();
            IconMenu.Font = new Font(IconMenu.Font.FontFamily, FontSize, IconMenu.Font.Style);
            IconMenu.ImageScalingSize = new Size(64,64);
            var standardconnections = NetworkConnectionInfo.GetConnections().ToList();
            if (standardconnections.Count == 0)
            {
                ToolStripMenuItem tsm = new ToolStripMenuItem("<<No Configured VPN Connections>>");
                tsm.Enabled = false;
                IconMenu.Items.Add(tsm);
            }
            else
            {
                foreach (var stdcon in standardconnections)
                {
                    ToolStripMenuItem tsm = new ToolStripMenuItem(stdcon.Name);
                    tsm.Checked = stdcon.Connected;

                    var grabIcon = ((Icon)Resources.ResourceManager.GetObject("server_network"));
                    tsm.Image = new Icon(grabIcon, 64, 64).ToBitmap();
                    if (stdcon.Connected)
                    {
                        tsm.Click += vpndisconnect_Click;
                    }
                    else
                    {
                        tsm.Click += vpnconnect_Click;
                    }
                    //tsm.Font = new Font(tsm.Font.FontFamily,FontSize,tsm.Font.Style);
                    tsm.Tag = stdcon;
                    IconMenu.Items.Add(tsm);
                }
            }
            IconMenu.Items.Add(new ToolStripSeparator());
            
            var wirelessconnections = NetworkConnectionInfo.GetWirelessConnections().ToList();
            if (wirelessconnections.Count == 0)
            {
                ToolStripMenuItem tsm = new ToolStripMenuItem("<<No Available Access Points>>");
                tsm.Enabled = false;
                IconMenu.Items.Add(tsm);
            }
            foreach (var wirelesscon in wirelessconnections)
            {
                
                //if (wirelesscon.Name.Trim().Length > 0)
                {
                    ToolStripMenuItem tsm = new ToolStripMenuItem(wirelesscon.Name==""?"<unknown>":wirelesscon.Name);
                    tsm.Checked = wirelesscon.Connected;
                    //tsm.Font = new Font(tsm.Font.FontFamily, FontSize, tsm.Font.Style);

                    var grabIcon = ((Icon) Resources.ResourceManager.GetObject(GetSignal((int) wirelesscon.APInfo.SignalStrength)));
                    tsm.Image = new Icon(grabIcon, 64, 64).ToBitmap();

                    tsm.Tag = wirelesscon;
                    IconMenu.Items.Add(tsm);
                    tsm.Click += WirelessClick;
                }
            }
            IconMenu.Items.Add(new ToolStripSeparator());
            ToolStripMenuItem SettingsItem = new ToolStripMenuItem("Settings...");
            SettingsItem.Click += SettingsItem_Click;
            IconMenu.Items.Add(SettingsItem);
            ToolStripMenuItem ExitItem = new ToolStripMenuItem("Exit");
            ExitItem.Click += ExitItem_Click;
            IconMenu.Items.Add(ExitItem);
        }

        private void SettingsItem_Click(object sender, EventArgs e)
        {
            chkAutoStart.Checked = IsStartupRegistered();
            radVPN.Checked = radWireless.Checked = radBoth.Checked = false;
            int Converted = (int)LoadedSettings.ShowConnectionTypes;
            switch (Converted)
            {
                case (int)NetMenuSettings.ConnectionDisplayType.Connection_VPN:
                    radVPN.Checked = true;
                    break;
                case (int)NetMenuSettings.ConnectionDisplayType.Connection_Wireless:
                    radWireless.Checked = true;
                    break;
                default:
                    radBoth.Checked = true;
                    break;
            }
            cboMenuAppearance.SelectedItem = LoadedSettings.MenuRenderer;
            ShowSettings = true;
            this.Show();
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            nIcon.Dispose();
            Application.Exit();
        }

        private String[] SignalIcons = new String[] { "signal_0", "signal_1", "signal_2", "signal_3", "signal_4", "signal_5" };
        private string GetSignal(int Percent)
        {
            if (Percent == 0) return "signal_0";
            if (Percent <= 20) return "signal_1";
            if (Percent <= 40) return "signal_2";
            if (Percent <= 60) return "signal_3";
            if (Percent <= 80) return "signal_4";
            if (Percent <= 100) return "signal_5";

            return "signal_5";

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

        private bool ShowSettings = false;
        private NetMenuSettings LoadedSettings;

        private void RegisterStartup(bool Startup)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
           ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (Startup)
            {
                registryKey.SetValue("BCNetMenu", Application.ExecutablePath);
            }
            else if (registryKey.GetValueNames().Contains("BCNetMenu"))
            {
                registryKey.DeleteValue("BCNetMenu");
            }
        }

        private bool IsStartupRegistered()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
          ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            return (String) registryKey.GetValue("BCNetMenu", "") == Application.ExecutablePath;

        }

        private void frmNetMenu_Shown(object sender, EventArgs e)
        {
            if (!ShowSettings) Visible = false;
            else
            {
            
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                RegisterStartup(chkAutoStart.Checked);

            if (radVPN.Checked)
                LoadedSettings.ShowConnectionTypes = NetMenuSettings.ConnectionDisplayType.Connection_VPN;
            else if (radWireless.Checked)
                LoadedSettings.ShowConnectionTypes = NetMenuSettings.ConnectionDisplayType.Connection_Wireless;
            else if(radBoth.Checked)
                LoadedSettings.ShowConnectionTypes = NetMenuSettings.ConnectionDisplayType.Connection_VPN | NetMenuSettings.ConnectionDisplayType.Connection_Wireless;

            LoadedSettings.MenuRenderer = (String)cboMenuAppearance.SelectedItem;
            LoadedSettings.Save();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
    public class NetworkConnectionInfo
    {
        public String Name;
        public bool Connected;
        public AccessPoint APInfo;
        public WlanProfileInfo Profile;
        public NetworkInterface NetInfo;
        public NetworkConnectionInfo(String pName,bool pConnected, AccessPoint pAPInfo,NetworkInterface pnetinfo)
        {
            Name = pName;
            Connected = pConnected;
            APInfo = pAPInfo;

            NetInfo = pnetinfo;
        }

        private static String GetStringForSSID(Dot11Ssid ssid)
        {
            return Encoding.ASCII.GetString(ssid.SSID, 0, (int) ssid.SSIDLength);
        }
        public static IEnumerable<NetworkConnectionInfo> GetWirelessConnections()
        {
            
            WlanClient client = new WlanClient();
            foreach (WlanInterface wlaninterface in client.Interfaces)
            {
                Dictionary<String, WlanProfileInfo> ProfileData = new Dictionary<string, WlanProfileInfo>();

                foreach (var prof in wlaninterface.GetProfiles())
                {
                    String sName = prof.profileName;
                    ProfileData.Add(sName,prof);
                }
                var networks = wlaninterface.GetAvailableNetworkList(0);
                foreach (WlanAvailableNetwork network in networks)
                {
                    WlanProfileInfo prof;
                    if (ProfileData.ContainsKey(network.profileName))
                    {
                        prof = ProfileData[network.profileName];

                    }
                    
                }
            }

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
