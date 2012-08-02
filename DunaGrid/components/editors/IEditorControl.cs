using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DunaGrid.components.editors
{
    interface IEditorControl
    {
        event EventHandler EndEdit;

        object Value { get; set; }

        int RowIndex { get; set; }

        string ColumnName { get; set; }

        void EndEditing();
    }
}
