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
    public abstract class AbstractGrid : UserControl, IComparable, IXScrollable
    {
        protected ColumnCollection columns;
        protected int posun_x = 0;

        protected int start_index = 0;

        public delegate void EventHandler();

        public event EventHandler NeedResize;

        public GridPosition Position
        {
            get;
            set;
        }

        public int MoveX
        {
            get
            {
                return posun_x;
            }
            set
            {
                posun_x = value;
                this.Refresh();
            }
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

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(-posun_x, 0);
            base.OnPaint(e);
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

        protected virtual void onNeedResize()
        {
            if (NeedResize != null)
            {
                NeedResize();
            }
        }

        protected void RenderVerticalLines(GraphicsContext gc)
        {
            int x = -posun_x;

            gc.Graphics.ResetTransform();

            foreach (IColumn c in this.Columns)
            {
                x += c.Width;
                gc.Graphics.DrawLine(new Pen(Color.DarkGray), new Point(x - 1, 0), new Point(x - 1, this.ClientSize.Height));
            }
        }

        protected void RenderHorizontalLine(GraphicsContext gc, IRow radek)
        {
            gc.Graphics.DrawLine(new Pen(Color.DarkGray), new Point(posun_x, radek.Height), new Point(this.Width + posun_x, radek.Height));
        }


        protected void RenderRow(GraphicsContext gc, IRow row)
        {
            int row_height = row.Height;

            gc.Graphics.SetClip(new Rectangle(posun_x, 0, this.Width + posun_x, row_height));

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

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                MessageBox.Show(GetCell(e.Location).ToString());
            }
        }

        protected virtual CellPosition GetCell(Point click_position)
        {
            // urci radek
            int r_index = this.start_index;
            int r_height = 0;
            List<IRow> visible_rows = this.getVisibleRows();

            for (int i = r_index; i < visible_rows.Count; i++)
            {
                r_height += visible_rows[i].Height + 1;
                if (r_height > click_position.Y)
                {
                    break;
                }

                r_index++;
            }

            IRow row = visible_rows[r_index - this.start_index];

            // urci sloupec

            int c_width = -this.MoveX;
            int c_index = 0;

            for (int i = c_index; c_index < this.Columns.Count; i++)
            {
                c_width += this.Columns[c_index].Width;

                if (c_width > click_position.X)
                {
                    break;
                }

                c_index++;
            }

            IColumn col = this.columns[c_index];
            // vrati vysledek ;)
            return new CellPosition(row, col);
        }

        /*protected virtual IRow GetRow(int poradi)
        {
            return 
        }

        protected virtual IColumn GetColumn(int poradi)
        {

        }*/

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
