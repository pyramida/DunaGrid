using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DunaGrid.dataReaders;
using DunaGrid.components;
using DunaGrid.columns;

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
        protected Dictionary<int, List<string>> selected_cells = new Dictionary<int, List<string>>();
        protected IRow edited_row = null;

        public IRow EditedRow
        {
            get
            {
                return this.edited_row;
            }
        }

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

        /// <summary>
        /// zahaji upravu daneho radku
        /// </summary>
        /// <param name="row_index"></param>
        /// <returns>boolean podle toho jestli doslo ke zmene radku, ktery je upravovan</returns>
        public bool StartEditRow(IRow row)
        {
            if (this.edited_row == row)
            {
                return false;
            }
            else
            {
                this.edited_row = row;
                return true;
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
                if (this[index]!=null) rows.Add(this[index]);
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
                if (this.data_reader != null)
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

                    if (this.selected_cells.ContainsKey(index))
                    {
                        temp.SelectCells(this.selected_cells[index]);
                    }

                    temp.CellSelectionChange += new RowEventHandler(temp_CellSelectionChange);
                    temp.CellValueChange += new CellEventHandler(temp_CellValueChange);

                    return temp;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                throw new NotImplementedException();  //bude vyzadovat zasah do DataReaderu
            }
        }

        /// <summary>
        /// v pripade zmeny dat v radku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void temp_CellValueChange(object sender, CellEventArgs e)
        {
            this.data_reader.SetRow(e.Position.row);
        }

        private void temp_CellSelectionChange(object sender, RowEventArgs e)
        {
            IRow r = (IRow)sender;
            if (this.selected_cells.ContainsKey(r.Index))
            {
                this.selected_cells[r.Index] = e.SelectedCells;
            }
            else
            {
                this.selected_cells.Add(r.Index, e.SelectedCells);
            }
        }

        public void UnselectAllCells()
        {
            this.selected_cells.Clear();
        }

        private void increment(ref int i)
        {
            i++;
        }

        private void decrement(ref int i)
        {
            i--;
        }

        public void SelectRange(CellPosition start, CellPosition end)
        {
            ColumnCollection cols = this.parent.Columns;

            int col_s=0, col_e=0;
            this.SetStartAndStop(cols.IndexOf(start.col), cols.IndexOf(end.col), ref col_s, ref col_e);

            int row_s = 0, row_e = 0;
            this.SetStartAndStop(start.row.Index, end.row.Index, ref row_s, ref row_e);

            List<string> selected_cols = new List<string>();
            for (int i = col_s; i <= col_e; i++)
            {
                selected_cols.Add(cols[i].Name);
            }

            for (int i = row_s; i <= row_e; i++)
            {
                this[i].SelectCells(selected_cols);
            }
        }

        private void SetStartAndStop(int i1, int i2, ref int o1, ref int o2)
        {
            if (i1 <= i2)
            {
                o1 = i1;
                o2 = i2;
            }
            else
            {
                o2 = i1;
                o1 = i2;
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
