using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using DunaGrid.components.editors;

namespace DunaGrid.columns
{
    public class TextColumn : TextBasedColumn
    {
        /// <summary>
        /// prazdny konstruktor
        /// </summary>
        public TextColumn()
        {

        }

        /// <summary>
        /// konstruktor, umoznujici vytvorit sloupec s jmenem
        /// </summary>
        /// <param name="name"></param>
        public TextColumn(string name)
        {
            this.column_name = name;
        }

        /// <summary>
        /// Komplexni konstruktor, nastavi vse dulezite
        /// </summary>
        /// <param name="name">jmeno sloupce</param>
        /// <param name="parent">grid, ve kterem se nachazi</param>
        public TextColumn(string name, DunaGridView parent)
        {
            this.column_name = name;
            this.parent = parent;
        }

        public override AbstractGridEditor GetEditControl()
        {
            return new TextBoxEditor();
        }
    }
}
