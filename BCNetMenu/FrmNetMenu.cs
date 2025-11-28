using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BCNetMenu.Properties;
using Microsoft.Win32;
using SimpleWifi;
using SimpleWifi.Win32;
using SimpleWifi.Win32.Interop;
using Office2007Rendering;
using Timer = System.Threading.Timer;

namespace BCNetMenu
{
    public partial class frmNetMenu : Form
    {
        //private bool _Blur = true;
        private Color _SelectedCustomAccentColor = Color.Red;

        private int FontSize = 14;
        private ContextMenuStrip IconMenu;
        bool IntensityRecurse;
        private NetMenuSettings LoadedSettings;
        NotifyIcon nIcon;

        private bool ShowSettings;

        private String[] SignalIcons = {"signal_0", "signal_1", "signal_2", "signal_3", "signal_4", "signal_5"};

        //Timer UpdateTipTimer;

        public frmNetMenu()
        {
            InitializeComponent();
        }

        public Color SelectedCustomAccentColor
        {
            get { return _SelectedCustomAccentColor; }
            set
            {
                _SelectedCustomAccentColor = value;
                RefreshButtonColor(cmdAccentColor, _SelectedCustomAccentColor);
                tBarIntensity.Value = _SelectedCustomAccentColor.A;
            }
        }

        private String GetFontDescription(Font Source)
        {
            String[] CurrentFontStyles = (from FontStyle f in Enum.GetValues(typeof(FontStyle)) where (Source.Style & f) == f select Enum.GetName(typeof(FontStyle), f)).ToArray();
            String StyleDesc = String.Join(",", CurrentFontStyles);

            return Source.SizeInPoints + " pt. " + Source.Name + " " + StyleDesc;
        }

        private void RefreshButtonColor(Button target, Color useColor)
        {
            target.ImageAlign = ContentAlignment.MiddleLeft;
            target.TextAlign = ContentAlignment.MiddleRight;
            //draw the image.

            Bitmap colorimage = new Bitmap(16, 16);
            Graphics canvas = Graphics.FromImage(colorimage);
            canvas.Clear(useColor);
            canvas.DrawRectangle(new Pen(Color.Black, 1), new Rectangle(0, 0, 15, 15));
            canvas.Dispose();
            target.Image = colorimage;
        }
        Dictionary<String,String> IconListing = new Dictionary<string, string>() {
            { "Flat Network Computer","network_computer" },
            { "XP Network Icon","XP_Network"},
            { "Dual Network Icon","Dual_Network"},
            { "Earth Icon","Network_Connections" },
            { "Vista Style Icon","Vista_Network" }

        };
        private Icon GetIcon(String sName)
        {
            if(File.Exists(sName))
            {
                try
                {
                    Icon LoadIcon = new Icon(sName);
                    return LoadIcon;
                }
                catch(Exception LoadException)
                {
                    
                }
            }
            
            object obj = Resources.ResourceManager.GetObject(sName, Resources.Culture);
            return ((System.Drawing.Icon)(obj));
            
        }
        
