using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.components.editors;
using System.Drawing;
using System.Windows.Forms;

namespace DunaGrid.columns
{
    public class NumberColumn : TextBasedColumn
    {
        /// <summary>
        /// prazdny konstruktor
        /// </summary>
        public NumberColumn()
        {
            this.string_format.Alignment = StringAlignment.Far;
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

        public override AbstractGridEditor GetEditControl()
        {
            return new IntegerEditor();
        }
    }
}
