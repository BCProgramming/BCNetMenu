using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCNetMenu
{
    class Win10MenuRenderer : ToolStripSystemRenderer
    {

        static class DWMNativeMethods
        {
            [DllImport("dwmapi.dll", EntryPoint = "#127")]
            internal static extern void DwmGetColorizationParameters(ref DWMCOLORIZATIONPARAMS parms);
        }

        public struct DWMCOLORIZATIONPARAMS
        {
            public uint ColorizationColor,
                ColorizationAfterglow,
                ColorizationColorBalance,
                ColorizationAfterglowBalance,
                ColorizationBlurBalance,
                ColorizationGlassReflectionIntensity,
                ColorizationOpaqueBlend;
        }
       
        protected Brush DarkBrush = new SolidBrush(Color.FromArgb(225, 50, 35, 10));
        protected Color AccentColor = Color.Orange;

        public Win10MenuRenderer(Color? pAccentColor = null)
        {
            if(pAccentColor != null)
            {
                AccentColor = pAccentColor.Value;
            }
            else
            {
                AccentColor = GetWindowColorizationColor(true);
            }
        }
        private static Color GetWindowColorizationColor(bool opaque)
        {
            DWMCOLORIZATIONPARAMS parms = new DWMCOLORIZATIONPARAMS();
            DWMNativeMethods.DwmGetColorizationParameters(ref parms);

            //Color.FromArgb(parms.ColorizationColor);
            return Color.FromArgb(
                (byte)(opaque ? 255 : parms.ColorizationColor >> 24), 
        (byte)(parms.ColorizationColor >> 16), 
        (byte)(parms.ColorizationColor >> 8), 
        (byte) parms.ColorizationColor
        );
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(DarkBrush,e.AffectedBounds);
            
            //base.OnRenderToolStripBackground(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            Rectangle useBounds = new Rectangle(0, 0, e.Item.Bounds.Width, e.Item.Bounds.Height);
            e.Graphics.FillRectangle(DarkBrush, useBounds);
            e.Graphics.DrawLine(new Pen(Color.White, 2),useBounds.Left+5,useBounds.Top+useBounds.Width/2,useBounds.Right-5, useBounds.Top + useBounds.Width / 2);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle useBounds = new Rectangle(0, 0, e.Item.Bounds.Width, e.Item.Bounds.Height);
            Brush useBrush = DarkBrush;
            if(e.Item.Selected)
            {
                Color Light1 = Color.FromArgb(128, Color.DarkGray.R, Color.DarkGray.G, Color.DarkGray.B);
                Color Light2 = Color.FromArgb(150, Color.DarkGray.R, Color.DarkGray.G, Color.DarkGray.B);
                useBrush = new LinearGradientBrush(useBounds, Light1,Light2, LinearGradientMode.Vertical);
            }
            using (Brush AccentBrush = new SolidBrush(AccentColor))
            {
                e.Graphics.FillRectangle(AccentBrush,useBounds);
                e.Graphics.FillRectangle(useBrush, useBounds);
            }
            if(e.Item.Selected) useBrush.Dispose();
            //base.OnRenderMenuItemBackground(e);
        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.Selected?Color.Black:Color.LightGray;
            
            base.OnRenderItemText(e);
        }
    }
    
}
