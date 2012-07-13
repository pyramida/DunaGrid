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
    public partial class BaseGridsContainer : UserControl
    {
        protected ObservableCollection<AbstractGrid> items = new ObservableCollection<AbstractGrid>();

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
                this.Controls.Add(g);
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
                g.Width = this.Width;
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
                    g.Width = this.Width;
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            setSizeToControl();
        }
    }
}
