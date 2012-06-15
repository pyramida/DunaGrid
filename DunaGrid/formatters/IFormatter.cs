using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DunaGrid.rows;

namespace DunaGrid.formatters
{
    public interface IFormatter
    {
        /// <summary>
        /// nastavuje pismo, ktere se pouzije v pripade ze podminka plati
        /// null znamena, ze se pouzije defautlni pismo
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// nastavuje barvu pisma, ktera se pouzije v pripade ze podminka plati
        /// </summary>
        Color FontColor { get; set; }

        /// <summary>
        /// nastavuje barvu pozadi, ktera se pouzije v pripade ze podminka plati
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// nastavuje zarovnani obsahu bunky, ktere se pouzije v pripade ze podminka plati
        /// </summary>
        ContentAlignment CellAlign { get; set; }

        /// <summary>
        /// vraci true nebo false podle toho jestli sedi formatovaci pravidlo
        /// </summary>
        /// <returns></returns>
        bool isMatch(IRow row);
    }
}
