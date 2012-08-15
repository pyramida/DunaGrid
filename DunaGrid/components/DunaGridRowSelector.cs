using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DunaGrid.rows;

namespace DunaGrid.components
{
    public partial class DunaGridRowSelector : AbstractSystemHeader
    {
        public delegate void RowResizeEventHandler(object sender, RowResizeEventArgs e);

        private IRow linked_row = null;
        private bool resize_top = true;
        private bool resize_bottom = true;
        private MouseState mouse_state = new MouseState();

        public event RowResizeEventHandler RowResize;

        public IRow Row
        {
            get
            {
                return this.linked_row;
            }
            set
            {
                this.linked_row = value;
                this.Refresh();
            }
        }

        public new int Height
        {
            get
            {
                return this.Row.Height;
            }

            set
            {
                this.Row.Height = value;
                this.Refresh();
            }
        }

        public bool EnableTopResize
        {
            get
            {
                return this.resize_top;
            }
            set
            {
                this.resize_top = value;
            }
        }

        public bool EnableBottomResize
        {
            get
            {
                return this.resize_bottom;
            }
            set
            {
                this.resize_bottom = value;
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.Row == null)
            {
                base.SetBoundsCore(x, y, width, height, specified);
            }
            else
            {
                base.SetBoundsCore(x, y, width, this.Row.Height + 1, specified);
            }
        }

        public new void Refresh()
        {
            this.SetBoundsCore(this.Location.X, this.Location.Y, this.Width, this.Height, BoundsSpecified.Width | BoundsSpecified.Height);

            base.Refresh();
        }
        

        public DunaGridRowSelector(IRow row) : base()
        {
            this.Row = row;
            InitializeComponent();
            this.HoverAreaPadding = new Padding(0, 3, 0, 3);
            DoubleBuffered = true;
        }

        private void DunaGridRowSelector_Load(object sender, EventArgs e)
        {

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.mouse_state.left_down = true;
            this.mouse_state.setLastLocation(e.Location);

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.mouse_state.left_down = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.mouse_state.left_down)
            {
                this.mouse_state.setLastLocation(e.Location);

                this.Height += this.mouse_state.getDeltaY();

                RowResizeEventArgs e_new = new RowResizeEventArgs(e, RowResizeEventArgs.ResizeSide.Bottom);
                OnRowResize(e_new);
            }

            base.OnMouseMove(e);
        }

        protected virtual void OnRowResize(RowResizeEventArgs e)
        {
            if (RowResize != null)
            {
                RowResize(this, e);
            }
        }
    }
}
