using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DunaGrid.rows;
using DunaGrid.columns;
using System.Drawing;

namespace DunaGrid.components
{
    /// <summary>
    /// popisuje vlastnosti vsech gridu ktere jdou umistit do BaseGridsContainers
    /// </summary>
    public abstract class AbstractGrid : UserControl, IComparable
    {
        protected ColumnCollection columns;

        public GridPosition Position
        {
            get;
            set;
        }

        public GridSizeMode AutoSizeMode
        {
            get;
            set;
        }

        public ColumnCollection Columns
        {
            get
            {
                return this.columns;
            }
            set
            {
                this.columns = value;
            }
        }

        public RowsCollection Rows
        {
            get;
            set;
        }

        public int SortWeight
        {
            get;
            set;
        }

        public int CompareTo(object obj)
        {
            if (obj is AbstractGrid)
            {
                AbstractGrid o = (AbstractGrid)obj;
                if (this.Position == o.Position)
                {
                    return this.SortWeight.CompareTo(o.SortWeight);
                }
                else if (this.Position == GridPosition.Top)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }

        protected void RenderVerticalLines(GraphicsContext gc)
        {
            int x = 0;

            gc.Graphics.ResetTransform();

            foreach (IColumn c in this.Columns)
            {
                x += c.Width;
                gc.Graphics.DrawLine(new Pen(Color.DarkGray), new Point(x - 1, 0), new Point(x - 1, this.ClientSize.Height));
            }
        }

        protected void RenderHorizontalLine(GraphicsContext gc, IRow radek)
        {
            gc.Graphics.DrawLine(new Pen(Color.DarkGray), new Point(0, radek.Height), new Point(this.Width, radek.Height));
        }


        protected void RenderRow(GraphicsContext gc, IRow row)
        {
            int row_height = row.Height;

            gc.Graphics.SetClip(new Rectangle(0, 0, this.Width, row_height));

            /*IFormatter formatter = this.formatters.getMatchFormatter(radek);
            radek.Formatter = formatter;*/

            row.render(gc, this.Columns);

            gc.Graphics.ResetClip();

            this.RenderHorizontalLine(gc, row);

            gc.Graphics.TranslateTransform(0, row_height + 1);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.AutoSizeMode == GridSizeMode.FullLenght)
            {
                base.SetBoundsCore(x, y, width, this.getHeight(), specified);
            }
            else
            {
                base.SetBoundsCore(x, y, width, height, specified);
            }
        }

        protected virtual int getHeight()
        {
            return this.Height;
        }

        public abstract List<IRow> getVisibleRows();
    }

    public enum GridPosition
    {
        Top,
        Bottom
    }

    public enum GridSizeMode
    {
        Fill,
        FullLenght
    }
}
