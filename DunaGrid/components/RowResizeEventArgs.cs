using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DunaGrid.components
{
    public class RowResizeEventArgs : MouseEventArgs
    {
        private ResizeSide rs;

        public enum ResizeSide
        {
            Top,
            Bottom
        }

        public ResizeSide RowResizeSide
        {
            get
            {
                return this.rs;
            }
        }

        public RowResizeEventArgs(MouseEventArgs e, ResizeSide rs) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            this.rs = rs;
        }
    }
}
