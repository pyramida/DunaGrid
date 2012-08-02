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
    public partial class AbstractGridEditor : UserControl, IEditorControl
    {
        protected object value;
        protected bool valid = true;

        public event EventHandler EndEdit;

        public AbstractGridEditor()
        {
            InitializeComponent();
        }

        public virtual object Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public virtual int RowIndex
        {
            get;
            set;
        }


        public string ColumnName
        {
            get;
            set;
        }

        protected virtual void OnEndEditing()
        {
            if (EndEdit != null)
            {
                EndEdit(this, EventArgs.Empty);
            }
        }

        public void EndEditing()
        {
            if (this.valid)
            {
                this.OnEndEditing();
            }

            this.Dispose();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.valid = false;
                this.Dispose();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                this.EndEditing();
            }
            base.OnKeyDown(e);
        }
    }
}
