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
    public partial class BaseGrid : UserControl
    {
        public ColumnCollection Columns
        {
            get;
            set;
        }

        public RowsCollection Rows
        {
            get;
            set;
        }

        public BaseGrid()
        {
            InitializeComponent();
            ResizeRedraw = true;
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
            for (int i = 0; i < this.Rows.Count && y < this.ClientSize.Height; i++)
            {
                IRow radek = this.Rows[i];
                int row_height = radek.Height;

                y += row_height + 1;
                gc.Graphics.SetClip(new Rectangle(0, 0, this.Width, row_height));

                /*IFormatter formatter = this.formatters.getMatchFormatter(radek);
                radek.Formatter = formatter;*/

                radek.render(gc, this.Columns);

                gc.Graphics.ResetClip();

                gc.Graphics.DrawLine(new Pen(Color.DarkGray), new Point(0, radek.Height), new Point(this.Width, radek.Height));

                gc.Graphics.TranslateTransform(0, row_height + 1);
            }

            gc.Graphics.ResetTransform();

            int x = 0;

            foreach (IColumn c in this.Columns)
            {
                x += c.Width;
                gc.Graphics.DrawLine(new Pen(Color.DarkGray), new Point(x - 1, 0), new Point(x - 1, this.ClientSize.Height));
            }
        }
    }
}
