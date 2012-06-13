using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DunaGridFormTest
{
    public partial class Form1 : Form
    {
        DataSet ds = new DataSet(); //testovani dataset

        BindingSource bs = new BindingSource();

        public Form1()
        {
            InitializeComponent();

            //naplni dataset testovacimi daty

            DataTable dt = new DataTable("test_table");
            dt.Columns.Add("Sloupec A");
            dt.Columns.Add("Sloupec B");
            dt.Columns.Add("Sloupec C");
            dt.Columns.Add("Sloupec D");
            dt.Columns.Add("Sloupec E");

            Random rnd = new Random();

            //naplnime tabulku nahodnymi daty
            for (int i = 0; i < 500; i++)
            {
                DataRow dr = dt.NewRow();
                for (int ic = 0; ic < dt.Columns.Count; ic++)
                {
                    dr[ic] = rnd.Next(0, 999);
                }
                dt.Rows.Add(dr);
            }
            ds.Tables.Add(dt);

            bs.DataSource = ds.Tables["test_table"];

            dunaGrid1.DataSource = bs;

            dunaGrid1.Columns[2].Elastic = true; //sloupec se bude roztahovat 
            dunaGrid1.Columns[2].MinimalWidth = 150;

            dunaGrid1.Columns[1].Elastic = true;
            dunaGrid1.Columns[1].MinimalWidth = 100;

            dunaGrid1.Columns[3].Elastic = true;
            dunaGrid1.Columns[3].MinimalWidth = 90;

            dunaGrid1.Columns[4].Elastic = true;
            dunaGrid1.Columns[4].MinimalWidth = 100;
        }

        private void dunaGrid1_Click(object sender, EventArgs e)
        {
           
        }
    }
}
