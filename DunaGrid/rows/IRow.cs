using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.columns;
using DunaGrid.formatters;
using System.Windows.Forms;

namespace DunaGrid.rows
{
    /// <summary>
    /// rozhrani pro tridy reprezentujici data v jednom radku
    /// </summary>
    public interface IRow
    {
        event RowEventHandler CellSelectionChange;

        int Height { get; set; }

        int Index { get; set; }

        bool Pinned { get; set; }
        
        object this[string columnname] { get; set; }

        IFormatter Formatter { get; set; }

        RowsCollection parentRowCollection { set; }

        void addCell(string columnname, object value);

        void render(GraphicsContext g, ColumnCollection visible_columns);

        void renderRowSelector(GraphicsContext g);

        void SelectCell(string column_name);

        void SelectCells(List<string> column_names);

        bool IsSelectedCell(string col_name);

        Control Edit(IColumn col);
    }
}
