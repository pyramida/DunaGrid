using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DunaGrid.columns
{
    public class NumberColumn : TextColumn
    {
        /// <summary>
        /// prazdny konstruktor
        /// </summary>
        public NumberColumn()
        {

        }

        /// <summary>
        /// konstruktor, umoznujici vytvorit sloupec s jmenem
        /// </summary>
        /// <param name="name"></param>
        public NumberColumn(string name)
        {
            this.column_name = name;
        }
                
        /// <summary>
        /// Komplexni konstruktor, nastavi vse dulezite
        /// </summary>
        /// <param name="name">jmeno sloupce</param>
        /// <param name="parent">grid, ve kterem se nachazi</param>
        public NumberColumn(string name, DunaGridView parent)
        {
            this.column_name = name;
            this.parent = parent;
        }
    }
}
