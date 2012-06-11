using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DunaGrid.components;
using DunaGrid.dataReaders;

namespace DunaGrid
{
    public partial class DunaGrid : Control
    {
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

        protected Padding padding = new Padding(3);

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

            this.Controls.Add(hscrollbar);

            //prida datareadery do kolekce
            this.data_readers.Add(new BindingSourceDataReader(this.data_readers));
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

        #region Vykreslovani
        /// <summary>
        /// Vykresleni gridu
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        /// <summary>
        /// vykresleni pozadi
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }
        #endregion
    }
}
