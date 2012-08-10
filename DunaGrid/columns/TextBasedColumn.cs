using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DunaGrid.columns
{
    public abstract class TextBasedColumn : AbstractColumn
    {
        protected StringFormat string_format = new StringFormat();

        public override void renderCell(GraphicsContext g, object value, CellRenderState render_state = CellRenderState.Normal)
        {
            base.renderCell(g, value, render_state);

            if (value != null)
            {
                GraphicsState gs = g.Graphics.Save();

                //aplikuje padding
                Padding pad = this.Padding;

                RectangleF area = new RectangleF(g.Graphics.ClipBounds.X + pad.Left,
                                                 g.Graphics.ClipBounds.Y + pad.Top,
                                                 g.Graphics.ClipBounds.Width - pad.Right - pad.Left,
                                                 g.Graphics.ClipBounds.Height - pad.Bottom - pad.Top);

                g.Graphics.SetClip(area);

                string hodnota = value.ToString();

                SizeF velikost = g.Graphics.MeasureString(hodnota, g.Font); //spocitam jakou velikost zabere vykreslena hodnota

                Font font;

                Color color;

                font = g.Font;
                color = this.GetFontColor(render_state);

                g.Graphics.DrawString(hodnota, font, new SolidBrush(color), g.Graphics.ClipBounds, string_format);

                g.Graphics.Restore(gs);
            }
        }
    }
}
