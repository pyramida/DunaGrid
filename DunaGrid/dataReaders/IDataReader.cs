using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.columns;
using DunaGrid.rows;

namespace DunaGrid.dataReaders
{
    /// <summary>
    /// rozhrani popisujici tridy, ktere zajistuji cteni z ruznych datovych zdroju
    /// </summary>
    public interface IDataReader
    {
        /// <summary>
        /// Datovy zdroj na ktery je navazan
        /// </summary>
        object DataSource { get; set; }

        /// <summary>
        /// vrati vsechny sloupce tabulky (automaticky je vygeneruje z datoveho zdroje)
        /// </summary>
        /// <returns></returns>
        ColumnCollection GetColumns();

        /// <summary>
        /// zaridi serazeni polozek
        /// </summary>
        /// <param name="rule"></param>
        void Order(OrderRule rule);

        /// <summary>
        /// zaridi serazeni podle vice parametru
        /// </summary>
        /// <param name="rules"></param>
        void Order(OrderRule[] rules);

        /// <summary>
        /// vraci celkovy pocet radku
        /// </summary>
        /// <returns></returns>
        int GetRowsCount();

        /// <summary>
        /// vrati radek podle daneho indexu
        /// </summary>
        /// <param name="index">poradove cislo radku (prvni je 0)</param>
        /// <returns></returns>
        IRow GetRow(int index);

        /// <summary>
        /// metoda urcujici zda datareader umi cist dany typ dat
        /// podle toho se urcuje jaky datareader vybrat
        /// </summary>
        /// <param name="data">data, ktera chci otestovat</param>
        /// <returns></returns>
        bool IsReadable(object data);
    }
}
