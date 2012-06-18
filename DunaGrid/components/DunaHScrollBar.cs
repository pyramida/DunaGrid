using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DunaGrid.components
{
    /// <summary>
    /// vylepseny horizontalni scrollbar s mistem pro navigaci (nebo cokoli jineho)
    /// + moznost pridat odsazeni od kraje
    /// </summary>
    public partial class DunaHScrollBar : UserControl
    {
        protected int right_margin = 0; //odsazeni od prave strany
        protected int navigatebar_width = 100;

        new public event ScrollEventHandler Scroll;

        public int RightMargin
        {
            get
            {
                return this.right_margin;
            }
            set
            {
                this.right_margin = value;
                updatePositions();
            }
        }

        public int NavigateBarWidth
        {
            get
            {
                return this.navigatebar_width;
            }
            set
            {
                this.navigatebar_width = value;
                updatePositions();
                updateSizes();
            }
        }
        
        public int Value
        {
            get
            {
                return hScrollBar1.Value;
            }

            set
            {
                hScrollBar1.Value = value;
            }
        }

        public int MaximumValue
        {
            get
            {
                return hScrollBar1.Maximum;
            }

            set
            {
                hScrollBar1.Maximum = value;
            }
        }

        public int MinimumValue
        {
            get
            {
                return hScrollBar1.Minimum;
            }

            set
            {
                hScrollBar1.Minimum = value;
            }
        }

        public int SmallChange
        {
            get
            {
                return hScrollBar1.SmallChange;
            }

            set
            {
                hScrollBar1.SmallChange = value;
            }
        }

        public int LargeChange
        {
            get
            {
                return hScrollBar1.LargeChange;
            }

            set
            {
                hScrollBar1.LargeChange = value;
            }
        }

        public Panel navigationPanel
        {
            get
            {
                return this.navigatePanel;
            }

            set
            {
                this.navigatePanel = value;
            }
        }


        public DunaHScrollBar()
        {
            InitializeComponent();
            updatePositions();
        }

        private void DunaHScrollBar_Load(object sender, EventArgs e)
        {

        }

        protected override void OnResize(EventArgs e)
        {
            //prepocita velikost scrollbaru + panelu + zajisti margin
            setScrollBarWidth();
            updateSizes();
            base.OnResize(e);
        }

        protected void setScrollBarWidth()
        {
            this.hScrollBar1.Width = this.Width - this.right_margin - navigatePanel.Width;
        }

        protected void setPosition()
        {
            navigatePanel.Location = Point.Empty;

            hScrollBar1.Location = new Point(navigatePanel.Width, 0);

            updateSizes();
        }

        protected void updateSizes()
        {
            navigatePanel.Height = this.Height;
            hScrollBar1.Height = this.Height;
            navigatePanel.Width = this.navigatebar_width;
        }

        protected void updatePositions()
        {
            setScrollBarWidth();
            setPosition();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (Scroll != null)
            {
                Scroll(this, e);
            }
        }
    }
}
