using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.rows;
using DunaGrid.columns;

namespace DunaGrid.components
{
    public struct CellPosition : IEquatable<CellPosition>
    {
        public IRow row;
        public IColumn col;

        public static CellPosition Empty
        {
            get
            {
                return new CellPosition(null, null);
            }
        }

        public CellPosition(IRow row, IColumn col)
        {
            this.row = row;
            this.col = col;
        }

        public override string ToString()
        {
            return "RADEK: " + row["Sloupec A"].ToString()+"\nSLOUPEC: "+col.Name;
        }

        public bool Equals(CellPosition other)
        {
            if (this.row == other.row && this.col == other.col)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(CellPosition first, CellPosition second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(CellPosition first, CellPosition second)
        {
            return !first.Equals(second);
        }
    }
}
