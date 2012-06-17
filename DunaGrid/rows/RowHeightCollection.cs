using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DunaGrid.rows
{
    public class RowHeightCollection : Dictionary<int, int>
    {
        public void setHeight(int row_index, int height)
        {
            if (this.ContainsKey(row_index))
            {
                this[row_index] = height;
            }
            else
            {
                this.Add(row_index, height);
            }
        }
    }
}
