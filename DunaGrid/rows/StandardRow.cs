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
        protected Dictionary<int, object> cells_values = new Dictionary<int, object>();

        public object this[int cell_index]
        {
            get
            {
                if (cells_values.ContainsKey(cell_index))
                {
                    return this.cells_values[cell_index];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (cells_values.ContainsKey(cell_index))
                {
                    this.cells_values[cell_index] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        public void addCell(object value)
        {
            this.cells_values.Add(this.cells_values.Count, value);
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
                c.renderCell(g, this.cells_values[c.DataSourceColumnIndex]);
                g.Graphics.TranslateTransform(c.Width + 1, 0);
            }

            g.Graphics.Restore(gs);
        }
    }
}
