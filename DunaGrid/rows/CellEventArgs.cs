using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.components;

namespace DunaGrid.rows
{
    public delegate void CellEventHandler(object sender, CellEventArgs e);

    public class CellEventArgs : EventArgs
    {
        public CellPosition Position { get; set; }
        public object OldValue { get; set; }
        public object Value { get; set; }
    }
}
