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
    public partial class DunaGridHeaderCell : UserControl
    {
        private state visual_state = state.Normal;

        [BrowsableAttribute(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text { get; set; }

        public DunaGridHeaderCell()
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

        protected override void OnMouseEnter(EventArgs e)
        {
            if (this.visual_state != state.Hot)
            {
                this.visual_state = state.Hot;
                Refresh();
            }
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.visual_state != state.Normal)
            {
                this.visual_state = state.Normal;
                Refresh();
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.visual_state != state.Pressed && e.Button == MouseButtons.Left)
            {
                this.visual_state = state.Pressed;
                Refresh();
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
            base.OnMouseUp(e);
        }

        enum state
        {
            Normal,
            Hot,
            Pressed
        }
    }
}
