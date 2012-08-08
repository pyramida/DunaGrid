using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DunaGrid.columns;
using DunaGrid.rows;

namespace DunaGrid.components
{
    public partial class FilterRow : AbstractGrid
    {
        DunaGrid.rows.FilterRow row = new DunaGrid.rows.FilterRow();

        public FilterRow()
        {
            InitializeComponent();
            this.Position = GridPosition.Top;
            this.AutoSizeMode = GridSizeMode.FullLenght;
        }

        public override List<rows.IRow> getVisibleRows()
        {
            List<rows.IRow> output = new List<rows.IRow>();

            output.Add(this.row);

            return output;
        }

        public override ColumnCollection Columns
        {
            get
            {
                return base.Columns;
            }
            set
            {
                base.Columns = value;

                if (this.Columns != null)
                {
                    foreach (IColumn c in this.Columns)
                    {
                        this.row.addCell(c.Name, null);
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GraphicsContext gc = new GraphicsContext();
            gc.Graphics = e.Graphics;

            this.RenderRow(gc, this.row);

            this.RenderVerticalLines(gc);
        }

        protected override int getHeight()
        {
            return 26;
        }
    }
}
