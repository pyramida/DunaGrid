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
    public partial class DunaGridRowSelector : AbstractSystemHeader
    {
        public IRow Row
        {
            get;
            set;
        }

        public new int Height
        {
            get
            {
                return this.Row.Height;
            }

            set
            {
                this.Row.Height = value;
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.Row == null)
            {
                base.SetBoundsCore(x, y, width, height, specified);
            }
            else
            {
                base.SetBoundsCore(x, y, width, this.Row.Height, specified);
            }
        }
        

        public DunaGridRowSelector(IRow row)
        {
            this.Row = row;
            InitializeComponent();
        }

        private void DunaGridRowSelector_Load(object sender, EventArgs e)
        {

        }
    }
}
