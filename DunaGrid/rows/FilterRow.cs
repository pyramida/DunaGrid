using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.components;


namespace DunaGrid.rows
{
    class FilterRow : StandardRow
    {
        public override int Height
        {
            get
            {
                return 25;
            }
        }

        protected override void renderCell(GraphicsContext g, object value, CellRenderState rs, columns.IColumn c)
        {
            c.renderCellBackground(g, rs);

            //g.Graphics.DrawImage(
        }
    }
}
