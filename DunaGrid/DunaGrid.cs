using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DunaGrid.components;
using DunaGrid.dataReaders;
using DunaGrid.columns;
using DunaGrid.rows;
using DunaGrid.formatters;

namespace DunaGrid
{
    public partial class DunaGrid : Control
    {
        protected static int MouseWheelScrollLines = SystemInformation.MouseWheelScrollLines;

        #region protected datove cleny
        /// <summary>
        /// Zdroj dat pro zobrazeni v gridu
        /// Grid bude umet vice moznosti: DataTable, BindingSource, array,...
        /// </summary>
        protected object dataSource=null;

        /// <summary>
        /// vertikalni scrollbar
        /// </summary>
        protected VScrollBar vscrollbar = new VScrollBar();

        /// <summary>
        /// horizontalni scrollbar
        /// </summary>
        protected DunaHScrollBar hscrollbar = new DunaHScrollBar();

        /// <summary>
        /// obsahuje vsechny dostupne DataReadery
        /// z teto kolekce se vybira nejvhodnejsi
        /// </summary>
        protected DataReaderCollection data_readers = new DataReaderCollection();

        /// <summary>
        /// aktualni datareader
        /// TODO: zmena nazvu tridy? V .NET uz jednou je (=zdroj potencialnich problemu)
        /// </summary>
        protected dataReaders.IDataReader actual_datareader = null;

        protected bool autocolumn = true;

        protected Padding padding = new Padding(3);

        protected ColumnCollection columns = new ColumnCollection();

        protected FormatterCollection formatters = new FormatterCollection();

        #endregion

        #region vlastnosti (properties)

        public object DataSource
        {
            get
            {
                return this.dataSource;
            }

            set
            {
                this.dataSource = value;
                this.setDataReader();
                this.generateColumns();
            }
        }

        public FormatterCollection RowFormatters
        {
            get
            {
                return this.formatters;
            }
            set
            {
                this.formatters = value;
            }
        }

        public ColumnCollection Columns
        {
            get
            {
                return this.columns;
            }
            set
            {
                this.columns = value;
            }
        }

        public bool AutoColumnGenerator
        {
            get
            {
                return this.autocolumn;
            }

            set
            {
                this.autocolumn = value;
            }
        }

        public Padding CellPadding
        {
            get
            {
                return this.padding;
            }
            set
            {
                this.padding = value;
            }
        }

        #endregion

        #region Kontruktory

        public DunaGrid()
        {
            this.InicializeComponent();
        }

        #endregion

        /// <summary>
        /// prida scrollbary eventuelne dalsi potrebne komponenty
        /// </summary>
        protected void InicializeComponent()
        {
            vscrollbar.Dock = DockStyle.Right;
            this.Controls.Add(vscrollbar);
            hscrollbar.Height = 17;
            hscrollbar.NavigateBarWidth = 120;
            hscrollbar.RightMargin = vscrollbar.Width;
            hscrollbar.Dock = DockStyle.Bottom;
            hscrollbar.Scroll +=new ScrollEventHandler(hscrollbar_Scroll);
            vscrollbar.Scroll += new ScrollEventHandler(hscrollbar_Scroll);

            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            this.Controls.Add(hscrollbar);

            //prida datareadery do kolekce
            this.data_readers.Add(new BindingSourceDataReader(this.data_readers));
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int nova_hodnota = this.vscrollbar.Value;

            if (e.Delta > 0)
            {
                nova_hodnota -= MouseWheelScrollLines;
            }
            else
            {
                nova_hodnota += MouseWheelScrollLines;
            }

            if (nova_hodnota >= this.vscrollbar.Minimum && nova_hodnota <= this.vscrollbar.Maximum)
            {
                this.vscrollbar.Value = nova_hodnota;
            }
            else if (nova_hodnota < this.vscrollbar.Minimum)
            {
                this.vscrollbar.Value = 0;
            }
            else
            {
                this.vscrollbar.Value = this.vscrollbar.Maximum;
            }
            Invalidate();
        }

