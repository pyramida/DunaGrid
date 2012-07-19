﻿using System;
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
    public partial class BaseGrid : AbstractGrid
    {
        public int StartIndex
        {
            get
            {
                return this.start_index;
            }

            set
            {
                if (this.start_index != value)
                {
                    this.start_index = value;
                    Refresh();
                }
            }
        }

        public BaseGrid()
        {
            InitializeComponent();
            ResizeRedraw = true;
            this.Position = GridPosition.Bottom;
            this.AutoSizeMode = GridSizeMode.Fill;
            this.SortWeight = 0;
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.Columns == null || this.Rows == null) return;

            //vykresli bunky
            GraphicsContext gc = new GraphicsContext();
            gc.Graphics = e.Graphics;
            int y = 0;
            for (int i = this.StartIndex; i < this.Rows.Count && y < this.ClientSize.Height; i++)
            {
                IRow radek = this.Rows[i];

                y += radek.Height + 1;

                this.RenderRow(gc, radek);
            }

            gc.Graphics.ResetTransform();

            RenderVerticalLines(gc);
        }

        public override List<IRow> getVisibleRows()
        {
            List<IRow> output = new List<IRow>();

            if (this.Rows!=null)
            {
                int y = 0;

                for (int i = this.StartIndex; i < this.Rows.Count && y < this.ClientSize.Height; i++)
                {
                    IRow radek = this.Rows[i];

                    y += radek.Height + 1;

                    output.Add(radek);
                }
            }

            return output;
        }
    }
}
