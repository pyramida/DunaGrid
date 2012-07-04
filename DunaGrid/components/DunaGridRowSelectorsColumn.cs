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
        private RowsCollection rows;

        public RowsCollection Rows 
        {
            get
            {
                return this.rows;
            }

            set
            {
                this.rows = value;
                CreateControls();
            }
        }



        public DunaGridRowSelectorsColumn()
        {
            InitializeComponent();
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

                if (ctr >= 0)
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
