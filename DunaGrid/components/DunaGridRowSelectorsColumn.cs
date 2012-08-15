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
    public partial class DunaGridRowSelectorsColumn : UserControl
    {
        private List<IRow> rows;

        public event EventHandler NeedRefresh;

        public List<IRow> Rows 
        {
            get
            {
                return this.rows;
            }

            set
            {
                this.rows = value;
                CreateControlsNew();
            }
        }



        public DunaGridRowSelectorsColumn()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        protected void CreateControlsNew()
        {
            if (this.Rows == null) return;

            this.SuspendLayout();

            int pocet_selectoru = this.Controls.Count;

            int y = 0;

            for (int i = 0; i < this.Rows.Count; i++)
            {
                if (i < pocet_selectoru)
                {

                    DunaGridRowSelector sel = (DunaGridRowSelector)this.Controls[i];
                    sel.Row = this.Rows[i];
                    sel.Location = new Point(0, y);
                    sel.Width = this.Width;
                }
                else
                {
                    DunaGridRowSelector sel = new DunaGridRowSelector(this.Rows[i]);
                    sel.Location = new Point(0, y);
                    sel.Orientation = Orientation.Vertical;
                    sel.Width = this.Width;
                    sel.RowResize += new DunaGridRowSelector.RowResizeEventHandler(sel_RowResize);
                    this.Controls.Add(sel);
                    if (i>0)
                    {
                        sel.PositionInRow = AbstractSystemHeader.cellPosition.middle;
                    }
                    else
                    {
                        sel.PositionInRow = AbstractSystemHeader.cellPosition.first;
                    }
                }

                y += this.Rows[i].Height + 1;
            }

            this.ResumeLayout();
        }

        protected virtual void OnNeedRefresh()
        {
            if (NeedRefresh != null)
            {
                NeedRefresh(this, EventArgs.Empty);
            }
        }

        protected void sel_RowResize(object sender, RowResizeEventArgs e)
        {
            CreateControlsNew();
            OnNeedRefresh();
        }

        protected void CreateControls()
        {
            if (this.Rows == null) return;

            this.SuspendLayout();
            this.Controls.Clear();

            int y = 0;

            int ctr = 0;

            for (int i=0; i<this.Rows.Count; i++)
            {
                IRow r = this.Rows[i];
                DunaGridRowSelector sel = new DunaGridRowSelector(r);
                sel.Location = new Point(0, y);
                sel.Width = this.Width;
                sel.Orientation = Orientation.Vertical;

                if (ctr > 0)
                {
                    sel.PositionInRow = AbstractSystemHeader.cellPosition.middle;
                }
                else
                {
                    sel.PositionInRow = AbstractSystemHeader.cellPosition.first;
                }

                this.Controls.Add(sel);

                y += r.Height + 1;

                ctr++;

                if (y > this.Height) break;
            }
            this.ResumeLayout();
        }

        private void DunaGridRowSelectorsColumn_Load(object sender, EventArgs e)
        {

        }
    }
}
