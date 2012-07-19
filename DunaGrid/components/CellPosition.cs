using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.rows;
using DunaGrid.columns;

namespace DunaGrid.components
{
    public struct CellPosition
    {
        public IRow row;
        public IColumn col;

        public CellPosition(IRow row, IColumn col)
        {
            this.row = row;
            this.col = col;
        }

        public override string ToString()
        {
            return "RADEK: " + row["Sloupec A"].ToString()+"\nSLOUPEC: "+col.Name;
        }
    }
}
