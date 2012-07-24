using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.columns;
using DunaGrid.components;

namespace DunaGrid.rows
{
    public delegate void RowEventHandler(object sender, RowEventArgs e);

    public class RowEventArgs : EventArgs
    {
        public List<string> SelectedCells { get; set; }

        public int Index { get; set; }

        //TODO: dopsat dalsi
    }
}
