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
            dt.Columns.Add("Sloupec A", typeof(int));
            dt.Columns.Add("Sloupec B");
            dt.Columns.Add("Sloupec C");
            dt.Columns.Add("Sloupec D");
            dt.Columns.Add("Sloupec E");

            Random rnd = new Random();

            //naplnime tabulku nahodnymi daty
            for (int i = 0; i < 500; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i + 1;
                for (int ic = 1; ic < dt.Columns.Count; ic++)
                {
                    dr[ic] = rnd.Next(0, 999);
                }
                dt.Rows.Add(dr);
            }
            ds.Tables.Add(dt);

            bs.DataSource = ds.Tables["test_table"];

            /*dunaGrid1.DataSource = bs;

            DunaGrid.formatters.ConditionFormatter cf = new DunaGrid.formatters.ConditionFormatter();
            cf.Condition = new DunaGrid.formatters.Condition(dunaGrid1.Columns[0], DunaGrid.formatters.Operators.equal, 35);
            cf.BackgroundColor = Color.Red;

            dunaGrid1.RowFormatters.Add(cf);

            dunaGrid1.Columns[2].Elastic = true; //sloupec se bude roztahovat 
            dunaGrid1.Columns[2].RatioWidth = 1;

            dunaGrid1.Columns[1].Elastic = true;
            dunaGrid1.Columns[1].MinimalWidth = 100;
            dunaGrid1.Columns[1].RatioWidth = 2;

            dunaGrid1.Columns[3].Elastic = true;

            dunaGrid1.Columns[4].Elastic = true;
            */

            DunaGrid.columns.ColumnCollection cc = new DunaGrid.columns.ColumnCollection();
            cc.Add(new DunaGrid.columns.TextColumn("Test"));
            cc.Add(new DunaGrid.columns.TextColumn("Test B"));
            cc.Add(new DunaGrid.columns.TextColumn("Test C"));
            cc.Add(new DunaGrid.columns.TextColumn("Pokus"));

            //cc[2].Elastic = true; //sloupec se bude roztahovat 
            //cc[2].RatioWidth = 1;

            //cc[1].Elastic = true;
            //cc[1].MinimalWidth = 50;
            //cc[1].RatioWidth = 2;

            //cc[3].Elastic = true;
            //cc[3].RatioWidth = 1;

            cc[0].Width = 150;
            cc[1].Width = 230;
            cc[2].Width = 50;
            cc[3].Width = 110;

            dunaGridHeaderRow1.Columns = cc;
            

            //maly test s radkem
            //dunaGrid1.Rows[10].Height = 30;

        }

        private void dunaGrid1_Click(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dunaGridHeaderRow1.test();
        }
    }
}
