using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DunaGrid.components.editors
{
    public partial class TextBoxEditor : AbstractGridEditor
    {
        public TextBoxEditor() : base()
        {
            InitializeComponent();
            textBox1.LostFocus += new EventHandler(textBox1_LostFocus);
            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
        }

        public override Padding CellPadding
        {
            set
            {
                base.CellPadding = value;
                SetTextBoxPosition();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            this.OnKeyDown(e);
        }

        private void textBox1_LostFocus(object sender, EventArgs e)
        {
            this.OnLostFocus(e);
        }

        public new bool Focus()
        {
            return textBox1.Focus();
            //return base.Focus();
        }

        public override object Value
        {
            set
            {
                textBox1.Text = value.ToString();
            }

            get
            {
                return textBox1.Text;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.valid_data = this.ValidateData();
            if (this.valid_data)
            {
                textBox1.BackColor = Color.White;
                this.BackColor = Color.White;
            }
            else
            {
                textBox1.BackColor = Color.Red;
                this.BackColor = Color.Red;
            }
        }

        private void SetTextBoxPosition()
        {
            textBox1.Location = new Point(
                    x: this.CellPadding.Left,
                    y: this.CellPadding.Top
                );

            textBox1.Size = new Size(
                    width: this.Width - this.CellPadding.Left - this.CellPadding.Right,
                    height: this.Height - this.CellPadding.Top - this.CellPadding.Bottom
                );
        }

        private void TextBoxEditor_Load(object sender, EventArgs e)
        {

        }

        protected override void OnResize(EventArgs e)
        {
            SetTextBoxPosition();
            base.OnResize(e);
        }
    }
}
