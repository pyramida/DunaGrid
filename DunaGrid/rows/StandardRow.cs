using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.columns;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace DunaGrid.rows
{
    class StandardRow : IRow
    {
        protected Dictionary<string, object> cells_values = new Dictionary<string, object>();

        public object this[string columnname]
        {
            get
            {
                return this.cells_values[columnname];
            }
            set
            {
                this.cells_values[columnname] = value;
            }
        }

        public void addCell(string columnname, object value)
        {
            this.cells_values.Add(columnname, value);
        }


        public void render(GraphicsContext g, columns.ColumnCollection visible_columns)
        {
            GraphicsState gs = g.Graphics.Save();

            g.Graphics.Clear(Color.White);

            g.Graphics.FillRectangle(Brushes.DarkGray, new Rectangle(0, 0, 30, 20));

            g.Graphics.TranslateTransform(31,0);

            foreach (IColumn c in visible_columns)
            {
                c.renderCellBackground(g);
                c.renderCell(g, this.cells_values[c.Name]);
                g.Graphics.TranslateTransform(c.Width + 1, 0);
            }

            g.Graphics.Restore(gs);
        }
    }
}
