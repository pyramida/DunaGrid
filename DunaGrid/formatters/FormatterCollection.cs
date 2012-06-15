using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.rows;

namespace DunaGrid.formatters
{
    /// <summary>
    /// kolekce formatteru, ktera umi vyhodnotit a vratit spravny formatter
    /// </summary>
    class FormatterCollection : List<IFormatter>
    {
        public IFormatter getMatchFormatter(IRow row)
        {
            foreach (IFormatter f in this)
            {
                if (f.isMatch(row))
                {
                    return f;
                }
            }

            return null;
        }
    }
}
