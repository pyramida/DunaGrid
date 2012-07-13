using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.dataReaders;

namespace DunaGrid.rows
{
    /// <summary>
    /// tvari se jako kolekce, ktera zjednodusuje pristup k datum v DataReaderu
    /// </summary>
    public class RowsCollection : IList<IRow>
    {
        protected IDataReader data_reader = null;
        protected DunaGridView parent;
        protected RowHeightCollection rows_height = new RowHeightCollection();
        protected List<int> pinned_rows = new List<int>();

        public IDataReader DataReader
        {
            get
            {
                return this.data_reader;
            }
            set
            {
                this.data_reader = value;
            }
        }

        public RowsCollection(DunaGridView parent_grid, IDataReader dr)
        {
            this.data_reader = dr;
            this.parent = parent_grid;
            pripichni();
        }

        public RowsCollection(DunaGridView parent_grid)
        {
            this.parent = parent_grid;
            pripichni();
        }

        private void pripichni()
        {
            pinned_rows.Add(10);
            pinned_rows.Add(16);
        }

        /// <summary>
        /// vraci prispendlene radky!
        /// </summary>
        /// <returns></returns>
        public List<IRow> GetPinnedRows()
        {
            List<IRow> rows = new List<IRow>();

            foreach (int index in pinned_rows)
            {
                rows.Add(this[index]);
            }

            return rows;
        }


        /// <summary>
        /// zavola na sebe radek v pripade ze se v gridu zmenili hodnoty
        /// </summary>
        /// <param name="row"></param>
        public void RowDataChange(IRow row)
        {
            
        }

        /// <summary>
        /// zavola na sebe radek v pripade, ze byla zmenena jeho velikost
        /// </summary>
        /// <param name="row"></param>
        public void RowSizeChange(IRow row)
        {
            this.rows_height.setHeight(row.Index, row.Height);
        }

        protected int getHeight(int index)
        {
            if (this.rows_height.ContainsKey(index))
            {
                return this.rows_height[index];
            }
            else
            {
                return this.parent.DefaultRowHeight;
            }
        }

        public int getCountVisibleRowsFromBottom(int height)
        {
            int i = this.Count-1;
            int y = 0;
            int ctr = 0;

            while (y < height && i >= 0)
            {
                if (this.rows_height.ContainsKey(i))
                {
                    y += this.rows_height[i] + 1;
                }
                else
                {
                    y += this.parent.DefaultRowHeight + 1;
                }

                i--;

                if (y < height)
                {
                    ctr++;
                }
            }

            return ctr;
        }

        /*implementace IList*/

        public int IndexOf(IRow item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, IRow item) //bude vyzadovat zasah do DataReaderu
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index) //bude vyzadovat zasah do DataReaderu
        {
            throw new NotImplementedException();
        }

        public IRow this[int index]
        {
            get
            {
                IRow temp = this.data_reader.GetRow(index);
                temp.parentRowCollection = this;
                temp.Height = this.getHeight(index);
                if (this.pinned_rows.Contains(index))
                {
                    temp.Pinned = true;
                }
                else
                {
                    temp.Pinned = false;
                }
                return temp;
            }
            set
            {
                throw new NotImplementedException();  //bude vyzadovat zasah do DataReaderu
            }
        }

        public DunaGridView Parent
        {
            get
            {
                return this.parent;
            }
        }

        public void Add(IRow item)  //bude vyzadovat zasah do DataReaderu
        {
            throw new NotImplementedException();
        }

        public void Clear()  //bude vyzadovat zasah do DataReaderu
        {
            throw new NotImplementedException();
        }

        public bool Contains(IRow item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IRow[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get 
            {
                if (this.data_reader != null)
                {
                    return this.data_reader.GetRowsCount();
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool IsReadOnly //co tohle vlastne dela?
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(IRow item)  //bude vyzadovat zasah do DataReaderu
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IRow> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /*konec implementace IList*/
    }
}
