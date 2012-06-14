using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DunaGrid.columns
{
    /// <summary>
    /// kolekce sloupcu
    /// usnadnuje prehazovani poradi atd.
    /// </summary>
    public class ColumnCollection : List<IColumn>
    {
        public IColumn this[string columnname]
        {
            get
            {
                foreach (IColumn c in this)
                {
                    if (c.Name == columnname) return c;
                }

                throw new IndexOutOfRangeException("neexistujici sloupec");
            }
        }

        /// <summary>
        /// presune polozku na novou pozici
        /// </summary>
        /// <param name="index">index polozky, kterou chci presunout</param>
        /// <param name="position">novy index polozky (index polozky PRED kterou se vlozi)</param>
        public void moveIndex(int index, int position)
        {
            //kontrola, zda je index validni
            if (this.checkIndex(index))
            {
                IColumn temp = this[index]; //docasne si ulozi presouvanou polozku

                int new_index = position; //index kam chci polozku posunout

                //zkontroluje jestli je cilovy index v rozsahu kolekce
                if (this.checkIndex(new_index))
                {
                    this.RemoveAt(index); //smaze presouvanou polozku
                    this.Insert(new_index, temp); //umisti polozku na novou pozici
                }
                else
                {
                    throw new IndexOutOfRangeException("neplatný nový index");
                }
            }
            else
            {
                throw new IndexOutOfRangeException("neplatný index přesouvané položky"); //vyjimka, ze index je mimo rozsah
            }
        }

        /// <summary>
        /// zjisti, jestli je index v realnem rozsahu
        /// </summary>
        /// <param name="i">index</param>
        /// <returns>bool</returns>
        protected bool checkIndex(int i)
        {
            if (i < this.Count && i >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Contains(string columnname)
        {
            foreach (IColumn c in this)
            {
                if (c.Name == columnname) return true;
            }

            return false;
        }
    }
}
