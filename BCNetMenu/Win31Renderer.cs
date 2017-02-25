using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCNetMenu
{
    public class Win31Renderer :ToolStripSystemRenderer 
    {
        private Brush BackgroundBrush = new SolidBrush(Color.White);
        private Brush SelectedBrush = new SolidBrush(Color.DarkBlue);
        private Font DrawFont = new Font("System", 10);
        private Pen DrawPen = new Pen(Color.Black, 1);
        private Pen SelectedPen = new Pen(Color.White, 1);
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(BackgroundBrush, e.AffectedBounds);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            e.Graphics.DrawRectangle(DrawPen, e.AffectedBounds);
        }
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle useBounds = new Rectangle(0, 0, e.Item.Bounds.Width, e.Item.Bounds.Height);
           
            if (e.Item.Selected)
            {
               e.Graphics.FillRectangle(SelectedBrush,useBounds);
            }
            
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.Selected ? Color.White : Color.Black;
            e.TextFont = DrawFont;
            if (!e.Item.Enabled)
            {
                e.Item.Enabled = false;
                e.Item.ForeColor = Color.Gray;
                base.OnRenderItemText(e);
                e.Item.Enabled = false;
            }
            else
            {
                base.OnRenderItemText(e);
            }
        }
    }
}
