using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DunaGrid.columns.validators;

namespace DunaGrid.components.editors
{
    interface IEditorControl
    {
        event EventHandler EndEdit;

        object Value { get; set; }

        int RowIndex { get; set; }

        bool EditCommited { get; set; }

        string ColumnName { get; set; }

        ValidatorCollection Validators { get; set; }

        void EndEditing();
    }
}
