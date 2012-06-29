using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

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
        public TextColumn(string name, DunaGrid parent)
        {
            this.column_name = name;
            this.parent = parent;
        }

        public override void renderCell(GraphicsContext g, object value, CellRenderState render_state = CellRenderState.Normal)
        {
            string hodnota = value.ToString();

            SizeF velikost = g.Graphics.MeasureString(hodnota, g.Font); //spocitam jakou velikost zabere vykreslena hodnota

            switch (render_state)
            {
                case CellRenderState.Normal:
                    g.Graphics.DrawString(hodnota, g.Font, Brushes.Black, new PointF(3, 3));
                    break;

                case CellRenderState.Disable:

                    break;

                case CellRenderState.Edit:

                    break;
            }
        }
    }
}
