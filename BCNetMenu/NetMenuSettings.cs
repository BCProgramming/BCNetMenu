using System;
using System.Collections.Generic;
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
            }
        }

        public void Save(String sXMLFile)
        {
            XDocument doc = new XDocument(new XElement("Settings",new XAttribute("ShowConnections",(int)_ConnectionTypes)));
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
