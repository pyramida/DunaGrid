using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.columns;
using DunaGrid.formatters;

namespace DunaGrid.rows
{
    /// <summary>
    /// rozhrani pro tridy reprezentujici data v jednom radku
    /// </summary>
    public interface IRow
    {
        int Height { get; set; }

        int Index { get; set; }
        
        object this[string columnname] { get; set; }

        IFormatter Formatter { get; set; }

        RowsCollection parentRowCollection { set; }

        void addCell(string columnname, object value);

        void render(GraphicsContext g, ColumnCollection visible_columns);
    }
}
