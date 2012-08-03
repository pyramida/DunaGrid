using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DunaGrid.columns.validators;

namespace DunaGrid.components.editors
{
    public partial class AbstractGridEditor : UserControl, IEditorControl
    {
        protected object value;
        protected bool valid = true;
        protected ValidatorCollection validators;
        protected bool commited = false;
        protected bool valid_data = true;

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

            this.valid = false;

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
                if (this.valid_data)
                {
                    this.EndEditing();
                }
                else
                {
                    System.Media.SystemSounds.Beep.Play(); //tohle jeste uplne nefrci: pipa to, ale provede to zalomeni
                }
            }
            base.OnKeyDown(e);
        }


        public ValidatorCollection Validators
        {
            get
            {
                if (this.validators == null)
                {
                    return new ValidatorCollection();
                }
                else
                {
                    return this.validators;
                }
            }

            set
            {
                this.validators = value;
            }
        }

        protected bool ValidateData()
        {
            return this.Validators.GetResult(this.Value);
        }


        public bool EditCommited
        {
            get { return this.commited; }
            set { this.commited = value; }
        }
    }
}
