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
    }
}
