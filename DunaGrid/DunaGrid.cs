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
    public class DunaGridView : UserControl
    {
        protected static int MouseWheelScrollLines = SystemInformation.MouseWheelScrollLines; //vytahne z nastaveni windows o kolik radku se ma grid posunout pri posunuti kolecka mysi

        #region protected datove cleny
        /// <summary>
        /// Zdroj dat pro zobrazeni v gridu
        /// Grid bude umet vice moznosti: DataTable, BindingSource, array,...
        /// </summary>
        protected object dataSource=null;

        protected RowsCollection rows;

        protected MouseState mouse_state = new MouseState();

        protected bool disable_elastics = false;

        /// <summary>
        /// vertikalni scrollbar
        /// </summary>
        protected VScrollBar vscrollbar;

        /// <summary>
        /// horizontalni scrollbar
        /// </summary>
        protected DunaHScrollBar hscrollbar;

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

        protected int vyska_hlavicky = 20;

        protected int sirka_rowselectoru = 30;

        protected Padding padding = new Padding(3);

        protected int row_height = 20;

        protected ColumnCollection columns = new ColumnCollection();

        protected FormatterCollection formatters = new FormatterCollection();
        private DunaGridHeaderRow dunaGridHeaderRow1;
        private DunaGridRowSelectorsColumn dunaGridRowSelectorsColumn1;
        private BaseGrid baseGrid1;

        protected Color line_color = Color.Black;

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

                //vhodne misto?
                baseGrid1.Columns = this.Columns;
                baseGrid1.Rows = this.Rows;
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

        public Color LineColor
        {
            get
            {
                return this.line_color;
            }
            set
            {
                this.line_color = value;
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

        public int RowSelectorWidth
        {
            get
            {
                return this.sirka_rowselectoru;
            }

            set
            {
                if (value>2) this.sirka_rowselectoru = value;
            }
        }

        #endregion

        #region Kontruktory

        public DunaGridView()
        {
            this.InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            //prida datareadery do kolekce
            this.data_readers.Add(new BindingSourceDataReader(this.data_readers));

            rows = new RowsCollection(this);
        }

        #endregion

        /// <summary>
        /// prida scrollbary eventuelne dalsi potrebne komponenty
        /// </summary>
        private void InitializeComponent()
        {
            this.vscrollbar = new System.Windows.Forms.VScrollBar();
            this.dunaGridRowSelectorsColumn1 = new DunaGrid.components.DunaGridRowSelectorsColumn();
            this.dunaGridHeaderRow1 = new DunaGrid.components.DunaGridHeaderRow();
            this.hscrollbar = new DunaGrid.components.DunaHScrollBar();
            this.baseGrid1 = new DunaGrid.components.BaseGrid();
            this.SuspendLayout();
            // 
            // vscrollbar
            // 
            this.vscrollbar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vscrollbar.Location = new System.Drawing.Point(595, 0);
            this.vscrollbar.Name = "vscrollbar";
            this.vscrollbar.Size = new System.Drawing.Size(17, 377);
            this.vscrollbar.TabIndex = 0;
            this.vscrollbar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscrollbar_Scroll);
            // 
            // dunaGridRowSelectorsColumn1
            // 
            this.dunaGridRowSelectorsColumn1.Location = new System.Drawing.Point(0, 17);
            this.dunaGridRowSelectorsColumn1.Name = "dunaGridRowSelectorsColumn1";
            this.dunaGridRowSelectorsColumn1.Rows = null;
            this.dunaGridRowSelectorsColumn1.Size = new System.Drawing.Size(27, 360);
            this.dunaGridRowSelectorsColumn1.TabIndex = 3;
            // 
            // dunaGridHeaderRow1
            // 
            this.dunaGridHeaderRow1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dunaGridHeaderRow1.Columns = null;
            this.dunaGridHeaderRow1.Location = new System.Drawing.Point(33, 3);
            this.dunaGridHeaderRow1.Name = "dunaGridHeaderRow1";
            this.dunaGridHeaderRow1.Size = new System.Drawing.Size(575, 25);
            this.dunaGridHeaderRow1.TabIndex = 2;
            // 
            // hscrollbar
            // 
            this.hscrollbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hscrollbar.LargeChange = 10;
            this.hscrollbar.Location = new System.Drawing.Point(0, 377);
            this.hscrollbar.MaximumValue = 100;
            this.hscrollbar.MinimumValue = 0;
            this.hscrollbar.Name = "hscrollbar";
            this.hscrollbar.NavigateBarWidth = 120;
            this.hscrollbar.RightMargin = 17;
            this.hscrollbar.Size = new System.Drawing.Size(612, 17);
            this.hscrollbar.SmallChange = 1;
            this.hscrollbar.TabIndex = 1;
            this.hscrollbar.Value = 0;
            this.hscrollbar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hscrollbar_Scroll);
            // 
            // baseGrid1
            // 
            this.baseGrid1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.baseGrid1.Location = new System.Drawing.Point(89, 63);
            this.baseGrid1.Name = "baseGrid1";
            this.baseGrid1.Size = new System.Drawing.Size(429, 265);
            this.baseGrid1.TabIndex = 4;
            // 
            // DunaGridView
            // 
            this.BackColor = System.Drawing.Color.DarkGray;
            this.Controls.Add(this.baseGrid1);
            this.Controls.Add(this.dunaGridRowSelectorsColumn1);
            this.Controls.Add(this.dunaGridHeaderRow1);
            this.Controls.Add(this.vscrollbar);
            this.Controls.Add(this.hscrollbar);
            this.Name = "DunaGridView";
            this.Size = new System.Drawing.Size(612, 394);
            this.ResumeLayout(false);

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
                this.dunaGridHeaderRow1.Columns = this.columns;
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

            int sirka_gridu = this.sirka_rowselectoru + 1;

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
            base.OnPaint(e);
            return;
            int sirka_celeho_gridu = this.sirka_rowselectoru + 1;

            //vykresli hlavicky sloupcu
            GraphicsContext gc = new GraphicsContext();
            gc.Graphics = e.Graphics;

            gc.Graphics.TranslateTransform(-hscrollbar.Value, 0);

            GraphicsState gs = gc.Graphics.Save();

            gc.Graphics.TranslateTransform(this.sirka_rowselectoru + 1, 0);

            if (!this.disable_elastics) this.countWidthForElasticColumn();

            foreach (IColumn c in this.columns)
            {
                gc.Graphics.SetClip(new Rectangle(0,0, c.Width, this.vyska_hlavicky));
                c.renderHead(gc, new ColumnContext(this.vyska_hlavicky, CellRenderState.Normal, new OrderRule())); // zatim pouze testovaci (velikost radku tu ve finale asi nebude)
                gc.Graphics.TranslateTransform(c.Width + 1, 0);
                sirka_celeho_gridu += c.Width + 1;
            }

            gc.Graphics.Restore(gs);

            gc.Graphics.DrawLine(new Pen(this.line_color), new Point(0, this.vyska_hlavicky), new Point(sirka_celeho_gridu, this.vyska_hlavicky)); //TODO: konstanta (vyska hlavicky)

            gc.Graphics.FillRectangle(Brushes.DarkGray, new Rectangle(hscrollbar.Value, 0, this.sirka_rowselectoru, this.vyska_hlavicky));

            gc.Graphics.TranslateTransform(0, this.vyska_hlavicky + 1);

            //vykresli jednotlive radky
            int y = 0;
            for (int i = vscrollbar.Value; i < this.rows.Count && y<this.ClientSize.Height; i++)
            {
                IRow radek = this.rows[i];
                int row_height = radek.Height;

                y += row_height + 1;
                gc.Graphics.SetClip(new Rectangle(0, 0, sirka_celeho_gridu, row_height));

                /*IFormatter formatter = this.formatters.getMatchFormatter(radek);
                radek.Formatter = formatter;*/

                gc.Graphics.TranslateTransform(this.sirka_rowselectoru + 1, 0);

                radek.render(gc, this.columns);

                gc.Graphics.TranslateTransform(-this.sirka_rowselectoru - 1 + hscrollbar.Value, 0);

                //vykresli RowSelector
                gc.Graphics.SetClip(new Rectangle(0,0, this.sirka_rowselectoru, radek.Height));
                radek.renderRowSelector(gc);

                gc.Graphics.TranslateTransform(-hscrollbar.Value, 0);

                gc.Graphics.ResetClip();

                gc.Graphics.DrawLine(new Pen(this.line_color), new Point(0, radek.Height), new Point(sirka_celeho_gridu, radek.Height));

                gc.Graphics.TranslateTransform(0, row_height + 1);
            }

            gc.Graphics.ResetTransform();

            gc.Graphics.DrawLine(new Pen(this.line_color), new Point(this.sirka_rowselectoru, 0), new Point(this.sirka_rowselectoru, this.ClientSize.Height));

            gc.Graphics.TranslateTransform(-hscrollbar.Value + this.sirka_rowselectoru + 1, 0);

            int x=0;

            foreach (IColumn c in this.columns)
            {
                x += c.Width+1;
                if (x - hscrollbar.Value > 0) gc.Graphics.DrawLine(new Pen(this.line_color), new Point(x, 0), new Point(x, this.ClientSize.Height));
            }

            base.OnPaint(e);
        }

        protected void countWidthForElasticColumn()
        {
            int sirka_neelastickych = this.sirka_rowselectoru + 1; //sirka sedych obdelniku pred radkem
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
                    sirka_neelastickych += c.Width + 1;
                }
            }

            if (elasticke_sloupce.Count > 0)
            {
                sirka_neelastickych += elasticke_sloupce.Count;
                int zbyvajici_plocha = this.ClientSize.Width - vscrollbar.Width - sirka_neelastickych;

                foreach (int index in elasticke_sloupce)
                {
                    float pomer = (float)this.columns[index].RatioWidth / sirka_elastickych;
                    int nova_sirka = (int)Math.Floor((float)zbyvajici_plocha * pomer);
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            /*const int x_tolerance = 2;
            const int y_tolerance = 2;

            this.mouse_state.setLastLocation(e.Location);*/

            base.OnMouseMove(e);

            /*if (!this.mouse_state.left_down)
            {
                // resize kurzor u RowSelecturu
                if (this.isBetween(e.Location.X, this.sirka_rowselectoru, x_tolerance))
                {
                    this.Cursor = Cursors.VSplit;
                    this.mouse_state.mouse_action = MouseAction.changeRowSelectorWidth;
                    return;
                }

                // resize sloupcu
                int x = this.sirka_rowselectoru;

                if (e.Location.Y <= this.vyska_hlavicky)
                {
                    foreach (IColumn c in this.columns)
                    {
                        x += c.Width + 1;
                        if (this.isBetween(e.Location.X, x - hscrollbar.Value, x_tolerance))
                        {
                            this.Cursor = Cursors.VSplit;//puvodne: Cursors.SizeWE;
                            this.mouse_state.mouse_action = MouseAction.changeColumnWidth;
                            this.mouse_state.parameters = c;
                            return;
                        }
                    }
                }

                //resize radku
                int y = this.vyska_hlavicky; ;
                if (e.Location.X < this.sirka_rowselectoru)
                {
                    for (int i = vscrollbar.Value; i < rows.Count && y < this.ClientSize.Height; i++)
                    {
                        IRow row = Rows[i];
                        y += row.Height + 1;
                        if (this.isBetween(e.Location.Y, y, y_tolerance))
                        {
                            this.Cursor = Cursors.HSplit;//Cursors.SizeNS;
                            this.mouse_state.mouse_action = MouseAction.changeRowHeight;
                            this.mouse_state.parameters = i;
                            return;
                        }
                    }
                }

                //defaultni kurzor
                this.Cursor = Cursors.Arrow;
                this.mouse_state.mouse_action = MouseAction.none;
            }
            else
            {
                //leve tlacitko mysi je stisknuto
                switch (this.mouse_state.mouse_action)
                {
                    case MouseAction.changeRowSelectorWidth:
                        this.RowSelectorWidth += this.mouse_state.getDeltaX();
                        this.setScrollBars();
                        Refresh();
                        break;
                    case MouseAction.changeRowHeight:
                        int row_index = (int)this.mouse_state.parameters;
                        this.Rows[row_index].Height += this.mouse_state.getDeltaY();
                        Refresh();
                        break;
                    case MouseAction.changeColumnWidth:
                        IColumn c = (IColumn)this.mouse_state.parameters;
                        //spocita pozici
                        int x_pos = this.sirka_rowselectoru+1;
                        foreach (IColumn col in this.columns)
                        {
                            x_pos += col.Width;
                            if (col == c)
                            {
                                break;
                            }
                        }

                        //overi, zda by nemel odblokovat
                        if (this.mouse_state.block_left)
                        {
                            if (this.mouse_state.last_location.X > x_pos)
                            {
                                this.mouse_state.block_left = false;
                                this.mouse_state.last_last_location.X = x_pos;
                            }
                        }

                        if (this.mouse_state.block_right)
                        {
                            if (this.mouse_state.last_location.X < x_pos)
                            {
                                this.mouse_state.block_right = false;
                                this.mouse_state.last_last_location.X = x_pos;
                            }
                        }

                        if (this.mouse_state.block_left || this.mouse_state.block_right)
                        {
                            return;
                        }*/

                        /*if (!this.isBetween(this.mouse_state.last_location.X - this.mouse_state.getDeltaX(), x_pos, x_tolerance))
                        {
                            return;
                        }*/

                        /*if (c.Elastic)
                        {
                            int index_c = this.columns.IndexOf(c);
                            if (this.columns.Count > index_c + 1)
                            {
                                //neni to posledni sloupec
                                IColumn next_col = this.columns[index_c + 1];

                                int delta = this.mouse_state.getDeltaX();

                                this.disable_elastics = true;
                                if (c.Width + delta >= c.MinimalWidth && next_col.Width - delta >= next_col.MinimalWidth)
                                {
                                    c.Width += delta;
                                    next_col.Width -= delta;
                                }
                                else
                                {
                                    //je treba zapnout jeden z bloku
                                    if (c.Width + delta >= c.MinimalWidth)
                                    {
                                        this.mouse_state.block_right = true;
                                    }
                                    else
                                    {
                                        this.mouse_state.block_left = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            int delta = this.mouse_state.getDeltaX();
                            if (c.Width + delta < c.MinimalWidth)
                            {
                                this.mouse_state.block_left = true;
                            }

                            c.Width += delta;
                        }
                        
                        this.setScrollBars();
                        Refresh();
                        break;
                }
            }*/

        }

        protected bool isBetween(int test_pos, int pos, int tolerance)
        {
            if (test_pos >= pos - tolerance && test_pos <= pos + tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            //this.mouse_state.left_down = true;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            /*this.mouse_state.left_down = false;

            if (this.disable_elastics)
            {
                this.disable_elastics = false;
                //prepocita pomery
                List<int> elasticke_sloupce = new List<int>();
                int sirka_elastickych = 0;
                for (int i = 0; i < this.columns.Count; i++)
                {
                    IColumn c = this.columns[i];
                    if (c.Elastic)
                    {
                        elasticke_sloupce.Add(i);
                        sirka_elastickych += c.Width;
                    }
                }

                foreach (int index in elasticke_sloupce)
                {
                    IColumn c = this.columns[index];

                    c.RatioWidth = (float)c.Width / sirka_elastickych;
                }

                Refresh();
            }*/

            base.OnMouseUp(e);
        }

        protected override void OnResize(EventArgs e)
        {
            setScrollBars();
            dunaGridHeaderRow1.Location = new Point(dunaGridRowSelectorsColumn1.Width, 0);
            dunaGridHeaderRow1.Width = this.ClientSize.Width - dunaGridRowSelectorsColumn1.Width - vscrollbar.Width;

            dunaGridRowSelectorsColumn1.Location = new Point(0, dunaGridHeaderRow1.Height);
            dunaGridRowSelectorsColumn1.Height = this.ClientSize.Height - dunaGridHeaderRow1.Height - hscrollbar.Height;

            //resize basegridu
            baseGrid1.Location = new Point(dunaGridRowSelectorsColumn1.Width, dunaGridHeaderRow1.Height);
            baseGrid1.Size = new Size(this.ClientSize.Width - dunaGridRowSelectorsColumn1.Width - vscrollbar.Width, this.ClientSize.Height - dunaGridHeaderRow1.Height - hscrollbar.Height);

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
            SetRowSelectors();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
        }

        protected void SetRowSelectors()
        {
            dunaGridRowSelectorsColumn1.Rows = this.Rows;


        }

        #endregion
    }
}
