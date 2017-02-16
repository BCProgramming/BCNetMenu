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
            Connection_VPN=1,
            Connection_Wireless=2
        }

        private ConnectionDisplayType _ConnectionTypes = ConnectionDisplayType.Connection_Wireless| ConnectionDisplayType.Connection_VPN;
        public ConnectionDisplayType ShowConnectionTypes
        {
            get
            {
                return _ConnectionTypes;
            }
            set { _ConnectionTypes = value; }
        }

        private Font _WifiFont = SystemFonts.MenuFont;

        private Font _VPNFont = SystemFonts.MenuFont;

        private Font _NetMenuItemsFont = SystemFonts.MenuFont;

        //these all default to SystemFonts.MessageBoxFont

        private String _MenuRenderer = "Office 2007";

        public String MenuRenderer {  get { return _MenuRenderer; } set { _MenuRenderer = value; } }
        
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
        public static String GetDefaultSettingsFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BASeCamp", "BCNetMenu", "BCNetMenu.config");
            

        }

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
                XElement WifiFontNode = RootElement.Element("WifiFont");
                XElement VPNFontNode = RootElement.Element("VPNFont");
                XElement NetMenuFontNode = RootElement.Element("NetMenuFont");

                if (WifiFontNode != null) WifiFont = StandardHelper.ReadElement<Font>(WifiFontNode, null);
                if (VPNFontNode != null) VPNFont = StandardHelper.ReadElement<Font>(VPNFontNode, null);
                if (NetMenuFontNode != null) NetMenuItemsFont = StandardHelper.ReadElement<Font>(NetMenuFontNode, null);
            }
        }
      
        public void Save(String sXMLFile)
        {
            XElement rootnode = new XElement("Settings", new XAttribute("ShowConnections", (int)_ConnectionTypes), new XAttribute("Renderer", _MenuRenderer));
            XDocument doc = new XDocument(rootnode);

            rootnode.Add(StandardHelper.Static.SerializeObject(_WifiFont, "WifiFont", null));
            rootnode.Add(StandardHelper.Static.SerializeObject(_WifiFont, "VPNFont", null));
            rootnode.Add(StandardHelper.Static.SerializeObject(_WifiFont, "NetMenuFont", null));
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
