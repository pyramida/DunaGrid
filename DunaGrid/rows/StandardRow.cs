using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.columns;
using DunaGrid.formatters;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace DunaGrid.rows
{
    public class StandardRow : IRow
    {
        protected Dictionary<string, object> cells_values = new Dictionary<string, object>();
        protected IFormatter formatter = null;

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

            if (this.formatter == null)
            {
                g.Graphics.Clear(Color.White);
            }
            else
            {
                g.Graphics.Clear(this.formatter.BackgroundColor);
            }

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


        public formatters.IFormatter Formatter
        {
            get
            {
                return this.formatter;
            }
            set
            {
                this.formatter = value;
            }
        }
    }
}
