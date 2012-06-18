﻿using System;
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
        protected static int MouseWheelScrollLines = SystemInformation.MouseWheelScrollLines; //vytahne z nastaveni windows o kolik radku se ma grid posunout pri posunuti kolecka mysi

        #region protected datove cleny
        /// <summary>
        /// Zdroj dat pro zobrazeni v gridu
        /// Grid bude umet vice moznosti: DataTable, BindingSource, array,...
        /// </summary>
        protected object dataSource=null;

        protected RowsCollection rows;

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

        protected int row_height = 20;

        protected ColumnCollection columns = new ColumnCollection();

        protected FormatterCollection formatters = new FormatterCollection();

        #endregion

        #region vlastnosti (properties)

        public RowsCollection Rows
        {
            get
            {
                return this.rows;
            }
            set
            {
                this.rows = value;
            }
        }

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
                this.setScrollBars();
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

        public int DefaultRowHeight
        {
            get
            {
                return this.row_height;
            }
            set
            {
                this.row_height = value;
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
            rows = new RowsCollection(this);
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

            Invalidate(); //TODO: volat nejak centralneji?
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
            if (this.dataSource != null)
            {
                this.actual_datareader.DataSource = this.dataSource;
                this.rows.DataReader = this.actual_datareader;
            }
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
                vscrollbar.Maximum = this.rows.Count - rows.getCountVisibleRowsFromBottom(this.ClientSize.Height - 21 - hscrollbar.Height);
                vscrollbar.LargeChange = 1;
            }
            else
            {
                vscrollbar.Maximum = 0;
            }
            vscrollbar.Minimum = 0;

            hscrollbar.MinimumValue = 0;

            int sirka_gridu = 31; //=konstanta sirky sedych obdelniku

            foreach (IColumn c in this.columns)
            {
                sirka_gridu += c.Width + 1;
            }

            hscrollbar.MaximumValue = sirka_gridu;
            int viditelna_sirka = this.Width - vscrollbar.Width;
            if (viditelna_sirka > 0)
            {
                hscrollbar.LargeChange = this.Width - vscrollbar.Width;
                hscrollbar.SmallChange = this.Width - vscrollbar.Width;
            }

            if (hscrollbar.Value + hscrollbar.SmallChange > hscrollbar.MaximumValue)
            {
                int nova_hodnota = hscrollbar.MaximumValue - hscrollbar.SmallChange;
                if (nova_hodnota > 0)
                {
                    hscrollbar.Value = nova_hodnota;
                }
                else
                {
                    hscrollbar.Value = 0;
                }
            }            
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

            gc.Graphics.TranslateTransform(-hscrollbar.Value, 0);

            GraphicsState gs = gc.Graphics.Save();

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

            gc.Graphics.FillRectangle(Brushes.DarkGray, new Rectangle(hscrollbar.Value, 0, 30, 20));

            gc.Graphics.TranslateTransform(0, 21);

            //vykresli jednotlive radky
            int y = 0;
            for (int i = vscrollbar.Value; i < this.rows.Count && y<this.ClientSize.Height; i++)
            {
                IRow radek = this.rows[i];
                int row_height = radek.Height;

                y += row_height + 1;
                gc.Graphics.SetClip(new Rectangle(0, 0, sirka_celeho_gridu, row_height));

                IFormatter formatter = this.formatters.getMatchFormatter(radek);
                radek.Formatter = formatter;

                gc.Graphics.TranslateTransform(31, 0);

                radek.render(gc, this.columns);

                gc.Graphics.TranslateTransform(-31 + hscrollbar.Value, 0);

                //vykresli RowSelector
                radek.renderRowSelector(gc);

                gc.Graphics.TranslateTransform(-hscrollbar.Value, 0);

                gc.Graphics.TranslateTransform(0, row_height + 1);
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
                int zbyvajici_plocha = this.ClientSize.Width - vscrollbar.Width - sirka_neelastickych;

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