        private void DunaGrid_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// na zaklade DataSource vybere vhodny datareader z kolekce this.data_readers a ulozi ho do this.actual_datareader
        /// </summary>
        protected void setDataReader()
        {
            this.actual_datareader = this.data_readers.GetByType(this.dataSource);
            if (this.dataSource!=null) this.actual_datareader.DataSource = this.dataSource;
        }

        /// <summary>
        /// na zaklade datoveho zdroje vygeneruje sloupce
        /// </summary>
        protected void generateColumns()
        {
            if (this.autocolumn && this.actual_datareader!=null)
            {
                this.columns = this.actual_datareader.GetColumns();
            }
        }

        protected void setScrollBars()
        {
            if (this.actual_datareader != null)
            {
                vscrollbar.Maximum = this.actual_datareader.GetRowsCount();
            }
            else
            {
                vscrollbar.Maximum = 0;
            }
            vscrollbar.Minimum = 0;

            
        }

        #region Vykreslovani
        /// <summary>
        /// Vykresleni gridu
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            int sirka_celeho_gridu = 31; //31 = sirka sedych obdelniku pred radkem

            //vykresli hlavicky sloupcu
            GraphicsContext gc = new GraphicsContext();
            gc.Graphics = e.Graphics;

            GraphicsState gs = gc.Graphics.Save();

            gc.Graphics.FillRectangle(Brushes.DarkGray, new Rectangle(0, 0, 30, 20));

            gc.Graphics.TranslateTransform(31, 0);

            this.countWidthForElasticColumn();

            foreach (IColumn c in this.columns)
            {
                gc.Graphics.SetClip(new Rectangle(0,0, c.Width, 20));
                c.renderHead(gc, new ColumnContext(20, CellRenderState.Normal, new OrderRule())); // zatim pouze testovaci (velikost radku tu ve finale asi nebude)
                gc.Graphics.TranslateTransform(c.Width + 1, 0);
                sirka_celeho_gridu += c.Width + 1;
            }

            gc.Graphics.Restore(gs);

            //vykresli jednotlive radky
            int y = 0;
            for (int i = vscrollbar.Value; i < actual_datareader.GetRowsCount() && y<this.ClientSize.Height; i++)
            {
                gc.Graphics.TranslateTransform(0, 21);
                y += 21;
                gc.Graphics.SetClip(new Rectangle(0, 0, sirka_celeho_gridu, 20));
                IRow radek = this.actual_datareader.GetRow(i);
                IFormatter formatter = this.formatters.getMatchFormatter(radek);
                radek.Formatter = formatter;
                radek.render(gc, this.columns);
            }

            base.OnPaint(e);
        }

        protected void countWidthForElasticColumn()
        {
            int sirka_neelastickych = 31; //31 = sirka sedych obdelniku pred radkem
            int sirka_elastickych = 0;
            List<int> elasticke_sloupce = new List<int>(); //uchovava indexy elastickych sloupcu

            for (int i = 0; i < this.columns.Count; i++)
            {
                IColumn c = this.columns[i];
                if (c.Elastic)
                {
                    elasticke_sloupce.Add(i);
                    sirka_elastickych += c.MinimalWidth;
                }
                else
                {
                    sirka_neelastickych += c.Width + 1;
                }
            }

            if (elasticke_sloupce.Count > 0)
            {
                sirka_neelastickych += elasticke_sloupce.Count;
                int zbyvajici_plocha = this.ClientSize.Width - sirka_neelastickych;

                foreach (int index in elasticke_sloupce)
                {
                    float pomer = (float)this.columns[index].MinimalWidth / sirka_elastickych;
                    int nova_sirka = (int)Math.Floor((float)zbyvajici_plocha * pomer);
                    if (nova_sirka > this.columns[index].MinimalWidth)
                    {
                        this.columns[index].Width = nova_sirka;
                    }
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            setScrollBars();
            base.OnResize(e);
        }
        
        /// <summary>
        /// vykresleni pozadi
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected void hscrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            Refresh();
        }

        #endregion
    }
}
