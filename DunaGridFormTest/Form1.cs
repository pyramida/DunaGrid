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

            dunaGrid1.DataSource = bs;

            DunaGrid.formatters.ConditionFormatter cf = new DunaGrid.formatters.ConditionFormatter();
            cf.Condition = new DunaGrid.Condition(dunaGrid1.Columns[0], DunaGrid.Operators.equal, 35);
            cf.BackgroundColor = Color.Red;

            dunaGrid1.RowFormatters.Add(cf);

            dunaGrid1.Rows[10].Height = 30;

            dunaGrid1.Rows[250].Height = 35;


            dunaGrid1.Columns[0].Width = 200;
            dunaGrid1.Columns[1].Width = 100;
            dunaGrid1.Columns[2].ReadOnly = true;
            dunaGrid1.Columns[2].Width = 300;
            dunaGrid1.Columns[3].Width = 100;
            //dunaGrid1.Columns[3].Pinned = true;
            dunaGrid1.Columns[4].Width = 260;

            dunaGrid1.Columns[2].Validators.Add(new DunaGrid.columns.validators.NotNullValidator());
        }

        private void dunaGrid1_Click(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
