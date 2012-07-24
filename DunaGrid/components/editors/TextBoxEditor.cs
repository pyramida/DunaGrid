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
        }
    }
}
