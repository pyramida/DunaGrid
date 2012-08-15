using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace DunaGrid.components
{
    /// <summary>
    /// Zobrazuje gridy z kolekce - klasicky grid, filterrow atd...
    /// </summary>

    public partial class BaseGridsContainer : UserControl, IXScrollable
    {
        protected ObservableCollection<AbstractGrid> items = new ObservableCollection<AbstractGrid>();

        protected int posun_x = 0;

        public event EventHandler NeedRefresh;

        protected bool right_border = false;

        private const int BORDER_WIDTH = 3;

        public bool RightBorder
        {
            get
            {
                return this.right_border;
            }
            set
            {
                this.right_border = value;
            }
        }

        public int MoveX
        {
            get
            {
                return posun_x;
            }
            set
            {
                posun_x = value;
                this.SuspendLayout();
                foreach (AbstractGrid ag in this.Items)
                {
                    ag.MoveX = value;
                }
                //this.Refresh();
                this.ResumeLayout();
            }
        }

        public ObservableCollection<AbstractGrid> Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
            }
        }

        public List<BaseGrid> MainGrid
        {
            get
            {
                List<BaseGrid> temp = new List<BaseGrid>();

                foreach (AbstractGrid g in Items)
                {
                    if (g is BaseGrid)
                    {
                        temp.Add((BaseGrid)g);
                    }
                }

                return temp;
            }
        }

        public BaseGridsContainer()
        {
            InitializeComponent();
            items.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(items_CollectionChanged);
        }

        private void items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //zpracuje jednotlive komponenty a logicky je rozmisti
            this.Controls.Clear();
            foreach (AbstractGrid g in this.Items)
            {
                g.NeedResize += new AbstractGrid.EventHandler(g_NeedResize);
                this.Controls.Add(g);
            }
        }

        private void g_NeedResize()
        {
            this.Refresh();
            this.OnNeedRefresh();
        }

        protected virtual void OnNeedRefresh()
        {
            if (NeedRefresh!=null)
            {
                NeedRefresh(this, new EventArgs());
            }
        }

        protected void setSizeToControl()
        {
            int vyska_vrchnich_gridu = 0;

            List<AbstractGrid> top = new List<AbstractGrid>();
            List<AbstractGrid> bottom = new List<AbstractGrid>();
            List<AbstractGrid> fill = new List<AbstractGrid>();

            foreach (AbstractGrid g in Items)
            {
                if (g.AutoSizeMode == GridSizeMode.Fill)
                {
                    fill.Add(g);
                }
                else if (g.Position == GridPosition.Bottom)
                {
                    bottom.Add(g);
                }
                else
                {
                    top.Add(g);
                }
            }

            int top_height = 0, bottom_height = 0;

            this.SetFullGrids(top, GridPosition.Top, ref top_height, ref bottom_height);
            this.SetFullGrids(bottom, GridPosition.Bottom, ref top_height, ref bottom_height);

            this.SetFillGrids(fill, ref top_height, ref bottom_height); 
        }

        private int GetBorderWidth()
        {
            if (this.RightBorder)
            {
                return BORDER_WIDTH;
            }
            else
            {
                return 0;
            }
        }

        private void SetFullGrids(List<AbstractGrid> grids, GridPosition pos, ref int top, ref int bottom)
        {
            grids.Sort();

            if (pos == GridPosition.Bottom)
            {
                grids.Reverse();
            }

            List<AbstractGrid> fill = new List<AbstractGrid>();

            foreach (AbstractGrid g in grids)
            {
                if (pos == GridPosition.Bottom)
                {
                    g.Location = new Point(0, this.Height - g.Height - bottom);
                    bottom += g.Height;
                }
                else
                {
                    g.Location = new Point(0, top);
                    top += g.Height;
                }
                g.Width = this.Width - this.GetBorderWidth();
            }
        }

        private void SetFillGrids(List<AbstractGrid> grids, ref int top, ref int bottom)
        {
            if (grids.Count>0)
            {
                grids.Sort();

                int vyska = (this.Height - top - bottom) / grids.Count;

                foreach (AbstractGrid g in grids)
                {
                    g.Location = new Point(0, top);
                    top += vyska;
                    g.Height = vyska;
                    g.Width = this.Width - this.GetBorderWidth();
                }
            }
        }

        public override void Refresh()
        {
            setSizeToControl();
            base.Refresh();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            setSizeToControl();
        }
    }
}
