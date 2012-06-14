using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using DunaGrid.columns;
using DunaGrid.rows;

namespace DunaGrid.dataReaders
{
    /// <summary>
    /// umozni gridu cist data z .NETovskeho BindingSourcu
    /// </summary>
    class BindingSourceDataReader : IDataReader
    {
        protected BindingSource data_source;

        public BindingSourceDataReader()
        {
            this.data_source = null;
        }

        public BindingSourceDataReader(object data_source)
        {
            this.DataSource = data_source;
        }

        public ColumnCollection GetColumns()
        {
            ColumnCollection temp = new ColumnCollection();

            DataTable dt = (DataTable)data_source.DataSource;

            int i = 0;

            foreach (DataColumn dc in dt.Columns)
            {
                IColumn col = ColumnTypeDelegator.getByType(dc.DataType);
                col.HeadText = dc.ColumnName;
                col.Width = 100;
                col.DataSourceColumnIndex = i;
                temp.Add(col);
                i++;
            }

            return temp;
        }

        public void Order(OrderRule rule)
        {
            throw new NotImplementedException();
        }

        public void Order(OrderRule[] rules)
        {
            throw new NotImplementedException();
        }

        public int GetRowsCount()
        {
            return this.data_source.List.Count;
        }

        public IRow GetRow(int index)
        {
            IRow temp = new StandardRow();

            if (this.data_source != null)
            {
                DataRowView row = (DataRowView)this.data_source.List[index];
                for (int i = 0; i < row.Row.Table.Columns.Count; i++)
                {
                    temp.addCell(row.Row.Table.Columns[i].ColumnName, row[i]);
                }
            }

            return temp;
        }

        public bool IsReadable(object data)
        {
            if (data == null) return false;
            else
            {
                return data.GetType() == typeof(BindingSource);
            }
        }

        public object DataSource
        {
            get
            {
                return this.data_source;
            }
            set
            {
                if (this.IsReadable(value))
                {
                    this.data_source = (BindingSource)value;
                }
            }
        }
    }
}
