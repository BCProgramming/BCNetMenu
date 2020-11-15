using BASeCamp.Elementizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BCNetMenu
{
    public class NetMenuSettings
    {
        [Flags]
        public enum ConnectionDisplayType
        {
            Connection_VPN = 1,
            Connection_Wireless = 2
        }

        private ConnectionDisplayType _ConnectionTypes = ConnectionDisplayType.Connection_Wireless | ConnectionDisplayType.Connection_VPN;
        private bool _DisconnectionNotifications = true;
        private bool _ConnectionNotifications = true;
        private bool _DWMBlur;
        private String _MenuRenderer = "Office 2007";

        private Font _NetMenuItemsFont = SystemFonts.MenuFont;

        private Color _OverrideAccentColor = Color.Red;
        //these all default to SystemFonts.MessageBoxFont

        //accent color is used for the Windows 10 foldout style.
        private bool _UseSystemAccentColor;

        private Font _VPNFont = SystemFonts.MenuFont;

        private Font _WifiFont = SystemFonts.MenuFont;
        private const String DEFAULT_ICON = "network_computer";
        private String _IconSetting = DEFAULT_ICON;

        public bool ConnectionNotifications { get { return _ConnectionNotifications; }  set { _ConnectionNotifications = value; }}

        public bool DisconnectNotifications {  get { return _DisconnectionNotifications; } set { _DisconnectionNotifications = value; } }
        public NetMenuSettings() : this(GetDefaultSettingsFilePath())
        {
        }

        public NetMenuSettings(String sXMLFile)
        {
            if (!File.Exists(sXMLFile))
            {
                return;
            }
            XDocument doc;
            using (FileStream fs = new FileStream(sXMLFile, FileMode.Open))
            {
                doc = XDocument.Load(fs);
                XElement RootElement = doc.Root;
                String DisplayData = RootElement.Attribute("ShowConnections").Value;
                _ConnectionTypes = (ConnectionDisplayType) int.Parse(DisplayData);
                _MenuRenderer = RootElement.Attribute("Renderer") == null ? "System" : RootElement.Attribute("Renderer").Value;
                _UseSystemAccentColor = RootElement.GetAttributeBool("UseSystemAccent", true);
                _DWMBlur = RootElement.GetAttributeBool("DWMBlur", false);
                _IconSetting = RootElement.GetAttributeString("Icon", DEFAULT_ICON);
                _ConnectionNotifications = RootElement.GetAttributeBool("Notifications", true);
                XElement WifiFontNode = RootElement.Element("WifiFont");
                XElement VPNFontNode = RootElement.Element("VPNFont");
                XElement NetMenuFontNode = RootElement.Element("NetMenuFont");
                XElement AccentColorNode = RootElement.Element("OverrideAccentColor");

                if (WifiFontNode != null) WifiFont = StandardHelper.ReadElement<Font>(WifiFontNode, null);
                if (VPNFontNode != null) VPNFont = StandardHelper.ReadElement<Font>(VPNFontNode, null);
                if (NetMenuFontNode != null) NetMenuItemsFont = StandardHelper.ReadElement<Font>(NetMenuFontNode, null);
                if (AccentColorNode != null) OverrideAccentColor = StandardHelper.ReadElement<Color>(AccentColorNode, null);
            }
        }

        public ConnectionDisplayType ShowConnectionTypes
        {
            get { return _ConnectionTypes; }
            set { _ConnectionTypes = value; }
        }


        public bool UseSystemAccentColor
        {
            get { return _UseSystemAccentColor; }
            set { _UseSystemAccentColor = value; }
        }

        public Color OverrideAccentColor
        {
            get { return _OverrideAccentColor; }
            set { _OverrideAccentColor = value; }
        }

        public String MenuRenderer
        {
            get { return _MenuRenderer; }
            set { _MenuRenderer = value; }
        }

        public Font WifiFont
        {
            get { return _WifiFont; }
            set { _WifiFont = value; }
        }

        public Font VPNFont
        {
            get { return _VPNFont; }
            set { _VPNFont = value; }
        }

        public Font NetMenuItemsFont
        {
            get { return _NetMenuItemsFont; }
            set { _NetMenuItemsFont = value; }
        }
        public String IconSetting
        {
            get { return _IconSetting; }
            set { _IconSetting = value; }
        }
        public bool DWMBlur
        {
            get { return _DWMBlur; }
            set { _DWMBlur = value; }
        }

        public static String GetDefaultSettingsFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BASeCamp", "BCNetMenu", "BCNetMenu.config");
        }

        public void Save(String sXMLFile)
        {
            XElement rootnode = new XElement("Settings");
            XDocument doc = new XDocument(rootnode);
            rootnode.Add(new XAttribute("ShowConnections", (int) _ConnectionTypes));
            rootnode.Add(new XAttribute("Renderer", _MenuRenderer));
            rootnode.Add(new XAttribute("UseSystemAccent", _UseSystemAccentColor));
            rootnode.Add(new XAttribute("DWMBlur", _DWMBlur));
            rootnode.Add(new XAttribute("Notifications", _ConnectionNotifications));
            rootnode.Add(new XAttribute("Icon", _IconSetting));
            rootnode.Add(StandardHelper.Static.SerializeObject(_WifiFont, "WifiFont", null));
            rootnode.Add(StandardHelper.Static.SerializeObject(_VPNFont, "VPNFont", null));
            rootnode.Add(StandardHelper.Static.SerializeObject(_NetMenuItemsFont, "NetMenuFont", null));
            rootnode.Add(StandardHelper.Static.SerializeObject(_OverrideAccentColor, "OverrideAccentColor", null));
            
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(sXMLFile));
            }
            catch (Exception exx)
            {
            }
            using (FileStream fs = new FileStream(sXMLFile, FileMode.Create))
            {
                doc.Save(fs);
            }
        }

        public void Save()
        {
            Save(GetDefaultSettingsFilePath());
        }
    }
}