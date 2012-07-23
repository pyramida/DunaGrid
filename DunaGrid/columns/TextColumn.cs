using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace DunaGrid.columns
{
    public class TextColumn : AbstractColumn
    {
        /// <summary>
        /// prazdny konstruktor
        /// </summary>
        public TextColumn()
        {

        }

        /// <summary>
        /// konstruktor, umoznujici vytvorit sloupec s jmenem
        /// </summary>
        /// <param name="name"></param>
        public TextColumn(string name)
        {
            this.column_name = name;
        }

        /// <summary>
        /// Komplexni konstruktor, nastavi vse dulezite
        /// </summary>
        /// <param name="name">jmeno sloupce</param>
        /// <param name="parent">grid, ve kterem se nachazi</param>
        public TextColumn(string name, DunaGridView parent)
        {
            this.column_name = name;
            this.parent = parent;
        }

        public override void renderCell(GraphicsContext g, object value, CellRenderState render_state = CellRenderState.Normal)
        {
            base.renderCell(g, value, render_state);

            string hodnota = value.ToString();

            SizeF velikost = g.Graphics.MeasureString(hodnota, g.Font); //spocitam jakou velikost zabere vykreslena hodnota

            Font font;

            Brush color;

            font = g.Font;
            color = new SolidBrush(this.GetFontColor(render_state));

            g.Graphics.DrawString(hodnota, font, color, new PointF(3, 3));
        }

        public override Control GetEditControl()
        {
            TextBox t = new TextBox();
            t.BorderStyle = BorderStyle.Fixed3D;
            t.Multiline = true;
            return t;
        }
    }
}
