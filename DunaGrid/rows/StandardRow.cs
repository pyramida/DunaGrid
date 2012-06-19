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
        protected RowsCollection parent_collection=null;
        protected int height, index;

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

        public RowsCollection parentRowCollection
        {
            set
            {
                this.parent_collection = value;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }

            set
            {
                if (value > 0)
                {
                    this.height = value;
                    if (parent_collection != null)
                    {
                        this.parent_collection.RowSizeChange(this);
                    }
                }
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

            int row_height = (int)Math.Floor(g.Graphics.ClipBounds.Height);

            //g.Graphics.TranslateTransform(31,0);

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


        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }


        public void renderRowSelector(GraphicsContext g)
        {
            //g.Graphics.FillRectangle(Brushes.DarkGray, new Rectangle(0, 0, 30, 20));
            g.Graphics.Clear(Color.DarkGray);
        }
    }
}
