using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DunaGrid.components
{
    public partial class DunaGridAllSelector : UserControl
    {
        public DunaGridAllSelector()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawButton(e.Graphics, 0, 0, this.Width, this.Height, ButtonState.Normal);

            base.OnPaint(e);
        }
    }
}
