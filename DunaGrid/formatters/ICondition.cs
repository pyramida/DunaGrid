using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.rows;

namespace DunaGrid.formatters
{
    public interface ICondition
    {
        /// <summary>
        /// vraci vysledek podminky pro danou hodnota
        /// </summary>
        /// <param name="value">hodnota, kterou porovnavam</param>
        /// <returns></returns>
        bool getResult(IRow row);
    }
}
