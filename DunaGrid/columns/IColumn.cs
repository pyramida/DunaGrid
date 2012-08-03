using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using DunaGrid.components.editors;
using DunaGrid.columns.validators;

namespace DunaGrid.columns
{
    public interface IColumn
    {
        event EventHandler WidthChanged;

        int Width { get; set; } //sirka bunky
        int MinimalWidth { get; set; } //minimalni sirka bunky
        float RatioWidth { get; set; } 
        string HeadText { get; set; } //text, co se vypise v hlavicce sloupce
        string Name { get; set; } //vnitrni pojmenovani sloupce
        bool Visible { get; set; } //je sloupec viditelny?
        bool ReadOnly { get; set; } //je sloupec jenom pro cteni?
        DunaGridView ParentGrid { get; set; } //odkaz na grid, ve kterem se sloupec nachazi
        int DataSourceColumnIndex { set; get; } //cislo sloupce v datovem zdroji (od nuly)
        bool Elastic { set; get; }
        bool Pinned { set; get; }
        ValidatorCollection Validators { get; }

        void renderHead(GraphicsContext g, ColumnContext context);
        void renderCell(GraphicsContext g, object value, CellRenderState render_state = CellRenderState.Normal);
        void renderCellBackground(GraphicsContext g, CellRenderState render_state = CellRenderState.Normal);
        AbstractGridEditor GetEditControl();
    }
}
