﻿using System;
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
        protected DunaGrid parent;

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

        public RowsCollection(DunaGrid parent_grid, IDataReader dr)
        {
            this.data_reader = dr;
            this.parent = parent_grid;
        }

        public RowsCollection(DunaGrid parent_grid)
        {
            this.parent = parent_grid;
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
                return temp;
            }
            set
            {
                throw new NotImplementedException();  //bude vyzadovat zasah do DataReaderu
            }
        }

        public DunaGrid Parent
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
            get { return this.data_reader.GetRowsCount(); }
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
