using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DunaGrid.components
{
    /// <summary>
    /// predhudce vseho co pod sebe vykresluje systemove hlavicky gridu
    /// podpora visualstyles tak i standardniho vykreslovani 
    /// </summary>
    abstract public partial class AbstractSystemHeader : UserControl
    {
        protected state visual_state = state.Normal;
        protected Rectangle hover_area = Rectangle.Empty;
        private Padding hover_area_padding=Padding.Empty;

        public cellPosition PositionInRow
        {
            get;
            set;
        }

        public Orientation Orientation
        {
            get;
            set;
        }

        public Padding HoverAreaPadding
        {
            get
            {
                /*Padding p = new Padding();
                p.Left = this.hover_area.X;
                p.Right = this.Width - this.hover_area.Width - this.hover_area.Left;
                p.Top = this.hover_area.Y;
                p.Bottom = this.Height - this.hover_area.Y - this.hover_area.Height;

                return p;*/
                return this.hover_area_padding;
            }
            set
            {
                Rectangle new_area = getRectangleFromPadding(value);

                this.hover_area = new_area;

                this.hover_area_padding = value;
            }
        }

        private Rectangle getRectangleFromPadding(Padding value)
        {
            Rectangle new_area = new Rectangle();
            new_area.X = value.Left;
            new_area.Width = this.Width - value.Left - value.Right;
            new_area.Y = value.Top;
            new_area.Height = this.Height - value.Top - value.Bottom;
            return new_area;
        }

        public virtual bool IsInHoverArea(Point mouse_position)
        {
            if (this.hover_area == Rectangle.Empty)
            {
                //hover zona neni definovana
                return true;
            }
            else
            {
                return this.hover_area.Contains(mouse_position);
            }
        }

        public AbstractSystemHeader()
        {
            InitializeComponent();
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

                if (this.Orientation == System.Windows.Forms.Orientation.Horizontal)
                {
                    vsr.DrawBackground(e.Graphics, this.ClientRectangle);
                }
                else
                {
                    Bitmap temp = new Bitmap(this.ClientSize.Height, this.ClientSize.Width); //nebyla by lepsi nejaka cache?
                    Graphics g = Graphics.FromImage(temp);

                    vsr.DrawBackground(g, new Rectangle(0, 0, temp.Width, temp.Height));

                    temp.RotateFlip(RotateFlipType.Rotate90FlipX);
                    
                    e.Graphics.DrawImage(temp, this.ClientRectangle);
                }

            }
            else
            {
                Rectangle r;
                if (this.Orientation == Orientation.Horizontal)
                {
                    r = new Rectangle(this.ClientRectangle.Location.X - 2, this.ClientRectangle.Location.Y, this.ClientRectangle.Width + 4, this.Height);
                }
                else
                {
                    r = new Rectangle(this.ClientRectangle.Location.X, this.ClientRectangle.Location.Y - 2, this.ClientRectangle.Width, this.Height + 4);
                }
                
                switch (this.visual_state)
                {
                    case state.Pressed:
                        ControlPaint.DrawButton(e.Graphics, r, ButtonState.Pushed | ButtonState.Flat);
                        break;
                    default:
                        ControlPaint.DrawButton(e.Graphics, r, ButtonState.Inactive);
                        if (this.PositionInRow != cellPosition.first)
                        {
                            if (this.Orientation == Orientation.Horizontal)
                            {
                                ControlPaint.DrawBorder3D(e.Graphics, 0, 3, 1, this.Height - 6, Border3DStyle.Etched, Border3DSide.Right);
                            }
                            else
                            {
                                ControlPaint.DrawBorder3D(e.Graphics, 3, 0, this.Width - 6, 1, Border3DStyle.Etched, Border3DSide.Bottom);
                            }
                        }

                        if (this.PositionInRow != cellPosition.last)
                        {
                            if (this.Orientation == Orientation.Horizontal)
                            {
                                ControlPaint.DrawBorder3D(e.Graphics, this.Width, 3, 1, this.Height - 6, Border3DStyle.Etched, Border3DSide.Right);
                            }
                            else
                            {
                                ControlPaint.DrawBorder3D(e.Graphics, 3, this.Height, this.Width - 6, 1, Border3DStyle.Etched, Border3DSide.Bottom);
                            }
                        }

                        break;
                }
            }

            base.OnPaint(e);

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

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.visual_state != state.Normal && e.Button == MouseButtons.Left)
            {
                this.visual_state = state.Normal;
                Refresh();
            }

            base.OnMouseUp(e);
        }

        protected override void OnResize(EventArgs e)
        {
            this.hover_area = this.getRectangleFromPadding(this.hover_area_padding);
            base.OnResize(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.IsInHoverArea(e.Location))
            {
                if (this.visual_state != state.Pressed)
                {
                    this.visual_state = state.Pressed;
                    Refresh();
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (IsInHoverArea(e.Location))
            {
                this.visual_state = state.Hot;
                Refresh();
                this.Cursor = Cursors.Arrow;
            }
            else
            {
                this.visual_state = state.Normal;
                Refresh();
                this.Cursor = Cursors.VSplit;
            }

            base.OnMouseMove(e);
        }

        protected enum state
        {
            Normal,
            Hot,
            Pressed
        }

        public enum cellPosition
        {
            first,
            middle,
            last
        }

        private void AbstractSystemHeader_Load(object sender, EventArgs e)
        {

        }
    }
}
