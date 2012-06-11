using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid;

namespace DunaGrid.columns
{
    /// <summary>
    /// predava se pri vykreslovani
    /// </summary>
    public struct ColumnContext
    {
        public int row_height;
        public CellRenderState renderState;
        public OrderRule order_rules;

        public ColumnContext(int row_height, CellRenderState state, OrderRule order_rules)
        {
            this.row_height = row_height;
            this.renderState = state;
            this.order_rules = order_rules;
        }
    }
}
