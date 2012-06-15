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
        object this[string columnname] { get; set; }

        IFormatter Formatter { get; set; }

        void addCell(string columnname, object value);

        void render(GraphicsContext g, ColumnCollection visible_columns);
    }
}