        private Icon GetIcon()
        {

            

            //get standard icon based on settings.
            //first of all, does the setting reflect an actual file?
            String sIcon = LoadedSettings.IconSetting;
            
            
            GetConnectionsInfo(out List<NetworkConnectionInfo> standardconnections, out List<NetworkConnectionInfo> wirelessconnections);
            if(!standardconnections.Any((s)=>s.Connected) && !wirelessconnections.Any((s)=>s.Connected))
            {
                //use disabled icon.
                sIcon = GetDisabledIcon(LoadedSettings.IconSetting);
                
            }

            Icon tryresource = (Icon)Resources.ResourceManager.GetObject(sIcon);
            if (tryresource != null) return tryresource;

            return GetIcon(sIcon);
        }
        private String GetDisabledIcon(String sStandard)
        {
            if(File.Exists(sStandard))
            {
                String sDisabled = Path.Combine(Path.GetDirectoryName(sStandard), Path.GetFileNameWithoutExtension(sStandard) + "_disabled" + Path.GetExtension(sStandard));
                if (File.Exists(sDisabled))
                    return sDisabled;
                else
                    return sStandard;
            }
            else
            {
                return sStandard + "_disabled";
            }
        }
        private void PopulateIconSplit(DropSplitButton target, Dictionary<String,String> TextLookup,NetMenuSettings Settings)
        {
            //First we determine which key is selected. We will search through both keys and values.
            var CurrentSetting = Settings.IconSetting;
            KeyValuePair<String, String> CurrentSettingkvp;
            if (TextLookup.ContainsKey(CurrentSetting))
            {
                CurrentSettingkvp = new KeyValuePair<string, string>(CurrentSetting, TextLookup[CurrentSetting]);
            }
            else if (File.Exists(CurrentSetting))
            {
                target.Image = GetIcon(CurrentSetting).ToBitmap();
                target.Text = Path.GetFileNameWithoutExtension(CurrentSetting);
                return;
            }
            else
            {
                int foundindex = -1;
                //might be a value.
                var searcharray = TextLookup.Values.ToArray();
                for (int i = 0; i < searcharray.Length; i++)
                {
                    if (searcharray[i].Equals(CurrentSetting, StringComparison.OrdinalIgnoreCase))
                    {
                        foundindex = i;
                        break;
                    }
                }
                if (foundindex > -1)
                {
                    CurrentSetting = searcharray[foundindex];
                    target.Image = GetIcon(CurrentSetting).ToBitmap();
                    target.Text = TextLookup.Keys.ToArray()[foundindex];
                }
            }
            ContextMenuStrip cms = new ContextMenuStrip();
            target.SplitMenuStrip = cms;
            target.ShowSplit = true;
            //fill the listing
            foreach (var iteratekvp in TextLookup)
            {
                String sAppDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                var gotico = GetIcon(iteratekvp.Value);
                if(gotico!=null)
                    {
                    ToolStripMenuItem tms = new ToolStripMenuItem(iteratekvp.Value, gotico.ToBitmap());
                    tms.Tag = iteratekvp;
                    tms.Click += (o, e) =>
                    {
                        Settings.IconSetting = ((KeyValuePair<String, String>)((ToolStripMenuItem)o).Tag).Value;
                        target.Image = ((ToolStripMenuItem)o).Image;
                    };
                    target.SplitMenuStrip.Items.Add(tms);
                }
                
            }
            

        }
        //private static readonly TimeSpan UpdateTimeDelay = new TimeSpan(0, 0, 0, 30);
        private void Form1_Load(object sender, EventArgs e)
        {
            cboMenuAppearance.Items.AddRange(new[] {"System", "Professional", "Office 2007", "Windows 10 Foldout"});
            Icon = Resources.network_computer;
            nIcon = new NotifyIcon();
            nIcon.Text = "BASeCamp Network Menu";
            nIcon.Click += NIcon_Click;
            IconMenu = new ContextMenuStrip();
            IconMenu.Items.Add(new ToolStripMenuItem("GHOST"));
            nIcon.ContextMenuStrip = IconMenu;
            //nIcon.MouseClick += NIcon_MouseClick;
            IconMenu.Opening += IconMenu_Opening;
            IconMenu.Opened += IconMenu_Opened;
            IconMenu.Closed += IconMenu_Closed;
            LoadedSettings = new NetMenuSettings();
            nIcon.Icon = GetIcon();
            IconMenu.Renderer = GetConfiguredToolStripRenderer();
            lblCurrentFont.Text = GetFontDescription(LoadedSettings.WifiFont);
            lblCurrentFont.Font = LoadedSettings.WifiFont;
            nIcon.Visible = true;
            Visible = false; //Form should be invisible. 
            Hide();
            nIcon.MouseUp += NIcon_MouseUp;
            nIcon.MouseMove += NIcon_MouseMove;
            chkDWMBlur.Checked = LoadedSettings.DWMBlur;
            chkSystemAccent.Checked = LoadedSettings.UseSystemAccentColor;
            SelectedCustomAccentColor = LoadedSettings.OverrideAccentColor;
            chkConnectionNotifications.Checked = LoadedSettings.ConnectionNotifications;
            //these controls are disabled if not using the win10 renderer style.
            UpdateAccentState();
            //UpdateTipTimer = new Timer(TipTimer, null, TimeSpan.Zero, UpdateTimeDelay);
            PopulateIconSplit(NotificationIconSplit,IconListing,LoadedSettings);
            RefreshNetworkData();
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;

        }

        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            this.RefreshNetworkData();
        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            //
        }

