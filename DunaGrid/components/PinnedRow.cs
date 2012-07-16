using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DunaGrid.rows;

namespace DunaGrid.components
{
    public partial class PinnedRow : AbstractGrid
    {
        public PinnedRow()
        {
            InitializeComponent();
            ResizeRedraw = true;
            DoubleBuffered = true;
            this.Position = GridPosition.Top;
            this.AutoSizeMode = GridSizeMode.FullLenght;
            this.SortWeight = 2;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            GraphicsContext gc = new GraphicsContext();
            gc.Graphics = e.Graphics;

            foreach (IRow r in this.Rows.GetPinnedRows())
            {
                this.RenderRow(gc, r);
            }



            this.RenderVerticalLines(gc);

            base.OnPaint(e);
        }

        protected override void OnClick(EventArgs e)
        {
            this.Refresh();
            Console.WriteLine("pocet sloupcu: {0}", this.Columns.Count);
            base.OnClick(e);
        }

        protected override int getHeight()
        {
            if (this.Rows != null)
            {
                List<IRow> pinned = this.Rows.GetPinnedRows();
                if (pinned.Count > 0)
                {
                    int vyska = 0;
                    foreach (IRow r in this.Rows.GetPinnedRows())
                    {
                        if (r!=null) vyska += r.Height + 1;
                    }

                    return vyska;
                }
                else
                {
                    return this.Height;
                }
            }
            else
            {
                return this.Height;
            }
        }

        public override List<IRow> getVisibleRows()
        {
            return this.Rows.GetPinnedRows();
        }
    }
}
