using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DunaGrid.components
{
    public class CellResizeEventArgs : MouseEventArgs
    {
        private ResizeSide rs;

        public enum ResizeSide
        {
            Left,
            Right
        }

        public ResizeSide CellResizeSide
        {
            get
            {
                return this.rs;
            }
        }

        public CellResizeEventArgs(MouseEventArgs e, ResizeSide rs) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
            this.rs = rs;
        }
    }
}
