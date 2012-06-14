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
        /// urcuje podminky, za kterych se formatovani aplikuje
        /// </summary>
        ICondition Condition { get; set; }

        /// <summary>
        /// nastavuje pismo, ktere se pouzije v pripade ze podminka plati
        /// null znamena, ze se pouzije defautlni pismo
        /// </summary>
        Font Font { get; set; }

        /// <summary>
        /// nastavuje barvu pisma, ktera se pouzije v pripade ze podminka plati
        /// null znamena, ze se pouzije defautlni barva
        /// </summary>
        Color FontColor { get; set; }

        /// <summary>
        /// nastavuje barvu pozadi, ktera se pouzije v pripade ze podminka plati
        /// null znamena, ze se pouzije defautlni barva pozadi
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// nastavuje zarovnani obsahu bunky, ktere se pouzije v pripade ze podminka plati
        /// null znamena, ze se pouzije defautlni zarovnani
        /// </summary>
        ContentAlignment CellAlign { get; set; }

        /// <summary>
        /// vraci font, ktery se ma pouzit pri vykresleni
        /// </summary>
        /// <returns></returns>
        Font getFont();

        /// <summary>
        /// Vraci barvu pisma
        /// </summary>
        /// <returns></returns>
        Color getFontColor();

        /// <summary>
        /// vraci barvu pozadi
        /// </summary>
        /// <returns></returns>
        Color getBackgroundColor(); //vrati barvu pozadi

        /// <summary>
        /// vraci zarovnani
        /// </summary>
        /// <returns></returns>
        ContentAlignment getContentAlign();

        /// <summary>
        /// vraci true nebo false podle toho jestli sedi formatovaci pravidlo
        /// </summary>
        /// <returns></returns>
        bool isMatch(IRow row);
    }
}
