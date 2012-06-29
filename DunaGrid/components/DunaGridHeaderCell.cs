using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using DunaGrid.columns;


namespace DunaGrid.components
{
    public delegate void CellResizeEventHandler(object sender, CellResizeEventArgs e);

    public partial class DunaGridHeaderCell : UserControl
    {
        public event CellResizeEventHandler CellResize;
        public event CellResizeEventHandler CellResizeStart;
        public event CellResizeEventHandler CellResizeEnd;

        private state visual_state = state.Normal;
        private bool resize_active = false;
        private IColumn linked_column = null;
        private CellResizeEventArgs.ResizeSide resize_side = 0;
        private MouseState mouse_state = new MouseState();

        public bool EnableLeftResize { get; set; }

        public bool EnableRightResize { get; set; }

        public int ResizeTolerance { get; set; }

        public IColumn LinkedColumn 
        {
            get
            {
                return this.linked_column;
            }

            set
            {
                this.linked_column = value;
            }
        }

        public new int Width
        {
            get
            {
                return this.linked_column.Width;
            }

            set
            {
                this.linked_column.Width = value;
                this.Refresh();
            }
        }

        public new string Name
        {
            get
            {
                return this.linked_column.Name;
            }

            set
            {
                this.linked_column.Name = value;
            }
        }

        public new Size MinimumSize
        {
            get
            {
                return new Size(this.linked_column.MinimalWidth, 0);
            }

            set
            {
                this.linked_column.MinimalWidth = ((Size)value).Width;
            }
        }

        public new string Text
        {
            get
            {
                return this.linked_column.HeadText;
            }

            set
            {
                this.linked_column.HeadText = value;
            }
        }

        public DunaGridHeaderCell(IColumn col)
        {
            linked_column = col;

            InitializeComponent();
            EnableLeftResize = true;
            EnableRightResize = true;
            ResizeTolerance = 5;
            this.ResizeRedraw = true;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, this.linked_column.Width, height, specified);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Application.RenderWithVisualStyles)
            {
                VisualStyleRenderer vsr;

                switch (this.visual_state)
                {
                    case state.Hot:
                        vsr = new VisualStyleRenderer(VisualStyleElement.Header.Item.Hot);
                        break;
                    case state.Pressed:
                        vsr = new VisualStyleRenderer(VisualStyleElement.Header.Item.Pressed);
                        break;
                    default:
                        vsr = new VisualStyleRenderer(VisualStyleElement.Header.Item.Normal);
                        break;
                }


                vsr.DrawBackground(e.Graphics, this.ClientRectangle);

            }
            else
            {
                switch (this.visual_state)
                {
                    case state.Pressed:
                        ControlPaint.DrawButton(e.Graphics, this.ClientRectangle, ButtonState.Pushed | ButtonState.Flat);
                        break;
                    default:
                        ControlPaint.DrawButton(e.Graphics, this.ClientRectangle, ButtonState.Inactive);
                        break;
                }
            }

            //vykresli text
            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.ClientRectangle, Color.Black, TextFormatFlags.LeftAndRightPadding | TextFormatFlags.VerticalCenter);
            base.OnPaint(e);
            
        }

        protected virtual void OnCellResize(CellResizeEventArgs e)
        {
            if (CellResize != null)
            {
                CellResize(this, e);
            }
        }

        protected virtual void OnCellResizeStart(CellResizeEventArgs e)
        {
            if (CellResizeStart != null)
            {
                CellResizeStart(this, e);
            }
        }

        protected virtual void OnCellResizeEnd(CellResizeEventArgs e)
        {
            if (CellResizeEnd != null)
            {
                CellResizeEnd(this, e);
            }
        }

        protected void addWidth(int delta_width)
        {
            if (this.resize_side != CellResizeEventArgs.ResizeSide.Right)
            {
                this.Width += delta_width;
                return;
            }

            if (this.mouse_state.block_left)
            {
                //zkusi odblokovat
                if (this.mouse_state.last_location.X >= this.MinimumSize.Width)
                {
                    this.mouse_state.block_left = false;
                }
                else
                {
                    return;
                }
            }

            int new_width = this.Width + delta_width;
            if (new_width > this.MinimumSize.Width)
            {
                this.Width = new_width;
            }
            else
            {
                int rozdil = this.MinimumSize.Width - new_width;
                if (rozdil > 0)
                {
                    this.Width = this.MinimumSize.Width;
                }
                this.mouse_state.block_left = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.mouse_state.setLastLocation(e.Location);
            if (this.resize_active)
            {
                int delta = this.mouse_state.getDeltaX();
                if (this.resize_side == CellResizeEventArgs.ResizeSide.Left)
                {
                    Point new_location = new Point(this.Location.X + delta, this.Location.Y);

                    if (!new_location.Equals(this.Location))
                    {
                        this.Location = new_location;
                        this.mouse_state.setLastLocation(new Point(e.X - delta, e.Y));
                    }
                    //this.Width -= delta;
                    //this.addWidth(-delta);
                }
                else
                {
                    //this.Width += delta;
                    this.addWidth(delta);
                }
                OnCellResize(new CellResizeEventArgs(e, this.resize_side)); //vyvola udalost
            }
            else
            {
                if (IsInResizeArea(e.Location))
                {
                    this.visual_state = state.Normal;
                    Refresh();
                    this.Cursor = Cursors.VSplit;
                }
                else
                {
                    this.visual_state = state.Hot;
                    Refresh();
                    this.Cursor = Cursors.Arrow;
                }
            }

            base.OnMouseMove(e);
        }

        protected bool IsInResizeArea(Point mouse_position)
        {
            if (this.EnableLeftResize && mouse_position.X < this.ResizeTolerance)
            {
                return true;
            }
            else if (this.EnableRightResize && mouse_position.X > this.Width - this.ResizeTolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.visual_state != state.Normal)
            {
                this.visual_state = state.Normal;
                Refresh();
            }
            this.Cursor = Cursors.Arrow;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.IsInResizeArea(e.Location))
                {
                    this.resize_active = true;
                    //zjisti stranu
                    if (e.X < this.ResizeTolerance)
                    {
                        this.resize_side = CellResizeEventArgs.ResizeSide.Left;
                    }
                    else
                    {
                        this.resize_side = CellResizeEventArgs.ResizeSide.Right;
                    }
                    this.OnCellResizeStart(new CellResizeEventArgs(e, this.resize_side));
                }
                else if (this.visual_state != state.Pressed)
                {
                    this.visual_state = state.Pressed;
                    Refresh();
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.visual_state != state.Normal && e.Button == MouseButtons.Left)
            {
                this.visual_state = state.Normal;
                Refresh();
            }

            if (this.resize_active)
            {
                this.OnCellResizeEnd(new CellResizeEventArgs(e, this.resize_side));
                this.resize_active = false;
            }
            
            base.OnMouseUp(e);
        }

        public new void Refresh()
        {
            this.SetBoundsCore(this.Location.X, this.Location.Y, this.Width, this.Height, BoundsSpecified.Width | BoundsSpecified.Height);

            base.Refresh();
        }

        enum state
        {
            Normal,
            Hot,
            Pressed
        }

        private void DunaGridHeaderCell_Load(object sender, EventArgs e)
        {

        }
    }
}