        private void IconMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            IconMenu.Items.Clear();
        }

        private void UpdateAccentState()
        {
            chkDWMBlur.Enabled = chkSystemAccent.Enabled = cmdAccentColor.Enabled = (IconMenu.Renderer is Win10MenuRenderer);
            //the accent color button is  only enabled if the checkbox is both unchecked and enabled.
            tBarIntensity.Enabled = cmdAccentColor.Enabled = !chkSystemAccent.Checked && chkSystemAccent.Enabled;
        }

        [DllImport("user32.dll")]
        private static extern bool IsMenu(IntPtr hMenu);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindow(IntPtr hWnd);

        private void IconMenu_Opened(object sender, EventArgs e)
        {
            //Now this one is a bit wacky, just an experiment.
            if (LoadedSettings.DWMBlur && IconMenu.Renderer is Win10MenuRenderer) //blur is only available with the Win10 renderer, as presumably it would look really bad otherwise.
            {
                Win10MenuRenderer.DWMNativeMethods.DWM_BLURBEHIND bb = new Win10MenuRenderer.DWMNativeMethods.DWM_BLURBEHIND(true);
                Win10MenuRenderer.DWMNativeMethods.EnableBlur(IconMenu.Handle);
            }
            else
            {
                Win10MenuRenderer.DWMNativeMethods.EnableBlur(IconMenu.Handle,false);
                }
            //throw new NotImplementedException();
        }

        private void NIcon_MouseMove(object sender, MouseEventArgs e)
        {
        }
        private void RefreshNetworkData()
        {
                        List<NetworkConnectionInfo> NewStandard = null, newWireless = null;
            GetConnectionsInfo(out NewStandard, out newWireless);
            NewStandard.RemoveAll((g) => !g.Connected);
            newWireless.RemoveAll((g) => !g.Connected);
            if(PreviousStandard!=null && PreviousWireless!=null)
            {
                
           

                if (this.LoadedSettings.DisconnectNotifications || this.LoadedSettings.LogConnectionChanges)
                {
                    //first find any new standard connections.
                    var ConnectedStandard = (from c in NewStandard where !PreviousStandard.Any((a)=>a.Name==c.Name) select c).ToList();
                    var DisconnectedStandard = (from c in PreviousStandard where  !NewStandard.Any((a) => a.Name == c.Name) select c).ToList();

                    //now evaluate wireless connections.
                    String sConnectionText = "";
                    String sDisconnectionText = "";
                    //we will only monitor for the connection types we are set to also display.
                    var ConnectedWireless = !LoadedSettings.ShowConnectionTypes.HasFlag(NetMenuSettings.ConnectionDisplayType.Connection_Wireless)?
                         new List<NetworkConnectionInfo>():
                         (from w in newWireless where !PreviousWireless.Any((a) => a.Name == w.Name) select w).ToList();
                    var DisconnectedWireless = !LoadedSettings.ShowConnectionTypes.HasFlag(NetMenuSettings.ConnectionDisplayType.Connection_VPN)?
                        new List<NetworkConnectionInfo>():
                        (from w in PreviousWireless where !newWireless.Any((a) => a.Name == w.Name) select w).ToList();
                    String buildNotificationText = "";
                    if ((DisconnectedStandard.Any() || DisconnectedWireless.Any()) )
                    {
                        //wireless or standard connection was disconnected. build a string for a description and show a notification regarding what disconnected.
                        sDisconnectionText =  "Disconnected: " + String.Join(",", from s in DisconnectedStandard.Concat(DisconnectedWireless) select s.Name);
                        
                    }
                    if((ConnectedWireless.Any() || ConnectedStandard.Any()) )
                    {

                        sConnectionText = Environment.NewLine + "Connected: " + String.Join(",", from s in ConnectedStandard.Concat(ConnectedWireless) select s.Name);
                        
                    }
                    buildNotificationText = String.Join("\n", String.Join(",", new[] { sDisconnectionText, sConnectionText }).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries));


                    if(!String.IsNullOrEmpty(buildNotificationText))
                    {
                        String TipTitle = "Connection Changes";
                        bool connectedChange = ConnectedWireless.Any() || ConnectedStandard.Any();
                        bool disconnectedChange = DisconnectedWireless.Any() || DisconnectedStandard.Any();
                        bool connectedOnly = !DisconnectedWireless.Any() && !DisconnectedStandard.Any();
                        bool disconnectedOnly = !ConnectedWireless.Any() && !ConnectedStandard.Any();
                        if (connectedOnly)
                            TipTitle = "New Connections";
                        else if (disconnectedOnly)
                            TipTitle = "Disconnections";
                        
                        
                        if((connectedChange && LoadedSettings.ConnectionNotifications) || (disconnectedChange && LoadedSettings.DisconnectNotifications))
                            nIcon.ShowBalloonTip(5000, TipTitle, buildNotificationText, ToolTipIcon.Info);

                        
                            //ShowBalloonTip(5000, "Disconnected", "Disconnected from VPN " + VPNName + "", ToolTipIcon.Info); CompleteAction?.Invoke();
                    }
                    if (LoadedSettings.LogConnectionChanges)
                        LogConnectionChange(ConnectedStandard, ConnectedWireless, DisconnectedStandard, DisconnectedWireless);

                }
            }
            PreviousStandard = NewStandard;
            PreviousWireless = newWireless;
        }
        
      
        private void UpdateToolTip()
        {
            String UpdatedTip = GetUpdatedTip();
            nIcon.Text =  UpdatedTip;
            nIcon.Icon = GetIcon();

        }
        FileStream LogFile = null;
        StreamWriter LogFileStream { get
            {
                if (LogFile == null || !LogFile.CanWrite)
                {
                    LogFile = new FileStream(LoadedSettings.LogFileLocation, FileMode.Append, FileAccess.Write);
                }
                    return new StreamWriter(LogFile);
                
            } }
        private void LogConnectionChange(IEnumerable<NetworkConnectionInfo> pStandardConnected, IEnumerable<NetworkConnectionInfo> pWirelessConnected, IEnumerable<NetworkConnectionInfo> pStandardDisconnected, IEnumerable<NetworkConnectionInfo> pWirelessDisconnected)
        {
            StringBuilder BuildAddition = new StringBuilder();
            foreach(var iterate in pStandardConnected)
            {
                BuildAddition.AppendLine(BuildTimeStamp() + " Connected:" + iterate.Name);
            }
            foreach(var iterate in pStandardDisconnected)
            {
                BuildAddition.AppendLine(BuildTimeStamp() + " Disconnected:" + iterate.Name);
            }
            foreach (var iterate in pWirelessConnected)
            {
                BuildAddition.AppendLine(BuildTimeStamp() + " Connected:" + iterate.Name);
            }
            foreach (var iterate in pWirelessDisconnected)
            {
                BuildAddition.AppendLine(BuildTimeStamp() + " Disconnected:" + iterate.Name);
            }
        }
        private String BuildTimeStamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffffff") + ">>";
        }
        private List<NetworkConnectionInfo> PreviousStandard = null, PreviousWireless = null;
        private void GetConnectionsInfo(out List<NetworkConnectionInfo> standardconnections,out List<NetworkConnectionInfo> wirelessconnections)
        {
            try
            {
                standardconnections = NetworkConnectionInfo.GetConnections().ToList();
            }
            catch (Exception exx)
            {
                standardconnections = new List<NetworkConnectionInfo>();
            }

            

            try
            {
                wirelessconnections = NetworkConnectionInfo.GetWirelessConnections().ToList();
            }
            catch (Exception exx)
            {
                wirelessconnections = new List<NetworkConnectionInfo>();
            }
        }

        private String GetUpdatedTip()
        {
            try
            {
                
                
                //retrieves the connected VPN and Wireless connections.
                GetConnectionsInfo(out List<NetworkConnectionInfo> standardconnections,out List<NetworkConnectionInfo> wirelessconnections);
                String[] ConnectedVPNs = (from c in standardconnections where c != null && c.Connected select c.Name).ToArray();
                String[] ConnectedWireless = (from c in wirelessconnections where c != null && c.Connected select c.Name).ToArray();
                String sResult = "Connected to " + String.Join(",", ConnectedVPNs) + "; " + String.Join(",", ConnectedWireless); 
                if(sResult.Length >=63)
                {
                    List<String> ConnectionInfo = new List<string>();
                    if (ConnectedVPNs.Length > 0) ConnectionInfo.Add(ConnectedVPNs.Length + " VPNs ");
                    if(ConnectedWireless.Length >0) ConnectionInfo.Add(ConnectedWireless.Length + " APs");
                    sResult =  "Connected:" + String.Join(";", ConnectionInfo);
                    
                }
                return sResult;
            }
            catch (Exception exx)
            {
                return "BASeCamp Network Menu";
            }
        }

        private void NIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(nIcon, null);
            }
        }


        private ToolStripRenderer GetConfiguredToolStripRenderer()
        {
            Color? useAccent = LoadedSettings.UseSystemAccentColor ? (Color?) null : LoadedSettings.OverrideAccentColor;
            if (LoadedSettings.MenuRenderer.Equals("Professional", StringComparison.OrdinalIgnoreCase))
                return new ToolStripProfessionalRenderer();
            if (LoadedSettings.MenuRenderer.Equals("System", StringComparison.OrdinalIgnoreCase))
                return new ToolStripSystemRenderer();
            if (LoadedSettings.MenuRenderer.Equals("Windows 10 Foldout", StringComparison.OrdinalIgnoreCase))
            {
                return new Win10MenuRenderer(useAccent, LoadedSettings.DWMBlur);
            }
            if (Environment.OSVersion.Version.Major >= 10)
            {
                return new Win10MenuRenderer(useAccent, LoadedSettings.DWMBlur);
            }
            return new Office2007Renderer();
        }
        private Bitmap Selectify(Image Source)
        {
            Color StartColor = Color.FromArgb(25, 200, 200, 200);
            Color EndColor = Color.FromArgb(50, 200, 200, 200);
            Bitmap result = new Bitmap(Source.Width,Source.Height);
            using (Graphics buildresult = Graphics.FromImage(result))
            {
                buildresult.Clear(Color.Transparent);
                buildresult.DrawImage(Source,new Point(0,0));
                LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(0, 0, Source.Width, Source.Height), StartColor, EndColor, LinearGradientMode.ForwardDiagonal);
                buildresult.DrawRectangle(new Pen(Color.White, 2), new Rectangle(0, 0, Source.Width, Source.Height));
                buildresult.FillRectangle(lgb, new Rectangle(0, 0, Source.Width, Source.Height));


            }
            return result;

        }
        Thread MenuPopulationThread = null;
        private void PopulateMenuThread(Object parameter)
        {
            //object parameter is the ToolStripMenu to populate.
            var useMenu = parameter as ContextMenuStrip;
            PopulateMenu(useMenu);
        }
        private void PopulateMenu(ContextMenuStrip Target)
        {

            
            List<NetworkConnectionInfo> standardconnections = null;
            try
            {
                standardconnections = NetworkConnectionInfo.GetConnections().ToList();
            }
            catch (Exception exx)
            {
                standardconnections = new List<NetworkConnectionInfo>();
            }
            if (standardconnections.Count == 0)
            {
                Invoke((MethodInvoker)(() =>
                {
                    ToolStripMenuItem tsm = new ToolStripMenuItem("<<No Configured VPN Connections>>");
                    tsm.Enabled = false;
                    Target.Items.Add(tsm);
                }));
            }
            else
            {
                foreach (var stdcon in standardconnections)
                {
                    Invoke((MethodInvoker)(() =>
                    {
                        ToolStripMenuItem tsm = new ToolStripMenuItem(stdcon.Name);
                        tsm.Checked = stdcon.Connected;

                        var grabIcon = ((Icon)Resources.ResourceManager.GetObject("server_network"));
                        var useImage = new Icon(grabIcon, 64, 64).ToBitmap();
                        if (tsm.Checked) useImage = Selectify(useImage);
                        tsm.Image = useImage;
                        if (stdcon.Connected)
                        {
                            tsm.Click += vpndisconnect_Click;
                        }
                        else
                        {
                            tsm.Click += vpnconnect_Click;
                        }
                        tsm.Font = LoadedSettings.VPNFont;
                        //tsm.Font = new Font(tsm.Font.FontFamily,FontSize,tsm.Font.Style);
                        tsm.Tag = stdcon;
                        Target.Items.Add(tsm);
                    }));
                }
            }
            Invoke((MethodInvoker)(()=>Target.Items.Add(new ToolStripSeparator())));
            List<NetworkConnectionInfo> wirelessconnections = null;
            try
            {
                wirelessconnections = NetworkConnectionInfo.GetWirelessConnections().ToList();
            }
            catch (Exception exx)
            {
                wirelessconnections = new List<NetworkConnectionInfo>();
            }
            if (wirelessconnections.Count == 0)
            {
                Invoke((MethodInvoker)(() =>
                {
                    ToolStripMenuItem tsm = new ToolStripMenuItem("<<No Available Access Points>>");
                    tsm.Enabled = false;
                    Target.Items.Add(tsm);
                }));
            }
            foreach (var wirelesscon in wirelessconnections)
            {
                //if (wirelesscon.Name.Trim().Length > 0)
                {
                    Invoke((MethodInvoker)(() =>
                    {
                        ToolStripMenuItem tsm = new ToolStripMenuItem(wirelesscon.Name == "" ? "<unknown>" : wirelesscon.Name);
                        tsm.Checked = wirelesscon.Connected;
                        //tsm.Font = new Font(tsm.Font.FontFamily, FontSize, tsm.Font.Style);

                        var grabIcon = ((Icon)Resources.ResourceManager.GetObject(GetSignal((int)wirelesscon.APInfo.SignalStrength)));
                        Image useImage = new Icon(grabIcon, 64, 64).ToBitmap();
                        if (tsm.Checked) useImage = Selectify(useImage);
                        tsm.Image = useImage;
                        tsm.Font = LoadedSettings.WifiFont;
                        tsm.Tag = wirelesscon;
                        IconMenu.Items.Add(tsm);
                        tsm.Click += WirelessClick;
                    }));
                }
            }
            Invoke((MethodInvoker)(() =>
            {
                Target.Items.Add(new ToolStripSeparator());
                ToolStripMenuItem SettingsItem = new ToolStripMenuItem("Settings...");
                SettingsItem.Click += SettingsItem_Click;

                Target.Items.Add(SettingsItem);
                ToolStripMenuItem ExitItem = new ToolStripMenuItem("Exit");
                SettingsItem.Font = ExitItem.Font = LoadedSettings.NetMenuItemsFont;
                ExitItem.Click += ExitItem_Click;
                Target.Items.Add(ExitItem);
            }));
        }
        private void IconMenu_Opening(object sender, CancelEventArgs e)
        {
            IconMenu.Renderer = GetConfiguredToolStripRenderer();
            IconMenu.Items.Clear();
            IconMenu.Font = new Font(IconMenu.Font.FontFamily, FontSize, IconMenu.Font.Style);
            IconMenu.ImageScalingSize = new Size(64, 64);
            IconMenu.Items.Clear();
            MenuPopulationThread = new Thread(PopulateMenuThread);
            MenuPopulationThread.Start(IconMenu);

        }


        private void SettingsItem_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            chkAutoStart.Checked = IsStartupRegistered();
            radVPN.Checked = radWireless.Checked = radBoth.Checked = false;
            int Converted = (int) LoadedSettings.ShowConnectionTypes;
            switch (Converted)
            {
                case (int) NetMenuSettings.ConnectionDisplayType.Connection_VPN:
                    radVPN.Checked = true;
                    break;
                case (int) NetMenuSettings.ConnectionDisplayType.Connection_Wireless:
                    radWireless.Checked = true;
                    break;
                default:
                    radBoth.Checked = true;
                    break;
            }
            cboMenuAppearance.SelectedItem = LoadedSettings.MenuRenderer;
            ShowSettings = true;
            Show();
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            nIcon.Dispose();
            Application.Exit();
        }

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

        private void WirelessClick(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            NetworkConnectionInfo coninfo = item.Tag as NetworkConnectionInfo;
            if (coninfo.APInfo != null)
            {
                Wifi wif = new Wifi();

                if (coninfo.APInfo.IsConnected)
                {
                    wif.Disconnect();
                }
                else
                {
                    coninfo.APInfo.Connect(new AuthRequest(coninfo.APInfo), false);
                }
            }
            nIcon.Icon = GetIcon();
        }

        private void vpndisconnect_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            NetworkConnectionInfo coninfo = item.Tag as NetworkConnectionInfo;
            NetworkConnectionInfo.DisconnectVPN(nIcon, coninfo.Name, false, () => { nIcon.Icon = GetIcon(); });
            
        }

        private void vpnconnect_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            NetworkConnectionInfo coninfo = item.Tag as NetworkConnectionInfo;
            NetworkConnectionInfo.ConnectVPN(nIcon, coninfo.Name,false,()=> { nIcon.Icon = GetIcon(); });
            //throw new NotImplementedException();
        }

        private void NIcon_Click(object sender, EventArgs e)
        {
        }

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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterStartup(chkAutoStart.Checked);

            if (radVPN.Checked)
                LoadedSettings.ShowConnectionTypes = NetMenuSettings.ConnectionDisplayType.Connection_VPN;
            else if (radWireless.Checked)
                LoadedSettings.ShowConnectionTypes = NetMenuSettings.ConnectionDisplayType.Connection_Wireless;
            else if (radBoth.Checked)
                LoadedSettings.ShowConnectionTypes = NetMenuSettings.ConnectionDisplayType.Connection_VPN | NetMenuSettings.ConnectionDisplayType.Connection_Wireless;

            LoadedSettings.MenuRenderer = (String) cboMenuAppearance.SelectedItem;
            LoadedSettings.WifiFont = LoadedSettings.VPNFont = LoadedSettings.NetMenuItemsFont = lblCurrentFont.Font;

            LoadedSettings.DWMBlur = chkDWMBlur.Checked;
            LoadedSettings.OverrideAccentColor = SelectedCustomAccentColor;
            LoadedSettings.UseSystemAccentColor = chkSystemAccent.Checked;
            LoadedSettings.ConnectionNotifications = chkConnectionNotifications.Checked;
            LoadedSettings.LogConnectionChanges = chkLogConnection.Checked;
            LoadedSettings.Save();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void frmNetMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                Hide();
                e.Cancel = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = lblCurrentFont.Font;
            fd.FontMustExist = true;
            fd.ShowColor = false;
            fd.ShowApply = false;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                lblCurrentFont.Font = fd.Font;
                lblCurrentFont.Text = GetFontDescription(fd.Font);
            }
        }

        private void chkDWMBlur_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAccentState();
        }

        private void chkSystemAccent_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAccentState();
        }

        private void cmdAccentColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                if (cd.ShowDialog(this) == DialogResult.OK)
                {
                    SelectedCustomAccentColor = cd.Color;
                }
            }
        }

        private void tBarIntensity_ValueChanged(object sender, EventArgs e)
        {
            if (IntensityRecurse) return;
            IntensityRecurse = true;
            try
            {
                int R = SelectedCustomAccentColor.R;
                int G = SelectedCustomAccentColor.G;
                int B = SelectedCustomAccentColor.B;
                Color makeColor = Color.FromArgb(tBarIntensity.Value, R, G, B);
                SelectedCustomAccentColor = makeColor;
            }
            finally
            {
                IntensityRecurse = false;
            }
        }

        private void btnOpenLog_Click(object sender, EventArgs e)
        {

        }

        private void frmNetMenu_MouseClick(object sender, MouseEventArgs e)
        {
           
        }

    

        public class NetworkConnectionInfo
        {
            public AccessPoint APInfo;
            public bool Connected;
            public String Name;
            public NetworkInterface NetInfo;
            public WlanProfileInfo Profile;

            public NetworkConnectionInfo(String pName, bool pConnected, AccessPoint pAPInfo, NetworkInterface pnetinfo)
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
                        ProfileData.Add(sName, prof);
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

                Wifi wifi = new Wifi();
                
                var AccessPoints = wifi.GetAccessPoints();

                foreach (var AccessPoint in AccessPoints)
                {
                    NetworkConnectionInfo nci = new NetworkConnectionInfo(AccessPoint.Name, AccessPoint.IsConnected, AccessPoint, null);
                    yield return nci;
                }
            }

            public static IEnumerable<NetworkConnectionInfo> GetConnections()
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                              @"\Microsoft\Network\Connections\Pbk\rasphone.pbk";
                List<String> ConfiguredVPNs = new List<string>();
                const string pattern = @"\[(.*?)\]";
                var matches = Regex.Matches(File.ReadAllText(path), pattern);

                foreach (Match m in matches)
                    ConfiguredVPNs.Add(m.Groups[1].Value);

                var AllInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (var winterface in AllInterfaces)
                {
                    if ((winterface.NetworkInterfaceType == NetworkInterfaceType.Ppp) && (winterface.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                    {
                        NetworkConnectionInfo nci = new NetworkConnectionInfo(winterface.Name, winterface.OperationalStatus == OperationalStatus.Up, null, winterface);
                        if (ConfiguredVPNs.Contains(nci.Name)) ConfiguredVPNs.Remove(nci.Name);
                        yield return nci;
                    }
                }
                foreach (var iterate in ConfiguredVPNs)
                {
                    yield return new NetworkConnectionInfo(iterate, false, null, null);
                }
            }
            public static bool ConnectionInProgress = false;
            public static void ConnectVPN(NotifyIcon ni, String VPNName, bool ShowNotification = true,Action CompleteAction = null)
            {
                ConnectionInProgress = true;
                try
                {
                    if (VPNName.Contains(" ")) VPNName = "\"" + VPNName + "\"";
                    ProcessStartInfo psi = new ProcessStartInfo("rasphone.exe", "-d " + VPNName);
                    Process p = Process.Start(psi);
                    p.EnableRaisingEvents = ShowNotification;
                    p.Exited += (o, s) =>
                    {

                        if (p.ExitCode == 0)
                        {
                            if(ShowNotification) ni.ShowBalloonTip(5000, "Connection Established", "Established connection to VPN " + VPNName + ".", ToolTipIcon.Info);
                            CompleteAction?.Invoke();
                        }
                        else
                        {
                            if (ShowNotification)
                                ni.ShowBalloonTip(5000, "Connection Failed", "Failed to Establish connection to VPN " + VPNName + ".", ToolTipIcon.Info);
                        }
                    };
                }
                finally
                {
                    ConnectionInProgress = false;
                }
                
            }


            public static void DisconnectVPN(NotifyIcon ni, String VPNName,bool ShowNotification = true, Action CompleteAction = null)
            {
                ConnectionInProgress = true;
                try
                {
                    if (VPNName.Contains(" ")) VPNName = "\"" + VPNName + "\"";
                    ProcessStartInfo psi = new ProcessStartInfo("rasphone.exe", "-h " + VPNName);
                    Process p = Process.Start(psi);
                    p.EnableRaisingEvents = ShowNotification;
                    p.Exited += (o, s) =>
                    {
                        if (p.ExitCode == 0)
                        {
                            if (ShowNotification)
                                ni.ShowBalloonTip(5000, "Disconnected", "Disconnected from VPN " + VPNName + "", ToolTipIcon.Info); CompleteAction?.Invoke();
                        }
                    };
                }
                finally
                {
                    ConnectionInProgress = false;
                }
            }
        }
    }
}