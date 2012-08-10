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
    public partial class IntegerEditor : AbstractGridEditor
    {
        public IntegerEditor() : base()
        {
            InitializeComponent();
            textBox1.LostFocus += new EventHandler(textBox1_LostFocus);
            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
        }

        public override object Value
        {
            set
            {
                textBox1.Text = value.ToString();
            }

            get
            {
                int i;
                if (Int32.TryParse(textBox1.Text, out i))
                {
                    return i;
                }
                else
                {
                    return null;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int result;
            if (Int32.TryParse(this.textBox1.Text, out result))
            {
                this.valid_data = this.ValidateData();
            }
            else
            {
                this.valid_data = false;
            }

            if (this.valid_data)
            {
                textBox1.BackColor = Color.White;
            }
            else
            {
                textBox1.BackColor = Color.Red;
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
    }
}
