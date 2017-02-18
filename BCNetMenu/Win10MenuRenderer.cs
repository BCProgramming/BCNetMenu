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
        protected bool _Blur = true;
        protected Color AccentColor = Color.Orange;

        protected Brush DarkBrush = new SolidBrush(Color.FromArgb(50, 50, 35, 10));

        public Win10MenuRenderer(Color? pAccentColor = null, bool pBlur = true)
        {
            _Blur = pBlur;
            if (pAccentColor != null)
            {
                AccentColor = pAccentColor.Value;
            }
            else
            {
                AccentColor = GetWindowColorizationColor(false);
                DarkBrush = new SolidBrush(Color.FromArgb(AccentColor.A / 2, 35, 35, 35));
            }
        }

        private static Color GetWindowColorizationColor(bool opaque)
        {
            DWMCOLORIZATIONPARAMS parms = new DWMCOLORIZATIONPARAMS();
            DWMNativeMethods.DwmGetColorizationParameters(ref parms);

            //Color.FromArgb(parms.ColorizationColor);
            return Color.FromArgb(
                (byte) (opaque ? 255 : (parms.ColorizationColor >> 24) / 2),
                (byte) (parms.ColorizationColor >> 16),
                (byte) (parms.ColorizationColor >> 8),
                (byte) parms.ColorizationColor
            );
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (_Blur)
            {
                //if set to use dwm blur, clear to transparent, paint some "darkening", then color it with the accent color.
                e.Graphics.Clear(Color.Transparent);
                e.Graphics.FillRectangle(DarkBrush, e.AffectedBounds);
                using (Brush AccentBrush = new SolidBrush(AccentColor))
                {
                    e.Graphics.FillRectangle(AccentBrush, e.AffectedBounds);
                }
            }
            else
            {
                e.Graphics.FillRectangle(DarkBrush, e.AffectedBounds);
                //base.OnRenderToolStripBackground(e);
            }
        }

        protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
        {
            if (!_Blur) base.OnRenderToolStripContentPanelBackground(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (!_Blur) base.OnRenderToolStripBorder(e);
            //e.Graphics.DrawRectangle(new Pen(Color.White,2),e.ToolStrip.Bounds);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            Rectangle useBounds = new Rectangle(0, 0, e.Item.Bounds.Width, e.Item.Bounds.Height);

            OnRenderMenuItemBackground(e);

            e.Graphics.DrawLine(new Pen(Color.White, 2), useBounds.Left + 25, useBounds.Top + useBounds.Height / 2, useBounds.Right - 25, useBounds.Top + useBounds.Height / 2);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle useBounds = new Rectangle(0, 0, e.Item.Bounds.Width, e.Item.Bounds.Height);
            Brush useBrush = null;
            if (e.Item.Selected)
            {
                Color Light1 = Color.FromArgb(128, Color.DarkGray.R, Color.DarkGray.G, Color.DarkGray.B);
                Color Light2 = Color.FromArgb(150, Color.DarkGray.R, Color.DarkGray.G, Color.DarkGray.B);
                useBrush = new LinearGradientBrush(useBounds, Light1, Light2, LinearGradientMode.Vertical);
            }
            if (useBrush != null) e.Graphics.FillRectangle(useBrush, useBounds);

            if (e.Item.Selected) useBrush.Dispose();
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.Selected ? Color.White : Color.LightGray;
            if (!e.Item.Enabled)
            {
                e.Item.Enabled = false;
                e.Item.ForeColor = Color.SlateBlue;
                base.OnRenderItemText(e);
                e.Item.Enabled = false;
            }
            else
            {
                base.OnRenderItemText(e);
            }
        }

        public static class DWMNativeMethods
        {
            [Flags]
            public enum DWM_BB
            {
                Enable = 1,
                BlurRegion = 2,
                TransitionMaximized = 4
            }

            [DllImport("user32.dll")]
            internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

            [DllImport("dwmapi.dll")]
            public static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);

            [DllImport("dwmapi.dll", EntryPoint = "#127")]
            internal static extern void DwmGetColorizationParameters(ref DWMCOLORIZATIONPARAMS parms);

            public static void EnableBlur(IntPtr WindowHandle)
            {
                var accent = new AccentPolicy();
                var accentStructSize = Marshal.SizeOf(accent);
                accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

                var accentPtr = Marshal.AllocHGlobal(accentStructSize);
                Marshal.StructureToPtr(accent, accentPtr, false);

                var data = new WindowCompositionAttributeData();
                data.Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY;
                data.SizeOfData = accentStructSize;
                data.Data = accentPtr;

                SetWindowCompositionAttribute(WindowHandle, ref data);

                Marshal.FreeHGlobal(accentPtr);
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct WindowCompositionAttributeData
            {
                public WindowCompositionAttribute Attribute;
                public IntPtr Data;
                public int SizeOfData;
            }

            internal enum WindowCompositionAttribute
            {
                WCA_ACCENT_POLICY = 19
            }

            internal enum AccentState
            {
                ACCENT_DISABLED = 0,
                ACCENT_ENABLE_GRADIENT = 1,
                ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
                ACCENT_ENABLE_BLURBEHIND = 3,
                ACCENT_INVALID_STATE = 4
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct AccentPolicy
            {
                public AccentState AccentState;
                public int AccentFlags;
                public int GradientColor;
                public int AnimationId;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct DWM_BLURBEHIND
            {
                public DWM_BB dwFlags;
                public bool fEnable;
                public IntPtr hRgnBlur;
                public bool fTransitionOnMaximized;

                public DWM_BLURBEHIND(bool enabled)
                {
                    fEnable = enabled;
                    hRgnBlur = IntPtr.Zero;
                    fTransitionOnMaximized = false;
                    dwFlags = DWM_BB.Enable;
                }

                public Region Region
                {
                    get { return Region.FromHrgn(hRgnBlur); }
                }

                public bool TransitionOnMaximized
                {
                    get { return fTransitionOnMaximized; }
                    set
                    {
                        fTransitionOnMaximized = value;
                        dwFlags |= DWM_BB.TransitionMaximized;
                    }
                }

                public void SetRegion(Graphics graphics, Region region)
                {
                    hRgnBlur = region.GetHrgn(graphics);
                    dwFlags |= DWM_BB.BlurRegion;
                }
            }
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
    }
}