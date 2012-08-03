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
    public partial class DunaGridHeaderRow : UserControl, IXScrollable
    {
        protected ColumnCollection columns;
        private bool disable_elastics=false;
        private int movex = 0;

        public ColumnCollection Columns 
        {
            get
            {
                return this.columns;
            }

            set
            {
                this.columns = value;
                if (this.columns != null)
                {
                    foreach (IColumn c in this.columns)
                    {
                        c.WidthChanged += new EventHandler(column_WidthChanged);
                    }
                    countElasticColumnsWidth();
                    this.CreateCells();
                }
            }
        }

        void column_WidthChanged(object sender, EventArgs e)
        {
            IColumn col = (IColumn)sender;

            foreach (object o in this.Controls)
            {
                if (o is DunaGridHeaderCell)
                {
                    DunaGridHeaderCell cell = (DunaGridHeaderCell)o;

                    if (cell.Name == col.Name) cell.Refresh();
                }
            }
        }

        public DunaGridHeaderRow()
        {
            InitializeComponent();
            countElasticColumnsWidth();
            CreateCells();
            this.ResizeRedraw = true;
        }

        protected void CreateCells()
        {
            this.Controls.Clear();

            int x = 0;
            int ctr = 0;

            if (this.columns != null)
            {
                int pinned_width = GetPinnedColsWidth();

                x += pinned_width;

                bool prev_elastic = false;

                //vytvori nepripinovane

                foreach (IColumn c in this.columns)
                {
                    DunaGridHeaderCell col = CreateCell(x, c);

                    if (ctr == 0)
                    {
                        col.EnableLeftResize = false;
                        col.PositionInRow = AbstractSystemHeader.cellPosition.first;
                    }
                    else if (ctr == this.columns.Count - 1)
                    {
                        col.PositionInRow = AbstractSystemHeader.cellPosition.last;
                        if (col.IsElastic)
                        {
                            col.EnableRightResize = false;
                        }
                    }
                    else
                    {
                        col.PositionInRow = AbstractSystemHeader.cellPosition.middle;
                    }

                    if (prev_elastic)
                    {
                        prev_elastic = false;
                        col.IsPrevColElastic = true;
                    }
                    if (col.IsElastic)
                    {
                        prev_elastic = true;
                    }
                    this.Controls.Add(col);
                    x += c.Width;
                    ctr++;
                    col.Pinned = false;
                }

                //vytvori pripinovane
                foreach (IColumn c in this.columns.getPinnedColumns())
                {
                    x = 0;
                    DunaGridHeaderCell col = CreateCell(x, c);
                    col.PositionInRow = AbstractSystemHeader.cellPosition.middle;
                    this.Controls.Add(col);
                    col.Pinned = true;
                    col.BringToFront();
                    x += col.Width;
                }
            }
        }

        private DunaGridHeaderCell CreateCell(int x, IColumn c)
        {
            DunaGridHeaderCell col = new DunaGridHeaderCell(c);
            col.Height = this.Height;
            col.Location = new Point(x, 0);
            col.CellResize += new CellResizeEventHandler(col_CellResize);
            col.CellResizeStart += new CellResizeEventHandler(col_CellResizeStart);
            col.CellResizeEnd += new CellResizeEventHandler(col_CellResizeEnd);
            col.Orientation = Orientation.Horizontal;
            return col;
        }

        private int GetPinnedColsWidth()
        {
            int pinned_width = 0;
            if (this.columns != null)
            {
                foreach (IColumn c in this.columns)
                {
                    if (c.Pinned)
                    {
                        pinned_width += c.Width;
                    }
                }
            }
            return pinned_width;
        }

        void col_CellResizeEnd(object sender, CellResizeEventArgs e)
        {
            if (this.disable_elastics)
            {
                this.disable_elastics = false;

                //spocita nove pomery
            }
        }

        void col_CellResizeStart(object sender, CellResizeEventArgs e)
        {
            DunaGridHeaderCell cell = (DunaGridHeaderCell)sender;
            if (cell.IsElastic)
            {
                this.disable_elastics = true; //tady to tak jednoduchy nebude
            }
        }

        void col_CellResize(object sender, CellResizeEventArgs e)
        {
            /*this.mouse_state.setLastLocation(e.Location);
            int delta = this.mouse_state.getDeltaX();
            IColumn c = (IColumn)((DunaGridHeaderCell)this.mouse_state.parameters).LinkedColumn;

            if (c.Elastic)
            {

            }
            else
            {
                c.Width += delta;
            }

            this.countElasticColumnsWidth();
            this.setWidthToControls();*/

            DunaGridHeaderCell resized_col = (DunaGridHeaderCell)sender;

            if (e.CellResizeSide == CellResizeEventArgs.ResizeSide.Left)
            {
                DunaGridHeaderCell prev_resized_col = null; //predchozi

                int index = this.Controls.IndexOf(resized_col);

                if (index > 0)
                {
                    prev_resized_col = (DunaGridHeaderCell)this.Controls[index - 1];
                }

                if (prev_resized_col.IsElastic)
                {

                }
                else
                {
                    prev_resized_col.Width -= prev_resized_col.Location.X + prev_resized_col.Width - resized_col.Location.X;
                }
            }
            else
            {

            }
            if (!this.disable_elastics) this.countElasticColumnsWidth();
            this.DeleteColumnGaps();
        }

        protected void DeleteColumnGaps()
        {
            int x = -this.movex;

            int pinned_width = GetPinnedColsWidth();

            x += pinned_width;
            this.SuspendLayout();
            foreach (object o in this.Controls)
            {
                if (o is DunaGridHeaderCell)
                {
                    DunaGridHeaderCell col = (DunaGridHeaderCell)o;
                    if (!col.Pinned)
                    {
                        col.Location = new Point(x, 0);
                        x += col.Width;
                        col.Refresh();
                    }
                }
            }
            this.ResumeLayout();
        }

        public override void Refresh()
        {
            RefreshControls();
            base.Refresh();
        }

        protected void countElasticColumnsWidth()
        {
            if (this.columns != null)
            {
                if (this.columns.Count > 0)
                {

                    int sirka_neelastickych = 0;
                    float sirka_elastickych = 0;
                    List<int> elasticke_sloupce = new List<int>(); //uchovava indexy elastickych sloupcu

                    for (int i = 0; i < this.columns.Count; i++)
                    {
                        IColumn c = this.columns[i];
                        if (c.Elastic)
                        {
                            elasticke_sloupce.Add(i);
                            sirka_elastickych += c.RatioWidth;
                        }
                        else
                        {
                            sirka_neelastickych += c.Width;
                        }
                    }

                    if (elasticke_sloupce.Count > 0)
                    {
                        int zbyvajici_plocha = this.Width - sirka_neelastickych;

                        foreach (int index in elasticke_sloupce)
                        {
                            float pomer = (float)this.columns[index].RatioWidth / sirka_elastickych;
                            int nova_sirka = (int)Math.Round((float)zbyvajici_plocha * pomer);
                            if (nova_sirka > this.columns[index].MinimalWidth)
                            {
                                this.columns[index].Width = nova_sirka;
                            }
                            else
                            {
                                this.columns[index].Width = this.columns[index].MinimalWidth;
                            }
                        }
                    }
                }
            }
        }



        protected override void OnResize(EventArgs e)
        {
            RefreshControls();
            base.OnResize(e);
        }

        private void RefreshControls()
        {
            countElasticColumnsWidth();
            this.DeleteColumnGaps();
        }

        public void test()
        {
            Console.WriteLine("ZMACKNUTO TEST");
            this.columns[0].Width = 50;
        }

        protected void setWidthToControls()
        {
            /*if (this.columns!=null)
            {
                int x = 0;

                foreach (IColumn c in this.columns)
                {
                    DunaGridHeaderCell hc = (DunaGridHeaderCell)this.Controls[c.Name];

                    if (x != hc.Location.X)
                    {
                        hc.Location = new Point(x, 0);
                    }

                    if (hc.Width != c.Width)
                    {
                        hc.Width = c.Width;
                    }

                    x += c.Width;
                }
            }*/
        }

        private void DunaGridHeaderRow_Load(object sender, EventArgs e)
        {
            
        }

        private void DunaGridHeaderRow_Resize(object sender, EventArgs e)
        {

        }

        public int MoveX
        {
            get
            {
                return this.movex;
            }
            set
            {
                this.movex = value;
                this.DeleteColumnGaps();
            }
        }
    }
}
