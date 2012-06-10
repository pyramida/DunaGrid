using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DunaGrid.components;

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
                this.Update();
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
        }

        private void DunaGrid_Load(object sender, EventArgs e)
        {

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
