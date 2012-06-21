using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DunaGrid.columns;

namespace DunaGrid.components
{
    public partial class DunaGridHeaderRow : UserControl
    {
        protected ColumnCollection columns; 

        public ColumnCollection Columns 
        {
            get
            {
                return this.columns;
            }

            set
            {
                this.columns = value;
                this.CreateCells();
            }
        }

        public int RowSelectorWidth { get; set; }

        public DunaGridHeaderRow()
        {
            InitializeComponent();
            CreateCells();
            this.ResizeRedraw = true;
        }

        protected void CreateCells()
        {
            this.Controls.Clear();

            DunaGridHeaderCell row_select = new DunaGridHeaderCell();
            row_select.Height = this.Height;
            row_select.Width = this.RowSelectorWidth;
            row_select.Location = new Point(0, 0);

            int x = this.RowSelectorWidth;

            this.Controls.Add(row_select);

            if (this.columns != null)
            {
                foreach (IColumn c in this.columns)
                {
                    DunaGridHeaderCell col = new DunaGridHeaderCell();
                    col.Height = this.Height;
                    col.Width = c.Width;
                    Console.WriteLine(c.Width.ToString());
                    col.Text = c.HeadText;
                    col.Location = new Point(x, 0);
                    this.Controls.Add(col);
                    x += c.Width;
                }
            }
        }

        private void DunaGridHeaderRow_Load(object sender, EventArgs e)
        {
            
        }

        private void DunaGridHeaderRow_Resize(object sender, EventArgs e)
        {

        }
    }
}
